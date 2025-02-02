using AutoMapper;
using Core.Entities;
using Core.Factories;
using Core.Interfases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers;

public class StateController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _loggingService;

    public StateController(IUnitOfWork unitOfWork, IMapper mapper, ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggingService = loggingService;
    }

    // Método existente: obtener todas los estados
    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<State>>> Get()
    {
        var message = string.Empty;
        try
        {
            var states = await _unitOfWork.States.GetAllAsync();

            foreach (var state in states)
            {
                // mensaje de la busqueda realizada
                message = $"States Listed | State ID: {state.Id} State Name: {state.Name}\n";

                // Logueamos la busqueda realizada
                _loggingService.LogInformation(message);
            }

            return _mapper.Map<List<State>>(states);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the States. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    // Método existente: obtener un estado por su ID
    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<State>> Get(int id)
    {
        var message = string.Empty;
        try
        {
            var state = await _unitOfWork.States.GetByIdAsync(id);
            if (state == null)
                return NotFound();

            // mensaje de la busqueda realizada
            message = $"State Listed | State ID: {state.Id} State Name: {state.Name}";

            // Logueamos la busqueda realizada
            _loggingService.LogInformation(message);

            return _mapper.Map<State>(state);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the State. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    // Método existente: agregar un estado
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<State>> Post(State oState)
    {
        var message = string.Empty;
        try
        {
            // Utilizamos el patron Factory (State) para crear la tarea
            var state = StateFactory.CreateState(oState.Name);

            // Añadir el estado creada usando el repositorio
            _unitOfWork.States.Add(state);
            await _unitOfWork.SaveAsync();

            // Verificamos si el estado fue creada correctamente
            if (state == null)
            {
                return BadRequest();
            }

            // Asignamos el Id de el estado para retornarlo correctamente
            oState.Id = state.Id;

            // mensaje del cambio realizado.
            message = $"State Created | State ID: {state.Id} State Name: {state.Name}";

            // Logeamos el cambio realizado.
            _loggingService.LogInformation(message);

            // Retornamos el estado recién creado con un código de respuesta 201
            return CreatedAtAction(nameof(Post), new { id = oState.Id }, oState);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the States. Please try again later.";
            _loggingService.LogError(message,ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    // Método existente: actualizar un estado
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<State>> Put([FromBody] State oState)
    {
        var message = string.Empty;
        try
        {
            if (oState == null)
                return NotFound();

            // Primero obtenemos la tarea de la base de datos, para asegurar que estamos actualizando la correcta
            var state = await _unitOfWork.States.GetByIdAsync(oState.Id);
            if (state == null)
                return NotFound();

            // mensaje del cambio realizado.
            message = $"State Updated | State ID: {state.Id} State Name (old): {state.Name} State Name (new): {oState.Name} ";


            // Utilizamos AutoMapper para mapear las propiedades de oState a state
            _mapper.Map(oState, state);  // Este paso asigna automáticamente las propiedades

            // Llamamos al método Update del repositorio para guardar los cambios
            _unitOfWork.States.Update(state);
            await _unitOfWork.SaveAsync();

            // Logeamos el cambio realizado.
            _loggingService.LogInformation(message);

            // Retornamos la tarea actualizada
            return Ok(state);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the States. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    // Método existente: eliminar un estado
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var message = string.Empty;
        try
        {
            var state = await _unitOfWork.States.GetByIdAsync(id);
            if (state == null)
                return NotFound();

            _unitOfWork.States.Remove(state);
            await _unitOfWork.SaveAsync();

            // mensaje del cambio realizado.
            message = $"State Deleted | State ID: {state.Id} State Name: {state.Name}";
            // Logeamos el cambio realizado.
            _loggingService.LogInformation(message);

            return NoContent();
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            message = "There was an issue retrieving the States. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }
}