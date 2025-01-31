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

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Habilita la UI de Swagger
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // 🔹 Aplicar CORS

app.UseAuthorization();

app.MapControllers();

app.Run();
