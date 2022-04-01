using System.Reflection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using SquareAPI.Business;
using SquareAPI.Data;
using SquareAPI.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers(
    options =>
    {
        // add filter to set produceType directly to StatusCode 400 and 500
        options.Filters.Add(new Microsoft.AspNetCore.Mvc.ProducesResponseTypeAttribute(typeof(FailResponse), (int)StatusCodes.Status400BadRequest));
        options.Filters.Add(new Microsoft.AspNetCore.Mvc.ProducesResponseTypeAttribute(typeof(FailResponse), (int)StatusCodes.Status500InternalServerError));
    }
);

// Dapper context and repository injection
builder.Services.AddSingleton<SquareAPIContext>();
builder.Services.AddScoped<IUserPointsRepository, UserPointsRepository>();
builder.Services.AddScoped<SquareService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Set swagger doc UI data.
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Square API",
        Version = "v1",
        Description = "API to save co-ordinate points and reterive square from these points",
        Contact = new OpenApiContact
        {
            Name = "Rohit Kumar",
            Email = "-"
        },
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// to make all routing paths to lower case
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Use swagger in middleware
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseExceptionHandler("/exception");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// custom response and logging for all exceptions
app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var response = new FailResponse
            {
                Message = "Exception occurred!",
                Success = false
            };

            await context.Response.WriteAsJsonAsync<FailResponse>(response);
            var exceptionHandlerFeature =
                context.Features.Get<IExceptionHandlerFeature>();

            app.Logger.LogError(default(EventId), exceptionHandlerFeature?.Error, "Error Occurred!");

        });
    });

app.Run();
