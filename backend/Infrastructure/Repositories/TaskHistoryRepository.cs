using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskHistoryRepository(TodoListContext context) : GenericRepository<TaskHistory>(context), ITaskHistoryRepository
{
    public override async Task<TaskHistory> GetByIdAsync(int id)
    {
        return await _context.TasksHistory
                          .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<TaskHistory>> GetAllAsync()
    {
        return await _context.TasksHistory.ToListAsync();
    }
}