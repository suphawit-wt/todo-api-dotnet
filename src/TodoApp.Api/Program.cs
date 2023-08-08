using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Constants;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add Database Context using SQL Server Connection
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(Const.DB_CONN));

// Dependency Injection of Middleware, Services and Repositories
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();