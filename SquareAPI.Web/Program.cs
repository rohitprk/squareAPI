using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SquareAPI.Business;
using SquareAPI.Business.Constants;
using SquareAPI.Data;
using SquareAPI.Web;
using SquareAPI.Web.Models.v1;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Key),
        ClockSkew = TimeSpan.Zero
    };

    // override 401 response
    jwt.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = ApplicationConstant.JsonContentType;
            await context.Response.WriteAsync(new FailResponse
            {
                Message = $"{ResponseMessage.UnauthorizedAccess} {context.ErrorDescription}",
                Success = false
            }.ToString());
        }
    };
});

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISquareService, SquareService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IUserService, UserService>();

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
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http,
        Description = "JWT Authorization header."
    });
    c.OperationFilter<SwaggerAuthFilter>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// custom response and logging for all exceptions
app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = ApplicationConstant.JsonContentType;
            var response = new FailResponse
            {
                Message = ResponseMessage.ExceptionOccurred,
                Success = false
            };

            await context.Response.WriteAsJsonAsync<FailResponse>(response);
            var exceptionHandlerFeature =
                context.Features.Get<IExceptionHandlerFeature>();

            app.Logger.LogError(default(EventId), exceptionHandlerFeature?.Error, ResponseMessage.ExceptionOccurred);

        });
    });

app.Run();
