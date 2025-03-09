using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Repositories;
using RAWANiAspireOrchestrationApp.ApiService.Application.Abstractions.Services;
using RAWANiAspireOrchestrationApp.ApiService.Application.Services;
using RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Data.DataFactory;
using RAWANiAspireOrchestrationApp.ApiService.Infrastructure.Repository.UserProfileRepo;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
               .AddApiExplorer(options =>
               {
                   options.GroupNameFormat = "'v'VVV";
                   options.SubstituteApiVersionInUrl = true;
               });

// Add services to the container.
builder.Services.AddControllers(); // Enable controllers
builder.Services.AddScoped<IErrorHandler, ErrorHandler>(); // Add error handling service
builder.Services.AddScoped<IDatabaseConnectionFactory, SqlDatabaseConnectionFactory>(); // ADO.NET Factory
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>(); // User profile repository

// ? Register MediatR and scan the current assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


builder.Services.AddProblemDetails(); // Adds ProblemDetails middleware for error handling

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(app =>
{
    // Default exception handling using ProblemDetails
    app.Run(async context =>
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred",
            Detail = "Please contact support if the issue persists."
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // Enable controllers

app.MapDefaultEndpoints(); // Keep Aspire default endpoints

app.Run();
