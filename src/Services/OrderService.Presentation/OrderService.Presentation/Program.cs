// OrderService/Program.cs
using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Database;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection; // Add this using directive
using Microsoft.Extensions.Configuration; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(connectionString)); // Ensure Microsoft.EntityFrameworkCore.SqlServer package is installed

// Configure MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq"); // Use the service name from Docker Compose
    });
});
builder.Services.AddMassTransitHostedService();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    dbContext.Database.Migrate();
}

app.Run();