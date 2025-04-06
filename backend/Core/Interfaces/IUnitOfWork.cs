namespace Core.Interfaces;

public interface IUnitOfWork
{
    ITaskRepository Tasks { get; }
    ITaskHistoryRepository TasksHistory { get; }
    IStateRepository States { get; }
    IUserRepository Users { get; }

    void Dispose();
    Task<int> SaveAsync();
}
