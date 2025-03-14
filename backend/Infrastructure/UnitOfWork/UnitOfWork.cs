using Core.Interfases;
using Infrastructure.Data;
using Infrastructure.Repositories;
namespace Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly TodoListContext _context;
    private ITaskRepository _tasks;
    private IStateRepository _states;
    private ITaskHistoryRepository _taskshistory;

    public UnitOfWork(TodoListContext context)
    {
        _context = context;
    }

    public ITaskRepository Tasks
    {
        get
        {
            if (_tasks is null)
                _tasks = new TaskRepository(_context);

            return _tasks;
        }
    }

    public IStateRepository States
    {
        get
        {
            if (_states is null)
                _states = new StateRepository(_context);

            return _states;
        }
    }

    public ITaskHistoryRepository TasksHistory
    {
        get
        {
            if (_taskshistory is null)
                _taskshistory = new TaskHistoryRepository(_context);

            return _taskshistory;
        }
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
