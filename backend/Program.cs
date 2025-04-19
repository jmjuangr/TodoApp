using Models;
using Microsoft.EntityFrameworkCore;
using TodoAppApi.Data;
var builder = WebApplication.CreateBuilder(args);

// 🔹 Agregar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));



builder.Services.AddControllers();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI(); 


// app.UseHttpsRedirection();

app.UseCors("AllowAll"); 

app.UseAuthorization();

app.MapControllers();

app.Run();
