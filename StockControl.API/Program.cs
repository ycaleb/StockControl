using Microsoft.EntityFrameworkCore;
using ProjetoConstrucao.API.Data;
using ProjetoConstrucao.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("ConstrucaoDB"));


builder.Services.AddScoped<RelatorioService,RelatorioService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
