using AccountProcessService.Application;
using AccountProcessService.Application.Models;
using AccountProcessService.Application.Validations.ModelValidators;
using AccountProcessService.Infrastructure;
using Common.Logging;
using Common.Logging.Middleware;
using FluentValidation.AspNetCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "V1 Docs");
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseErrorLoggingMiddleware();

app.MapControllers();

app.Run();
