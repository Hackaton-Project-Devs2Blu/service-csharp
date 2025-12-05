using Patricia.ChatBot.Entity;

namespace Patricia.ChatBot.Repository;

public interface IUserRepository
{
    Task<UserEntity?> GetByEmail(string email);
    Task<UserEntity> Create(UserEntity user);
    Task<List<UserEntity>> GetAll();
    Task<UserEntity?> GetById(long id);
    Task<bool> Delete(long id);
}
