using System.Reflection;
using Checkout.Domain;
using Checkout.Exceptions;
using Checkout.Filters;
using Checkout.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>());
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContextFactory<CheckoutDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"))
        .EnableSensitiveDataLogging());
    
    
//    .AddSqlite<CheckoutDbContext>(
//    builder.Configuration.GetConnectionString("Default"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
