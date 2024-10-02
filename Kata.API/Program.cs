using Kata.Application.Services;
using Kata.Domain.Interfaces;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data;
using Kata.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<KataDBContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    options.UseInMemoryDatabase("TestDatabase")
);

builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", policy => {
    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
}));
builder.Services.AddTransient<IClanRepository, ClanRepository>();
builder.Services.AddTransient<IBattleRepository, BattleRepository>();
builder.Services.AddScoped<IClanService, ClanService>();
builder.Services.AddScoped<IBattleService, BattleService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
