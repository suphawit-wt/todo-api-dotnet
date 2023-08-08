using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Constants;
using TodoApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add Database Context using SQL Server Connection
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(Const.DB_CONN));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();