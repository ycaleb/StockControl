using Microsoft.EntityFrameworkCore;
using StockControl.Data;
using StockControl.Services;
using StockControl.Application.Interfaces;
using StockControl.Application.Services;
using StockControl.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("StockDB"));


builder.Services.AddScoped<RelatorioService,RelatorioService>();
builder.Services.AddScoped<EstoqueService>();
builder.Services.AddScoped<MaterialService>();

builder.Services.AddScoped<IMaterialRepository,MaterialRepository>();
builder.Services.AddScoped<IMovimentoEstoqueRepository,MovimentoEstoqueRepository>();
builder.Services.AddScoped<IMaterialRepository,MaterialRepository>();
builder.Services.AddScoped<MaterialService>();


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
