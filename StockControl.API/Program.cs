using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using StockControl.Data;
using StockControl.Services;
using StockControl.Application.Interfaces;
using StockControl.Infrastructure.Repositories;
using StockControl.Application.Services;
using Microsoft.AspNetCore.Identity;
using StockControl.Models;


var builder = WebApplication.CreateBuilder(args);

// ==========================
// Configuração DB
// ==========================
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("StockDB"));

// ==========================
// Services & Repositories
// ==========================
builder.Services.AddScoped<RelatorioService>();
builder.Services.AddScoped<EstoqueService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMovimentoEstoqueRepository, MovimentoEstoqueRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// ==========================
// Configuração JWT
// ==========================
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtConfig["Key"] ?? "CHAVE_PADRAO_TEMP");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"]
    };
});

builder.Services.AddAuthorization();

// ==========================
// Swagger com JWT
// ==========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "StockControl API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Use: Bearer {token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ==========================
// Seed admin
// ==========================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = new PasswordHasher<StockControl.Models.Usuario>();

    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new StockControl.Models.Usuario
        {
            Nome = "admin",
            Cpf = "416.368.290-23",
            SenhaHash = passwordHasher.HashPassword(null!, "123")
        });
        db.SaveChanges();
    }
}

// ==========================
// Seed Materiais
// ==========================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Materiais.Any())
    {
        db.Materiais.AddRange(
            new Material { Nome = "Parafuso M6", UnidadeMedida = "UN", CustoUnitario = 0.15m, QuantidadeEstoque = 100 },
            new Material { Nome = "Porca M6", UnidadeMedida = "UN", CustoUnitario = 0.10m, QuantidadeEstoque = 100 },
            new Material { Nome = "Chave Phillips 3/8", UnidadeMedida = "UN", CustoUnitario = 12.50m, QuantidadeEstoque = 10 },
            new Material { Nome = "Martelo", UnidadeMedida = "UN", CustoUnitario = 25.00m, QuantidadeEstoque = 5 },
            new Material { Nome = "Fita Isolante", UnidadeMedida = "UN", CustoUnitario = 3.50m, QuantidadeEstoque = 20 },
            new Material { Nome = "Lixa 120", UnidadeMedida = "UN", CustoUnitario = 1.20m, QuantidadeEstoque = 50 },
            new Material { Nome = "Tinta Acrílica 1L", UnidadeMedida = "LT", CustoUnitario = 18.90m, QuantidadeEstoque = 15 },
            new Material { Nome = "Broca 10mm", UnidadeMedida = "UN", CustoUnitario = 2.50m, QuantidadeEstoque = 30 },
            new Material { Nome = "Alicate Universal", UnidadeMedida = "UN", CustoUnitario = 15.00m, QuantidadeEstoque = 8 },
            new Material { Nome = "Chave Allen Kit", UnidadeMedida = "UN", CustoUnitario = 20.00m, QuantidadeEstoque = 6 }
        );
        db.SaveChanges();
    }
}

// ==========================
// Seed Movimentos de Estoque
// ==========================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.MovimentosEstoque.Any())
    {
        var materiais = db.Materiais.ToList();

        var movimentos = new List<MovimentoEstoque>();
        var random = new Random();

        for (int i = 0; i < materiais.Count; i++)
        {
            var mat = materiais[i];

            var dataEntrada = new DateTime(2025, 10, 1 + i, 9, 0, 0);

            movimentos.Add(new MovimentoEstoque
            {
                MaterialId = mat.Id,
                Tipo = "entrada",
                Quantidade = random.Next(5, 100),
                ValorTotal = mat.CustoUnitario * random.Next(5, 100),
                Data = dataEntrada,
                Observacao = "Estoque inicial"
            });

            var dataSaida = dataEntrada.AddDays(2);

            movimentos.Add(new MovimentoEstoque
            {
                MaterialId = mat.Id,
                Tipo = "saida",
                Quantidade = random.Next(1, 20),
                ValorTotal = mat.CustoUnitario * random.Next(1, 20),
                Data = dataSaida,
                Observacao = "Venda teste"
            });

            var entradas = movimentos.Where(m => m.MaterialId == mat.Id && m.Tipo == "entrada").Sum(m => m.Quantidade);
            var saidas = movimentos.Where(m => m.MaterialId == mat.Id && m.Tipo == "saida").Sum(m => m.Quantidade);
            mat.QuantidadeEstoque = entradas - saidas;
        }

        db.MovimentosEstoque.AddRange(movimentos);
        db.SaveChanges();
    }
}

// ==========================
// Pipeline
// ==========================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockControl API V1");
});

app.MapControllers();

app.Run();
