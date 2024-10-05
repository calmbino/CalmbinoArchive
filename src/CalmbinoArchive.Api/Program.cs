using CalmbinoArchive.Application.Extentions;
using CalmbinoArchive.Infrastructure.Extensions;
using CalmbinoArchive.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://localhost:7269",
                "http://localhost:5036").AllowAnyHeader().AllowAnyMethod();
        });
});

// Add infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);
// Add persistence layer
builder.Services.AddPersistence(builder.Configuration);
// Add application layer
builder.Services.AddApplication();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Console.WriteLine($"Current Environment >>> {app.Environment.EnvironmentName}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();