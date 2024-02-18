using VentaWorker.Worker.Middleware;
using VentaWorker.Infrastructure;
using VentaWorker.Worker.Workers;
using VentaWorker.Application;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfigServer(
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    })
    );


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Capa de aplicacion
builder.Services.AddApplication();

//Capa de infra
builder.Services.AddInfraestructure(builder.Configuration);
//Adiconando el background service
builder.Services.AddHostedService<ActualizarStocksWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//Adicionar middleware customizado para tratar las excepciones
app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();
