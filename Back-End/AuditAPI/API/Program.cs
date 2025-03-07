using AuditAPI.Application.Services;
using AuditAPI.Domain.Interfaces;
using AuditAPI.Infrastructure.Repositories;
using AuditAPI.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

//Variavel de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

// Add services to the container.
builder.Services.AddScoped<IProdutoRepository>(provider => 
    new ProdutoRepository(connectionString)
);

builder.Services.AddScoped<ProdutoService, ProdutoService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
