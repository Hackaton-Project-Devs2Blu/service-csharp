using Microsoft.EntityFrameworkCore;
using Patricia.ChatBot.Entity;
namespace Patricia.ChatBot.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserEntity?> GetByEmail(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserEntity> Create(UserEntity user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<List<UserEntity>> GetAll()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<UserEntity?> GetById(long id)
    {
        return await _db.Users.FindAsync(id);
    }

    public async Task<bool> Delete(long id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
}