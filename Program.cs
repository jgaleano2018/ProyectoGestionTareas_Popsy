using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoGestionTareas.Domain.Services.Interfaces;
using ProyectoGestionTareas.Domain.Services;
using ProyectoGestionTareas.Domain.Port.Repositories;
using ProyectoGestionTareas.Domain.Model;
using System.Configuration;
using LinqToDB;
using Microsoft.Extensions.Options;
using ProyectoGestionTareas.Infraestructure.Context;
using ProyectoGestionTareas.Infraestructure.Adapters.Repositories;

var builder = WebApplication.CreateBuilder(args);

var corsUrls = "http://localhost:4200"; // here change or add your urls

if (corsUrls == null)
{
    throw new Exception("CorsUrls not found in appsettings.json");

}
var theConnection = builder.Configuration.GetConnectionString("DefaultConnection");
/*builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(theConnection, option =>
    {
        option.EnableRetryOnFailure();
    });
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);*/

builder.Services.AddTransient<ITareaService, TareaService>();
builder.Services.AddTransient<ITareaRepository, TareaRepository>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IUsuarioTareaService, UsuarioTareaService>();
builder.Services.AddTransient<IUsuarioTareaRepository, UsuarioTareaRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsApi", builder =>
    {
        builder.WithOrigins(corsUrls)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("CorsApi"); // don't forget to use the cors policy

app.UseAuthorization();

app.MapControllers();

app.Run();
