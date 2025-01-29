using AutoMapper;
using Core.Entities;
using Core.Interfases;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace API.Controllers;

public class TaskController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Método existente: obtener todas los paises
    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Core.Entities.Task>>> Get()
    {
        try
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();
            return _mapper.Map<List<Core.Entities.Task>>(tasks);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the Task. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: obtener una tarea por su ID
    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Core.Entities.Task>> Get(int id)
    {
        try
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            return _mapper.Map<Core.Entities.Task>(task);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the tasks. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: agregar una tarea
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Core.Entities.Task>> Post(Core.Entities.Task oTask)
    {
        try
        {
            var country = _mapper.Map<Core.Entities.Task>(oTask);
            _unitOfWork.Tasks.Add(country);
            await _unitOfWork.SaveAsync();
            if (country == null)
            {
                return BadRequest();
            }
            oTask.Id = country.Id;
            return CreatedAtAction(nameof(Post), new { id = oTask.Id }, oTask);

        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the tasks. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: actualizar una tarea
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Core.Entities.Task>> Put([FromBody] Core.Entities.Task oTask)
    {
        try
        {
            if (oTask == null)
                return NotFound();

            var country = _mapper.Map<Core.Entities.Task>(oTask);
            _unitOfWork.Tasks.Update(country);
            await _unitOfWork.SaveAsync();
            return oTask;
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the tasks. Please try again later.", Details = ex.Message });
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
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            _unitOfWork.Tasks.Remove(task);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the tasks. Please try again later.", Details = ex.Message });
        }
    }
}