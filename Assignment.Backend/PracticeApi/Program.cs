using System.Security.Cryptography;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;

using PracticeApi.Features.Invoices;
using PracticeApi.Features.Invoices.GetInvoiceById;
using PracticeApi.Features.Invoices.PayInvoice;
using PracticeApi.Features.Patients;
using PracticeApi.Features.Patients.AddPatient;
using PracticeApi.Features.Patients.GetPatientById;
using PracticeApi.Features.Patients.SearchPatient;
using PracticeApi.Features.Providers;
using PracticeApi.Features.Providers.AddProvider;
using PracticeApi.Features.Providers.GetProviderById;
using PracticeApi.Features.Providers.GetProviderByNpiId;
using PracticeApi.Features.Providers.GetProviders;
using PracticeApi.Features.Visits;
using PracticeApi.Features.Visits.AddVisit;
using PracticeApi.Features.Visits.GetAvailableSlots;
using PracticeApi.Features.Visits.GetVisitById;
using PracticeApi.Features.Visits.GetVisitsByDate;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Security;
using PracticeApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .WithMethods("GET", "POST", "PUT")
              .AllowAnyHeader();
    });
});

// Configure JSON options to serialize enums as strings
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Generate Key and IV for symmetric AES encryption at app startup
using (var aes = Aes.Create())
{
    var key = aes.Key;
    var iv = aes.IV;

    // Register EncryptionService as a singleton
    builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(key, iv));
}

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IGetInvoiceByIdHandler, GetInvoiceByIdHandler>();
builder.Services.AddScoped<IPayInvoiceHandler, PayInvoiceHandler>();
builder.Services.AddScoped<IAddProviderHandler, AddProviderHandler>();
builder.Services.AddScoped<IGetProviderByIdHandler, GetProviderByIdHandler>();
builder.Services.AddScoped<IGetProviderByNpiIdHandler, GetProviderByNpiIdHandler>();
builder.Services.AddScoped<IGetProvidersHandler, GetProvidersHandler>();
builder.Services.AddScoped<IAddPatientHandler, AddPatientHandler>();
builder.Services.AddScoped<IGetPatientByIdHandler, GetPatientByIdHandler>();
builder.Services.AddScoped<ISearchPatientHandler, SearchPatientHandler>();
builder.Services.AddScoped<IAddVisitHandler, AddVisitHandler>();
builder.Services.AddScoped<IGetAvailableSlotsHandler, GetAvailableSlotsHandler>();
builder.Services.AddScoped<IGetVisitByIdHandler, GetVisitByIdHandler>();
builder.Services.AddScoped<IGetVisitsByDateHandler, GetVisitsByDateHandler>();

builder.Services.AddPracticeDb();
builder.Services.AddDbRepositories();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/docs");
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/docs", "Practice API V1");
    });

    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.EnsureDbCreated();

app.MapInvoiceEndpoints();
app.MapPatientEndpoints();
app.MapProviderEndpoints();
app.MapVisitEndpoints();

app.Run();
