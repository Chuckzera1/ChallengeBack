using Microsoft.EntityFrameworkCore;
using ChallengeBack.Infrastructure.Data;
using ChallengeBack.Api.Extensions;
using ChallengeBack.Application.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ChallengeBack API",
        Version = "v1",
        Description = "API para gerenciamento de empresas e fornecedores",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ChallengeBack Team"
        }
    });
});

// Add controllers
builder.Services.AddControllers();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.HandleDependencyInjection();
builder.Services.AddHttpClient();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddCors(opt => {
    opt.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Apply database migrations automatically
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChallengeBack API v1");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();

app.Run();

