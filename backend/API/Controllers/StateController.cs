using AutoMapper;
using Core.Entities;
using Core.Interfases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class StateController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StateController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Método existente: obtener todas los estados
    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<State>>> Get()
    {
        try
        {
            var states = await _unitOfWork.States.GetAllAsync();
            return _mapper.Map<List<State>>(states);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the States. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: obtener un estado por su ID
    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<State>> Get(int id)
    {
        try
        {
            var state = await _unitOfWork.States.GetByIdAsync(id);
            if (state == null)
                return NotFound();

            return _mapper.Map<State>(state);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the State. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: agregar un estado
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<State>> Post(State oState)
    {
        try
        {
            var state = _mapper.Map<State>(oState);
            _unitOfWork.States.Add(state);
            await _unitOfWork.SaveAsync();
            if (state == null)
            {
                return BadRequest();
            }
            oState.Id = state.Id;
            return CreatedAtAction(nameof(Post), new { id = oState.Id }, oState);
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the States. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: actualizar un estado
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<State>> Put([FromBody] State oState)
    {
        try
        {
            if (oState == null)
                return NotFound();

            var state = _mapper.Map<State>(oState);
            _unitOfWork.States.Update(state);
            await _unitOfWork.SaveAsync();
            return oState;
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the States. Please try again later.", Details = ex.Message });
        }
    }

    // Método existente: eliminar un estado
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var state = await _unitOfWork.States.GetByIdAsync(id);
            if (state == null)
                return NotFound();

            _unitOfWork.States.Remove(state);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            // Loggeamos el error para fines de depuración y luego devolvemos una respuesta con un mensaje amigable
            return StatusCode(500, new { Message = "There was an issue retrieving the States. Please try again later.", Details = ex.Message });
        }
    }
}