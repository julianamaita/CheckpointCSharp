using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService
{
    private readonly IUnitOfWork _uow;
    public UserService(IUnitOfWork uow) => _uow = uow;

    public async Task<User> CreateAsync(string name, string email)
    {
        var exists = _uow.Users.Query().Any(u => u.Email == email);
        if (exists) throw new InvalidOperationException("E-mail já cadastrado.");

        var user = new User { Name = name, Email = email };
        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();
        return user;
    }

    public IQueryable<User> Query() => _uow.Users.Query();

    public User? GetDetailed(int id) =>
        _uow.Users.Query()
            .Include(u => u.Assessments)
            .Include(u => u.JournalEntries)
            .Include(u => u.Alerts)
            .Include(u => u.Goals)
            .FirstOrDefault(u => u.Id == id);

    public async Task UpdateAsync(int id, string? name, string? email)
    {
        var user = _uow.Users.Query().FirstOrDefault(u => u.Id == id);
        if (user is null) throw new InvalidOperationException("Usuário não encontrado.");

        if (!string.IsNullOrWhiteSpace(name)) user.Name = name!.Trim();
        if (!string.IsNullOrWhiteSpace(email))
        {
            email = email!.Trim();
            var exists = _uow.Users.Query().Any(u => u.Email == email && u.Id != id);
            if (exists) throw new InvalidOperationException("E-mail já em uso por outro usuário.");
            user.Email = email;
        }

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = _uow.Users.Query().FirstOrDefault(u => u.Id == id);
        if (user is null) throw new InvalidOperationException("Usuário não encontrado.");
        _uow.Users.Remove(user);
        await _uow.SaveChangesAsync();
    }
}
