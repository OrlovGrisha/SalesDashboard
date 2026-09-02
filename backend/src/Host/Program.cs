using Host.ExceptionHandling;
using SalesDashboard.Application;
using SalesDashboard.Infrastructure;
using Scalar.AspNetCore;
class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        
        const string CorsPolicyName = "AllowFrontend";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                policy.WithOrigins(
                        builder.Configuration["Frontend:Origin"] ?? "http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        
        builder.Services.AddInfrastructure(connectionString);
        builder.Services.AddApplication();

        
        var app = builder.Build();

        app.UseExceptionHandler();
        
        app.UseCors(CorsPolicyName);
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // отдаёт JSON-документ по /swagger/v1/swagger.json
            app.MapScalarApiReference(options =>
                options.WithOpenApiRoutePattern("/swagger/v1/swagger.json"));
        }
        
        app.MapControllers();
        
        await app.RunAsync();
    }
}