using dotenv.net;
using Go_Saku.Net.Data;
using Go_Saku.Net.Repositories;
using Go_Saku.Net.Usecase;
using go_saku_cs.Middleware;
using go_saku_cs.Repositories;
using go_saku_cs.Usecase;
using Go_Saku_CS.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserUsecase, UserUsecase>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<IBankUsecase, BankUsecase>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<IPhotoUsecase, PhotoUsecase>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserApiDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DBCONNECTION")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();



// Dapatkan nilai JWTKEY dari variabel env
string jwtKey = Environment.GetEnvironmentVariable("JWTKEY");
byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtKey);
byte[] validKeyBytes = new byte[32]; // Panjang kunci minimal yang dibutuhkan adalah 128 bit (16 byte)
Array.Copy(keyBytes, validKeyBytes, Math.Min(keyBytes.Length, validKeyBytes.Length));

SymmetricSecurityKey securityKey = new SymmetricSecurityKey(validKeyBytes);

app.UseWhen(context =>
   context.Request.Path.StartsWithSegments("/api/users/auth"),
    appBuilder =>
    {
        appBuilder.UseMiddleware<AuthMiddlewareUsername>(securityKey);
    });
app.UseWhen(context =>
   context.Request.Path.StartsWithSegments("/api/bank") ||
    context.Request.Path.StartsWithSegments("/api/photo"),
    appBuilder =>
    {
        appBuilder.UseMiddleware<AuthMiddlewareUserId>(securityKey);
    });

app.MapControllers();




app.Run();
