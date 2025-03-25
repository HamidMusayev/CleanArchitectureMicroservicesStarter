using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Gateway",
        Version = "v1"
    });
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
    
    // Add Swagger endpoints for microservices
    c.SwaggerEndpoint("http://localhost:5001/swagger/v1/swagger.json", "OrderService v1");
    c.SwaggerEndpoint("http://localhost:5002/swagger/v1/swagger.json", "PaymentService v1");
    c.SwaggerEndpoint("http://localhost:5003/swagger/v1/swagger.json", "InventoryService v1");
    
    c.RoutePrefix = string.Empty;  // Sets Swagger UI at the root of the gateway
});

app.UseRouting();
app.MapControllers();

app.MapReverseProxy();  // Reverse Proxy for services

app.Run();