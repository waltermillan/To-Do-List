using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StateRepository(TodoListContext context) : GenericRepository<State>(context), IStateRepository
{
    public override async Task<State> GetByIdAsync(int id)
    {
        return await _context.States
                          .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<State>> GetAllAsync()
    {
        return await _context.States.ToListAsync();
    }

    public override async Task<(int totalRegistros, IEnumerable<State> registros)> GetAllAsync(
                int pageIndex, int pageSize, string search)
    {
        var consulta = _context.States as IQueryable<State>;

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