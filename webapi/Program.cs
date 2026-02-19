using System.Text;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Kimbito.Infra.Repositories;
using Kimbito.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<KimbitoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("KimbitoDbConnection")));

// =======================
// 🔹 INJEÇÃO DE DEPENDÊNCIA
// =======================

builder.Services.AddScoped<IAutenticacao, AutenticacaoRepository>();
builder.Services.AddScoped<IUtilizador, UtilizadorRepository>();
builder.Services.AddScoped<AutenticacaoService>();
builder.Services.AddScoped<UtilizadorService>();
builder.Services.AddScoped<ICategoria, CategoriaRepository>();
builder.Services.AddScoped<IProduto, ProdutoRepository>();
builder.Services.AddScoped<IFormaPagamento, FormaPagamentoRepository>();
builder.Services.AddScoped<IMorada, MoradaRepository>();
builder.Services.AddScoped<IEncomenda, EncomendaRepository>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<FormaPagamentoService>();
builder.Services.AddScoped<MoradaService>();
builder.Services.AddScoped<EncomendaService>();

// =======================
// 🔐 JWT CONFIG
// =======================

var jwtSecret = builder.Configuration["Jwt:Secret"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {

            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("Falha JWT: " + ctx.Exception.Message);
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
                {
                    context.Token = token.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    context.Token = token;
                }

                Console.WriteLine("Token limpo para validação: " + context.Token);
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthorization();

// =======================
// 📘 SWAGGER CONFIG
// =======================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Kimbito API",
        Version = "v1"
    });

    // 🔐 JWT no Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Authorization header usando Bearer. Exemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
