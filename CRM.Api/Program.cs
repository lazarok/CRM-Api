using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CRM.Api.Middlewares;
using CRM.Application;
using CRM.Application.Exceptions;
using CRM.Application.Models;
using CRM.Application.Services;
using CRM.Infrastructure.Persistence;
using CRM.Infrastructure.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IWorkContext, WorkContext>();

// Add services to the container.
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configurePolicy =>
    {
        configurePolicy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "CRM Api",
        Version = "v1",
        Description = "This Api will be responsible for overall data distribution and authorization.",
        Contact = new OpenApiContact
        {
            Name = "Contact Name",
            Email = "contact@mail.com",
            Url = new Uri("https://localhost/contact"),
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            }, new List<string>()
        },
    });
});

var app = builder.Build();

app.UseMiddleware<UserIdentityMiddleware>();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<TenantMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", " Api CRM");
    });
}

app.UseHttpsRedirection();

// Extensions
app.UseGlobalException();

app.MapControllers();

app.Run();

public static class Extensions
{
    public static void UseGlobalException(this WebApplication app)
    {
        _ = app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                
                context.Response.ContentType = "application/json";
                var responseModel = new ApiResponse();

                var error = exceptionHandlerPathFeature?.Error;

                if (error is CustomException ce)
                {
                    context.Response.StatusCode = (int) ce.StatusCode;

                    var message = ce.Message + ". " + string.Join(". ", ce.ErrorMessages ?? []);
                    
                    responseModel = ApiResponse.Error(ce.ResponseCode, message);
                }
                else
                {

                    if (!app.Environment.IsProduction())
                    {
                        responseModel = ApiResponse.Error(error?.Message ?? string.Empty);
                    }
                    else
                    {
                        // TODO log
                        responseModel = ApiResponse.Error("Please, contact the support service.");
                    }
                
                    switch (error)
                    {
                        case KeyNotFoundException e:
                            // not found error
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        default:
                            // unhandled error
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }

                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    DictionaryKeyPolicy = null,
                    PropertyNamingPolicy = null
                };

                await context.Response.WriteAsJsonAsync(responseModel, jsonSerializerOptions);
            });
        });
    }
}