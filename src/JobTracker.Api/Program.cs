using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using JobTracker.Application.Companies;
using JobTracker.Infrastructure.Companies;
using FluentValidation;
using FluentValidation.AspNetCore;
using JobTracker.Application.Companies;
using JobTracker.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); });
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCompanyRequestValidator>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
