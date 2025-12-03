
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase("PatriciaChatBotDB"));

            builder.Services.AddScoped<UserService>();

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
