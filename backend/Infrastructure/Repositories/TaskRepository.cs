using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskRepository(TodoListContext context) : GenericRepository<Core.Entities.Task>(context), ITaskRepository
{
    public override async Task<Core.Entities.Task> GetByIdAsync(int id)
    {
        return await _context.Tasks
                          .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Core.Entities.Task>> GetAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public override async Task<(int totalRegistros, IEnumerable<Core.Entities.Task> registros)> GetAllAsync(
                int pageIndex, int pageSize, string search)
    {
        var consulta = _context.Tasks as IQueryable<Core.Entities.Task>;

        if (!string.IsNullOrEmpty(search))
        {
            consulta = consulta.Where(p => p.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }

        var totalRegistros = await consulta.CountAsync();
        var registros = await consulta
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return (totalRegistros, registros);
    }
}