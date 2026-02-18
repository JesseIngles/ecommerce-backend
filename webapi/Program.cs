using System.Text;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Kimbito.Infra.Repositories;
using Kimbito.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<KimbitoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("KimbitoDbConnection")));

// Autenticação e utilizadores
builder.Services.AddScoped<IAutenticacao, AutenticacaoRepository>();
builder.Services.AddScoped<IUtilizador, UtilizadorRepository>();
builder.Services.AddScoped<AutenticacaoService>();
builder.Services.AddScoped<UtilizadorService>();

// E-commerce
builder.Services.AddScoped<Kimbito.Domain.Interfaces.ICategoria, CategoriaRepository>();
builder.Services.AddScoped<Kimbito.Domain.Interfaces.IProduto, ProdutoRepository>();
builder.Services.AddScoped<Kimbito.Domain.Interfaces.IFormaPagamento, FormaPagamentoRepository>();
builder.Services.AddScoped<Kimbito.Domain.Interfaces.IMorada, MoradaRepository>();
builder.Services.AddScoped<Kimbito.Domain.Interfaces.IEncomenda, EncomendaRepository>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<FormaPagamentoService>();
builder.Services.AddScoped<MoradaService>();
builder.Services.AddScoped<EncomendaService>();

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "your_jwt_secret_here_min_32_chars_long_for_HS256";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "KimbitoEcommerce";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "KimbitoEcommerce";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
