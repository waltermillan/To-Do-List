using AutoMapper;
using Core.Entities;
using Core.Interfases;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using TaskEntity = Core.Entities.Task;

namespace API.Controllers;
[ApiController]
[Route("api/tasks")]
public class TaskController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _loggingService;

    public TaskController(IUnitOfWork unitOfWork, IMapper mapper, ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggingService = loggingService;
    }

    // Método existente: obtener todas los paises
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

    // Método existente: obtener una tarea por su ID
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
            if (task == null)
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

    // Método existente: agregar una tarea
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

            if (task == null)
            {
                return BadRequest();
            }

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

    // Método existente: actualizar una tarea
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Core.Entities.Task>> Put([FromBody] Core.Entities.Task oTask)
    {
        var message = string.Empty;
        try
        {
            if (oTask == null)
                return NotFound();

            var task = await _unitOfWork.Tasks.GetByIdAsync(oTask.Id);
            if (task == null)
                return NotFound();

            message = $"Task Updated | Task ID: {task.Id} Task Name (old): {task.Name} Task Name (new): {oTask.Name} ";

            _mapper.Map(oTask, task);  // Este paso asigna automáticamente las propiedades

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


    // Método existente: eliminar una tarea
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var message = string.Empty;
        try
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null)
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