using Microsoft.EntityFrameworkCore;

using server.Data;

var builder =
    WebApplication.CreateBuilder(
        args);

builder.Services
    .AddDbContext<AppDbContext>(
        options =>
            options.UseSqlite(
                "Data Source=BankTeller.db"));

builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy
                    .WithOrigins(
                        "https://localhost:7296")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });

builder.Services
    .AddControllers();



builder.Services
    .AddEndpointsApiExplorer();

builder.Services
    .AddSwaggerGen();

var app =
    builder.Build();

if (app.Environment
    .IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();



app.Run();