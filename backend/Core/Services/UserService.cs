using Core.Entities;
using Core.Interfaces;

namespace Core.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository stateRepository)
    {
        _userRepository = stateRepository;
    }

    public async Task<IEnumerable<User>> GeAllUsers()
    {
        return await _userRepository.GetAllAsync();
    }

    public void AddUser(string name, string password)
    {
        var user = Core.Factories.UserFactory.CreateUser(name, password);

        _userRepository.Add(user);
    }

    public void AddUsers(IEnumerable<User> user)
    {
        _userRepository.AddRange(user);
    }

    public void UpdateUser(User user)
    {
        var existingUser = _userRepository.GetByIdAsync(user.Id).Result;

        if (existingUser is null)
            throw new KeyNotFoundException("User to update not found");

        _userRepository.Update(user);
    }

    public void DeleteUser(User user)
    {
        var existingUser = _userRepository.GetByIdAsync(user.Id).Result;

        if (existingUser is null)
            throw new KeyNotFoundException("User to delete not found");

        _userRepository.Remove(user);
    }
}
