using AutoMapper;
using Core.Entities;
using Core.Interfases;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace API.Controllers;

public class TaskHistoryController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskHistoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Método existente: obtener todas los paises
    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TaskHistory>>> Get()
    {
        try
        {
            var tasks = await _unitOfWork.TasksHistory.GetAllAsync();
            return _mapper.Map<List<TaskHistory>>(tasks);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the TaskHistory. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: obtener una tarea por su ID
    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskHistory>> Get(int id)
    {
        try
        {
            var taskHistory = await _unitOfWork.TasksHistory.GetByIdAsync(id);
            if (taskHistory == null)
                return NotFound();

            return _mapper.Map<TaskHistory>(taskHistory);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the TasksHistory. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: agregar una tarea
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskHistory>> Post(TaskHistory oTaskHistory)
    {
        try
        {
            var taskHistory = _mapper.Map<TaskHistory>(oTaskHistory);
            _unitOfWork.TasksHistory.Add(taskHistory);
            await _unitOfWork.SaveAsync();
            if (taskHistory == null)
            {
                return BadRequest();
            }
            oTaskHistory.Id = taskHistory.Id;
            return CreatedAtAction(nameof(Post), new { id = oTaskHistory.Id }, oTaskHistory);

        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the TasksHistory. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: actualizar una tarea
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskHistory>> Put([FromBody] TaskHistory oTaskHistory)
    {
        try
        {
            if (oTaskHistory == null)
                return NotFound();

            var taskHistory = _mapper.Map<TaskHistory>(oTaskHistory);
            _unitOfWork.TasksHistory.Update(taskHistory);
            await _unitOfWork.SaveAsync();
            return oTaskHistory;
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the TasksHistory. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: eliminar una tarea
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var taskHistory = await _unitOfWork.TasksHistory.GetByIdAsync(id);
            if (taskHistory == null)
                return NotFound();

            _unitOfWork.TasksHistory.Remove(taskHistory);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the TasksHistory. Please try again later.", Details = ex.Message });
        }
    }
}