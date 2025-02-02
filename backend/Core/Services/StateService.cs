using Core.Entities;
using Core.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Services;

public class StateService
{
    private readonly IStateRepository _stateRepository;

    public StateService(IStateRepository stateRepository)
    {
        _stateRepository = stateRepository;
    }

    public async Task<State> GetStateById(int id)
    {
        var state = await _stateRepository.GetByIdAsync(id);
        if (state == null)
        {
            throw new KeyNotFoundException("State not found"); // Mensaje de excepción correcto
        }
        return state;
    }

    public async Task<IEnumerable<State>> GeAllStates()
    {
        return await _stateRepository.GetAllAsync();
    }

    public void AddState(string name)
    {
        // Usamos el TaskFactory para crear el estado
        var state = Core.Factories.StateFactory.CreateState(name);

        // Ahora agregamos la tarea usando el repositorio
        _stateRepository.Add(state);
    }

    public void AddStates(IEnumerable<State> state)
    {
        _stateRepository.AddRange(state);
    }

    public void UpdateState(State state)
    {
        var existingState = _stateRepository.GetByIdAsync(state.Id).Result;
        if (existingState == null)
        {
            throw new KeyNotFoundException("State to update not found");
        }
        _stateRepository.Update(state);
    }

    public void DeleteState(State state)
    {
        var existingState = _stateRepository.GetByIdAsync(state.Id).Result;
        if (existingState == null)
        {
            throw new KeyNotFoundException("State to delete not found");
        }
        _stateRepository.Remove(state);
    }
}
