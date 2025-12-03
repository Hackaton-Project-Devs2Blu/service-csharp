using Microsoft.EntityFrameworkCore;
using Patricia.ChatBot.Entity;

namespace Patricia.ChatBot;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
}