using Core.Entities;
using Core.Interfaces;

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

        if (state is null)
            throw new KeyNotFoundException("State not found");

        return state;
    }

    public async Task<IEnumerable<State>> GeAllStates()
    {
        return await _stateRepository.GetAllAsync();
    }

    public void AddState(string name)
    {
        var state = Core.Factories.StateFactory.CreateState(name);

        _stateRepository.Add(state);
    }

    public void AddStates(IEnumerable<State> state)
    {
        _stateRepository.AddRange(state);
    }

    public void UpdateState(State state)
    {
        var existingState = _stateRepository.GetByIdAsync(state.Id).Result;

        if (existingState is null)
            throw new KeyNotFoundException("State to update not found");

        _stateRepository.Update(state);
    }

    public void DeleteState(State state)
    {
        var existingState = _stateRepository.GetByIdAsync(state.Id).Result;

        if (existingState is null)
            throw new KeyNotFoundException("State to delete not found");

        _stateRepository.Remove(state);
    }
}
