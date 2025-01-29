using Core.Entities;
using Core.Interfases;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskRepository(Context context) : GenericRepository<Core.Entities.Task>(context), ITaskRepository
{

    // Método existente
    public override async Task<Core.Entities.Task> GetByIdAsync(int id)
    {
        return await _context.Tasks
                          .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Método existente
    public override async Task<IEnumerable<Core.Entities.Task>> GetAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    // Método existente para paginación y búsqueda
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