using MediatR;
using System.Reflection;
using TodoApp.API.Middleware;
using TodoApp.Core.Commands;
using TodoApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => {
cfg.RegisterServicesFromAssembly(typeof(CreateTodoCommand).Assembly);
});

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TodoApp API",
        Version = "v1",
        Description = "A simple RESTful API for managing todo items"
    });

    // Generate XML comments for Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();