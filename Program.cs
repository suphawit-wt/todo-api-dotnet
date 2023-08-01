using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using todo_api_sqlserver.Data;
using todo_api_sqlserver.Models.Responses;
using todo_api_sqlserver.Services.Auth;
using todo_api_sqlserver.Services.Todos;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
             {
                Reference = new OpenApiReference
                 {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                 }
             },
             new List<string>()
         }
    });
});
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("connSQLServer")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration.GetSection("JWT:Issuer").Value,
            ValidAudience = configuration.GetSection("JWT:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Value!)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

app.UseStatusCodePages(async statusCodeContext =>
{
    switch (statusCodeContext.HttpContext.Response.StatusCode)
    {
        case 401:
            await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(new MessageResponse { Message = "Unauthorized" });
            break;
        case 500:
            await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(new MessageResponse { Message = "Internal Server Error" });
            break;
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
