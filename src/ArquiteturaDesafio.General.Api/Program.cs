using ArquiteturaDesafio.Infrastructure.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.General.Api.Filters;
using Microsoft.OpenApi.Models;
using ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Configuration;
using MongoDB.Driver;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Context;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Seed;
using ArquiteturaDesafio.Core.Domain.Interfaces;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "General Api - Controle de AUTH, DEBIT, CREDIT, REPORTS com o banco POSTGREE e MONGODB", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the text input below. Example: \"Bearer 12345abcdef\""
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                }
            });
        });

        // Registro dos serviços
        builder.Services.AddInfrastructure(builder.Configuration);

        // Adicione o serviço de autenticação JWT
        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

        // Registra o filtro de exceção personalizado
        builder.Services.AddMvc(options =>
        {
            options.Filters.Add(new CustomExceptionFilter());
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        // Instanciar banco MongoDB
        using (var scope = app.Services.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            var _jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
            var initializer = new MongoDbInitializer(database);
            await initializer.InitializeAsync();

            try
            {
                using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                    context.Database.Migrate();
                    Console.WriteLine("Migration foi executado");

                    SeedData.Initialize(serviceScope.ServiceProvider, _jwtTokenService);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        await app.RunAsync();
    }
}