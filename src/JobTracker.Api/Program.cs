using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using JobTracker.Application.Companies;
using JobTracker.Infrastructure.Companies;
using FluentValidation;
using FluentValidation.AspNetCore;
using JobTracker.Application.Companies;
using JobTracker.Api.Middleware;
using JobTracker.Application.JobPostings;
using JobTracker.Infrastructure.JobPostings;
using JobTracker.Application.JobApplications;
using JobTracker.Infrastructure.JobApplications;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSwaggerGen(c =>
{
    c.UseInlineDefinitionsForEnums();
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCompanyRequestValidator>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<ICompanyReadRepository, CompanyReadRepository>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<IJobPostingService, JobPostingService>();
builder.Services.AddScoped<IJobPostingReadRepository, JobPostingReadRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();


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
