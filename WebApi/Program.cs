using JsonPlaceholderApiClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Polly.Extensions.Http;
using JsonPlaceholderWebApi.Extensions;
using System;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Configura il contesto del database per utilizzare SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configura il client HTTP con Polly
builder.Services.AddHttpClient<ApiClient>()
    .AddRetryPolicy(); // Usa il metodo di estensione per configurare la politica di retry con Polly

// Aggiungi servizi per i controller
builder.Services.AddControllers();

// Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware per lo sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware per gestire HTTPS
app.UseHttpsRedirection();

// Middleware per autorizzazione
app.UseAuthorization();

// Configura il routing per i controller
app.MapControllers();

// Avvia l'applicazione
app.Run();
