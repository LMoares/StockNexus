using Microsoft.OpenApi;
using StockNexusAPI.Infrastructure;
using StockNexusAPI.Infrastructure.Services.Authentication;
using StockNexusAPI.API.Extensions;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

//configure logging
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);

    var path = Path.Combine(context.HostingEnvironment.ContentRootPath, "Logs", "stocknexus-log-.txt");

    configuration.WriteTo.File(
        path: path,
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
});

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
//JWT Config
builder.Services.AddJWTAuthentication(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddCustomRateLimiter();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOpenApi();

//Swagger config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT Token"
    });

    x.AddSecurityRequirement((document) =>
    {
        var schemeRef = new OpenApiSecuritySchemeReference("Bearer", document, null);
        var requirement = new OpenApiSecurityRequirement();
        requirement.Add(schemeRef, new List<string>());
        return requirement;
    });
});

builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "StockNexusAPI is running");

app.Run();
