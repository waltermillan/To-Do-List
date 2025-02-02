using AutoMapper;
using Core.Entities;
using Core.Interfases;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace API.Controllers;

public class TaskHistoryController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _loggingService;   

    public TaskHistoryController(IUnitOfWork unitOfWork, IMapper mapper, ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggingService = loggingService;
    }

    // Método existente: obtener todas los paises
    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TaskHistory>>> Get()
    {
        var message = string.Empty;
        try
        {
            var tasksHistory = await _unitOfWork.TasksHistory.GetAllAsync();

            foreach (var taskHistory in tasksHistory)
            {
                // mensaje de la busqueda realizada
                message = $"TasksHistory Listed | TaskHistory ID: {taskHistory.Id} Task ID: {taskHistory.TaskId} State Id: {taskHistory.StateId} Changed Date: {taskHistory.ChangedDate}\n";

                // Logueamos la busqueda realizada
                _loggingService.LogInformation(message);
            }

            return _mapper.Map<List<TaskHistory>>(tasksHistory);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the TaskHistory. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    // Método existente: obtener una tarea por su ID
    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskHistory>> Get(int id)
    {
        var message = string.Empty;
        try
        {
            var taskHistory = await _unitOfWork.TasksHistory.GetByIdAsync(id);
            if (taskHistory == null)
                return NotFound();

            // mensaje de la busqueda realizada
            message = $"TasksHistory Listed | TaskHistory ID: {taskHistory.Id} Task ID: {taskHistory.TaskId} State Id: {taskHistory.StateId} Changed Date: {taskHistory.ChangedDate}\n";

            // Logueamos la busqueda realizada
            _loggingService.LogInformation(message);

            return _mapper.Map<TaskHistory>(taskHistory);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the TasksHistory. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    // Método existente: agregar una tarea
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskHistory>> Post(TaskHistory oTaskHistory)
    {
        var message = string.Empty;
        try
        {
            // Utilizamos el patron Factory (TaskHistory), para crear la tarea historica
            var taskHistory = Core.Factories.TaskHistoryFactory.CreateTaskHistory(oTaskHistory.Id, oTaskHistory.TaskId, oTaskHistory.StateId, oTaskHistory.ChangedDate);

            // Añadir la tarea creada usando el repositorio
            _unitOfWork.TasksHistory.Add(taskHistory);
            await _unitOfWork.SaveAsync();

            if (taskHistory == null)
            {
                return BadRequest();
            }
            oTaskHistory.Id = taskHistory.Id;

            // mensaje del cambio realizado.
            message = $"TaskHistory Created | TaskHistory ID: {taskHistory.Id} TaskHistory TaskId: {taskHistory.TaskId} TaskHistory StateId: {taskHistory.StateId} TaskHistory Changed Date: {taskHistory.ChangedDate}";

            // Logeamos el cambio realizado.
            _loggingService.LogInformation(message);

            return CreatedAtAction(nameof(Post), new { id = oTaskHistory.Id }, oTaskHistory);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the TasksHistory. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    // Método existente: actualizar una tarea
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskHistory>> Put([FromBody] TaskHistory oTaskHistory)
    {
        var message = string.Empty;
        try
        {
            if (oTaskHistory == null)
                return NotFound();

            // Primero obtenemos la tarea de la base de datos, para asegurar que estamos actualizando la correcta
            var taskHistory = await _unitOfWork.TasksHistory.GetByIdAsync(oTaskHistory.Id);
            if (taskHistory == null)
                return NotFound();

            // mensaje del cambio realizado.
            message = $"TaskHistory Updated | TaskHistory ID: {taskHistory.Id} TaskHistory TaskId (old): {taskHistory.TaskId} TaskHistory StateId (old): {taskHistory.StateId} TaskHistory Changed Date (old): {taskHistory.ChangedDate}\n" +
                $"TaskHistory TaskId (new): {oTaskHistory.TaskId} TaskHistory StateId (new): {oTaskHistory.StateId} TaskHistory Changed Date (new): {oTaskHistory.ChangedDate}";


            // Utilizamos AutoMapper para mapear las propiedades de oTask a task
            _mapper.Map(oTaskHistory, taskHistory);  // Este paso asigna automáticamente las propiedades

            // Llamamos al método Update del repositorio para guardar los cambios
            _unitOfWork.TasksHistory.Update(taskHistory);
            await _unitOfWork.SaveAsync();

            // Logeamos el cambio realizado.
            _loggingService.LogInformation(message);

            // Retornamos la tarea actualizada
            return Ok(taskHistory);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the TasksHistory. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    // Método existente: eliminar una tarea
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var message = string.Empty;
        try
        {
            var taskHistory = await _unitOfWork.TasksHistory.GetByIdAsync(id);
            if (taskHistory == null)
                return NotFound();

            _unitOfWork.TasksHistory.Remove(taskHistory);
            await _unitOfWork.SaveAsync();

            // mensaje del cambio realizado.
            message = $"TaskHistory Deleted | TaskHistory ID: {taskHistory.Id} TasksHistory TaskId: {taskHistory.TaskId} TasksHistory StateId: {taskHistory.StateId} TasksHistory ChangedDate: {taskHistory.ChangedDate}";
            // Logeamos el cambio realizado.
            _loggingService.LogInformation(message);

            return NoContent();
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the TasksHistory. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }
}