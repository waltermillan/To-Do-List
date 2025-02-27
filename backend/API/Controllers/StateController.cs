using AutoMapper;
using Core.Entities;
using Core.Factories;
using Core.Interfases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers;
[ApiController]
[Route("api/states")]
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

    [HttpGet]
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
                message = $"States Listed | State ID: {state.Id} State Name: {state.Name}\n";
                _loggingService.LogInformation(message);
            }

            return _mapper.Map<List<State>>(states);
        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the States. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<State>> Get(int id)
    {
        var message = string.Empty;
        try
        {
            var state = await _unitOfWork.States.GetByIdAsync(id);

            if (state is null)
                return NotFound();

            message = $"State Listed | State ID: {state.Id} State Name: {state.Name}";

            _loggingService.LogInformation(message);

            return _mapper.Map<State>(state);
        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the State. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<State>> Post(State oState)
    {
        var message = string.Empty;
        try
        {
            // Utilizamos el patron Factory (State) para crear la tarea
            var state = StateFactory.CreateState(oState.Name);

            _unitOfWork.States.Add(state);
            await _unitOfWork.SaveAsync();

            if (state is null)
                return BadRequest();

            oState.Id = state.Id;

            message = $"State Created | State ID: {state.Id} State Name: {state.Name}";

            _loggingService.LogInformation(message);

            return CreatedAtAction(nameof(Post), new { id = oState.Id }, oState);
        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the States. Please try again later.";
            _loggingService.LogError(message,ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    [HttpPut("{id}")]
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

            var state = await _unitOfWork.States.GetByIdAsync(oState.Id);

            if (state is null)
                return NotFound();

            message = $"State Updated | State ID: {state.Id} State Name (old): {state.Name} State Name (new): {oState.Name} ";

            _mapper.Map(oState, state);  // Este paso asigna automáticamente las propiedades

            _unitOfWork.States.Update(state);
            await _unitOfWork.SaveAsync();

            _loggingService.LogInformation(message);

            return Ok(state);
        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the States. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
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
            var state = await _unitOfWork.States.GetByIdAsync(id);

            if (state is null)
                return NotFound();

            _unitOfWork.States.Remove(state);
            await _unitOfWork.SaveAsync();

            message = $"State Deleted | State ID: {state.Id} State Name: {state.Name}";
            _loggingService.LogInformation(message);

            return NoContent();
        }
        catch (Exception ex)
        {
            message = "There was an issue retrieving the States. Please try again later.";
            _loggingService.LogError(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }
}