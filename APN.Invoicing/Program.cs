using APN.Invoicing.Application;
using APN.Invoicing.Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var cString = builder.Configuration.GetConnectionString("DbConnect") ?? throw new ArgumentException("DbConnect");
builder.Services
    .AddApplication()
    .AddInfrastructure(cString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
