using Core.Entities;
using Core.Interfases;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskHistoryRepository(TodoListContext context) : GenericRepository<TaskHistory>(context), ITaskHistoryRepository
{

    // Método existente
    public override async Task<TaskHistory> GetByIdAsync(int id)
    {
        return await _context.TasksHistory
                          .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Método existente
    public override async Task<IEnumerable<TaskHistory>> GetAllAsync()
    {
        return await _context.TasksHistory.ToListAsync();
    }
}