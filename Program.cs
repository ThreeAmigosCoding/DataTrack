using System.Text;
using DataTrack.Auth;
using DataTrack.DataBase;
using DataTrack.Repositories.Implementation;
using DataTrack.Repositories.Interface;
using DataTrack.Secrets;
using DataTrack.Services.Implementation;
using DataTrack.Services.Interface;
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

# region Database

// builder.Services.AddSingleton<DatabaseContext>();
// Register the DatabaseContext with the dependency injection container
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySQL("server=localhost;port=3306;user=root;password=root123;database=datatrackdb");
});

# endregion

# region Repositories

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

# endregion

# region Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();

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

app.Run();