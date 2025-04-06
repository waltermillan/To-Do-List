using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TasksController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _loggingService;

    public TasksController(IUnitOfWork unitOfWork, IMapper mapper, ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggingService = loggingService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Core.Entities.Task>>> Get()
    {
        var message = string.Empty;
        try
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();

            foreach (var task in tasks)
            {
                message = $"Tasks Listed | Task ID: {task.Id} Task Name: {task.Name}\n";
                _loggingService.LogInformation(message);
            }

            return _mapper.Map<List<Core.Entities.Task>>(tasks);
        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the Task. Please try again later.";
            _loggingService.LogError(message, ex);
            return StatusCode(500, new { Message = "There was an issue retrieving the Task. Please try again later.", Details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Core.Entities.Task>> Get(int id)
    {
        var message = string.Empty;
        try
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);

            if (task is null)
                return NotFound();

            message = $"Task Listed | Task ID: {task.Id} Task Name: {task.Name}";

            _loggingService.LogInformation(message);
            return _mapper.Map<Core.Entities.Task>(task);
        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the tasks. Please try again later.";
            _loggingService.LogError(message, ex);
            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Core.Entities.Task>> Post(Core.Entities.Task oTask)
    {
        var message = string.Empty;
        try
        {
            var task = Core.Factories.TaskFactory.CreateTask(oTask.Name, oTask.StateId, oTask.InitialDate, oTask.FinishDate, oTask.Done);

            _unitOfWork.Tasks.Add(task);
            await _unitOfWork.SaveAsync();

            if (task is null)
                return BadRequest();

            oTask.Id = task.Id;
            message = $"Task Created | Task ID: {task.Id} Task Name: {task.Name}";
            _loggingService.LogInformation(message);

            return CreatedAtAction(nameof(Post), new { id = oTask.Id }, oTask);

        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the tasks. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Core.Entities.Task>> Put([FromBody] Core.Entities.Task oTask)
    {
        var message = string.Empty;
        try
        {
            if (oTask is null)
                return NotFound();

            var task = await _unitOfWork.Tasks.GetByIdAsync(oTask.Id);

            if (task is null)
                return NotFound();

            message = $"Task Updated | Task ID: {task.Id} Task Name (old): {task.Name} Task Name (new): {oTask.Name} ";

            _mapper.Map(oTask, task);

            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveAsync();

            _loggingService.LogInformation(message);

            return Ok(task);
        }
        catch (Exception ex)
        {
            message = "There was an issue updating the task. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message , Details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var message = string.Empty;
        try
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);

            if (task is null)
                return NotFound();

            _unitOfWork.Tasks.Remove(task);
            await _unitOfWork.SaveAsync();

            message = $"Task Deleted | Task ID: {task.Id} Task Name: {task.Name}";
            _loggingService.LogInformation(message);

            return NoContent();
        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the tasks. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }
}