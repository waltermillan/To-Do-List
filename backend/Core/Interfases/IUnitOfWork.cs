using Core.Interfases;

namespace Core.Interfases;

public interface IUnitOfWork
{
    ITaskRepository Tasks { get; }
    ITaskHistoryRepository TasksHistory { get; }
    IStateRepository States { get; }

    void Dispose();
    Task<int> SaveAsync();
}
