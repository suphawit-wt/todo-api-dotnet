using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Constants;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Core.Interfaces.Services;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager _configs = builder.Configuration;

string[] CORS_ORIGINS = _configs.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new string[] { "" };

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(CORS_ORIGINS)
               .WithMethods("GET", "POST", "PUT", "DELETE")
               .WithHeaders("Authorization", "Content-Type")
               .AllowCredentials();
    });
});

// Add Database Context using SQL Server Connection
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(Const.DB_CONN));

// Dependency Injection of Middleware, Services and Repositories
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();