
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Patricia.ChatBot.Repository;
using Patricia.ChatBot.Services;

namespace Patricia.ChatBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<GeminiService>();
            builder.Services.AddSingleton<DatasetService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var connectionString =
                "Host=hackathon-project-db-hackathon.cf21ln5uzado.us-west-2.rds.amazonaws.com;" +
                "Port=5432;" +
                "Database=db_ecs_fargate;" +
                "Username=userdatabase;" +
                "Password=VeRyH4rdPa55w.rd**;" +
                "SSL Mode=Require;Trust Server Certificate=true;";

            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(connectionString)
            );

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/api/csharp" }
                        };
                    });
                });
                app.UseSwaggerUI();
            }

            app.UsePathBase("/api/csharp");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
