using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoviesApi;
using MoviesApi.Models;
using MoviesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add Genre service
builder.Services.AddTransient<IGenresService, GenresService>();

//Add Movie service
builder.Services.AddTransient<IMoviesService, MoviesService>();

//Add Automapper service
builder.Services.AddAutoMapper(typeof(Program));

//Enable CORS
builder.Services.AddCors();

//Swagger Seciurty Tokens
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Enable CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();
