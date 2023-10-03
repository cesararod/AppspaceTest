using CRApiSolution.Interfaces;
using CRApiSolution.Services;
using CRApiSolution.Contexts;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Media Recommendation API", Version = "v1" });
});

string connectionString = builder.Configuration.GetConnectionString("CinemaDatabase");
builder.Services.AddHttpClient();
builder.Services.AddScoped<IMediaRecommendationService, MediaRecommendationApi>();
builder.Services.AddDbContext<MovieDbContext>(options =>
            options.UseSqlServer(connectionString));
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
