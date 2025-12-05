using Microsoft.EntityFrameworkCore;
using Patricia.ChatBot.Dto;
using Patricia.ChatBot.Entity;
using Patricia.ChatBot.Repository;

namespace Patricia.ChatBot.Services;

public class UserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<UserResponseDto> Create(UserRequestDto dto)
    {
        var existingUser = await _repo.GetByEmail(dto.Email);
        if (existingUser != null)
            throw new Exception("Email já cadastrado.");

        var user = new UserEntity
        {
            Nome = dto.Nome,
            Email = dto.Email,
            IsAdmin = dto.IsAdmin,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
        };

        await _repo.Create(user);

        return ToResponse(user);
    }

    public async Task<List<UserResponseDto>> GetAll()
    {
        var users = await _repo.GetAll();
        return users.Select(ToResponse).ToList();
    }

    public async Task<UserResponseDto?> GetById(long id)
    {
        var user = await _repo.GetById(id);
        return user is null ? null : ToResponse(user);
    }

    public async Task<bool> Delete(long id)
    {
        return await _repo.Delete(id);
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto dto)
    {
        var user = await _repo.GetByEmail(dto.Email);

        if (user is null)
            throw new Exception("Credenciais inválidas.");

        var senhaOk = BCrypt.Net.BCrypt.Verify(dto.Senha, user.SenhaHash);

        if (!senhaOk)
            throw new Exception("Credenciais inválidas.");

        return new LoginResponseDto
        {
            Id = user.Id,
            Nome = user.Nome,
            Email = user.Email,
            IsAdmin = user.IsAdmin
        };
    }

    private static UserResponseDto ToResponse(UserEntity u)
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
