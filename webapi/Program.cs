using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Kimbito.Infra.Repositories;
using Kimbito.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<KimbitoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("KimbitoDbConnection")));
builder.Services.AddScoped<IAutenticacao, AutenticacaoRepository>();
builder.Services.AddScoped<IUtilizador, UtilizadorRepository>();
builder.Services.AddScoped<AutenticacaoService>();
builder.Services.AddScoped<UtilizadorService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
