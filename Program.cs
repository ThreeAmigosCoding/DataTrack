using DataTrack.DataBase;
using DataTrack.Repositories.Implementation;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Implementation;
using DataTrack.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddSingleton<DatabaseContext>();
// Register the DatabaseContext with the dependency injection container
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySQL("server=localhost;port=3306;user=root;password=root123;database=datatrackdb");
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();