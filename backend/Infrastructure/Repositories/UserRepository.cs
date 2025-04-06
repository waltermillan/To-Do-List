using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(TodoListContext context) : GenericRepository<User>(context), IUserRepository
{
    public virtual async Task<User> GetByUsrAsync(string name)
    {
        return await _context.Users
                    .FirstOrDefaultAsync(u => u.Name == name);
    }

    // Método existente
    public override async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users
                          .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Método existente
    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public override async Task<(int totalRegistros, IEnumerable<User> registros)> GetAllAsync(
                int pageIndex, int pageSize, string search)
    {
        var consulta = _context.Users as IQueryable<User>;

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