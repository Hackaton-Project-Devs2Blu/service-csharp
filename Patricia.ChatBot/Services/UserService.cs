using Microsoft.EntityFrameworkCore;
using Patricia.ChatBot.Dto;
using Patricia.ChatBot.Entity;

namespace Patricia.ChatBot.Services;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserResponseDto> Create(UserRequestDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Email já cadastrado.");

        var user = new UserEntity
        {
            Nome = dto.Nome,
            Email = dto.Email,
            IsAdmin = dto.IsAdmin,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return ToResponse(user);
    }

    public async Task<List<UserResponseDto>> GetAll()
    {
        var users = await _db.Users.ToListAsync();
        return users.Select(ToResponse).ToList();
    }

    public async Task<UserResponseDto?> GetById(long id)
    {
        var user = await _db.Users.FindAsync(id);
        return user is null ? null : ToResponse(user);
    }

    public async Task<bool> Delete(long id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    private UserResponseDto ToResponse(UserEntity u)
    {
        return new UserResponseDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            IsAdmin = u.IsAdmin,
            CriadoEm = u.CriadoEm
        };
    }
}