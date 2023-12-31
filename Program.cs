using System.Text;
using DataTrack.Auth;
using DataTrack.DataBase;
using DataTrack.Repositories.Implementation;
using DataTrack.Repositories.Interface;
using DataTrack.Secrets;
using DataTrack.Services.Implementation;
using DataTrack.Services.Interface;
using DataTrack.WebSocketConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

# region Auth

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "dataTrack-server",
        ValidAudience = "dataTrack-client",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((Secrets.Key))),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityData.AdminUserPolicyName, p => 
        p.RequireClaim(IdentityData.AdminUserClaimName, "True"));
});

# endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

# region Database

//builder.Services.AddSingleton<DatabaseContext>();
// Register the DatabaseContext with the dependency injection container
// builder.Services.AddDbContext<DatabaseContext>(options =>
// {
//     options.UseMySQL("server=localhost;port=3306;user=root;password=root123;database=datatrackdb");
// });
builder.Services.AddSingleton<DatabaseContext>(sp =>
{
    var options = new DbContextOptionsBuilder<DatabaseContext>()
        .UseMySQL("server=localhost;port=3306;user=root;password=root123;database=datatrackdb")
        .Options;

    return new DatabaseContext(options);
});

builder.Services.AddSingleton<DataBaseContextSeed>();
# endregion

# region Repositories

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
builder.Services.AddSingleton<IAnalogInputRepository, AnalogInputRepository>();
builder.Services.AddSingleton<IDigitalInputRepository, DigitalInputRepository>();
builder.Services.AddSingleton<IAnalogInputRecordRepository, AnalogInputRecordRepository>();
builder.Services.AddSingleton<IDigitalInputRecordRepository, DigitalInputRecordRepository>();
builder.Services.AddSingleton<IAlarmRepository, AlarmRepository>();
builder.Services.AddSingleton<IAlarmRecordRepository, AlarmRecordRepository>();
builder.Services.AddSingleton<IAnalogOutputRepository, AnalogOutputRepository>();
builder.Services.AddSingleton<IDigitalOutputRepository, DigitalOutputRepository>();

# endregion

# region Services

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IDeviceService, DeviceService>();
builder.Services.AddSingleton<IAnalogInputService, AnalogInputService>();
builder.Services.AddSingleton<IDigitalInputService, DigitalInputService>();
builder.Services.AddSingleton<IAnalogInputRecordService, AnalogInputRecordService>();
builder.Services.AddSingleton<IDigitalInputRecordService, DigitalInputRecordService>();
builder.Services.AddSingleton<IAlarmService, AlarmService>();
builder.Services.AddSingleton<IAnalogOutputService, AnalogOutputService>();
builder.Services.AddSingleton<IDigitalOutputService, DigitalOutputService>();

builder.Services.AddHostedService<SimulationService>();

# endregion

# region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp",
        x =>
        {
            x.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

# endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("AngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

# region Sockets

app.MapHub<InputHub>("/socket/input");
app.MapHub<AlarmHub>("/socket/alarm");

# endregion

var dbContextSeed = app.Services.GetRequiredService<DataBaseContextSeed>();
dbContextSeed.Seed();

app.Run();