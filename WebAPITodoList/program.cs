
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ToDoListApi.Middlewares;
using WebAPITodoList.Data;
using WebAPITodoList.Mappings;
using WebAPITodoList.Repositories;
using WebAPITodoList.Repositories.Interfaces;
using WebAPITodoList.Services;
using WebAPITodoList.Settings;

var builder = WebApplication.CreateBuilder(args);
// Configurazione CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        //policy.WithOrigins("http://localhost:5173/") // Aggiungi qui gli URL dei client autorizzati
        //       .AllowAnyMethod()
        //       .AllowAnyHeader()
        //       .AllowCredentials();
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Carica secrets.json solo in ambiente di sviluppo
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
var connectionString = builder.Configuration.GetConnectionString("TodoListContext");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'TodoListContext' not found.");
}

// Services de configuration
builder.Services.AddDbContext<MyToDoDbContext>(options =>
    options.UseSqlServer(connectionString));

//Aggiungi Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ToDo List API",
        Version = "v1",
        Description = "API for managing ToDo lists and users",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Your Name",
            Email = "jondoe@mio.com"
        }
    });

    // Schema di sicurezza JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Inserisci il token JWT nel formato: Bearer {token}"
    });

    // Applica lo schema a tutte le richieste protette
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


//Logging
builder.Services.AddLogging();


//Controllers
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile));

//Jwt
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

//Registra il servizi di token Jwt
builder.Services.AddSingleton<TokenService>();

//Aggiungi autenticazione Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = false, // puoi mettere true + Audience se vuoi testare
            ValidAudience = jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowAll");


app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();