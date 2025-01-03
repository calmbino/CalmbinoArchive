using CalmbinoArchive.Application.Extensions;
using CalmbinoArchive.Infrastructure.Extensions;
using CalmbinoArchive.ServiceDefaults;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://localhost:7036",
                      "http://localhost:5196")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


// Add aspire Services
builder.AddServiceDefaults();

// Add PostgreSQL
builder.AddPostgreDatabase();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplication()
       .AddInfrastructure(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure aspire
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();