using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Core;
using WebApi;
using Infrastructure.Initializers;
using Twilio;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("EstroDataConnectionPostgres") ?? throw new InvalidOperationException("Connection string 'EstroDataConnectionPostgres' not found.");

var accountSid = builder.Configuration["Twilio:AccountSID"];
var authToken = builder.Configuration["Twilio:AuthToken"];
TwilioClient.Init(accountSid, authToken);

builder.Services.AddDBContext(connection);

builder.Services.AddControllersWithCustomSchema();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenWithCustomSchema();

builder.Services.AddInfrastuctureService();

builder.Services.AddRepository();

builder.Services.AddValidator();

builder.Services.AddAutoMapper();

builder.Services.AddCustomService();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthenticationWithOptions(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

//builder.Services.AddCors();

var app = builder.Build();

app.UseCors(options =>
    options.AllowAnyHeader()
           .AllowAnyOrigin()
           .AllowAnyMethod());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseStaticFiles();

app.UseCors("AllowLocalhost5173");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await UsersAndRolesInitializer.SeedUsersAndRoles(app);

app.Run();
