using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ODTLearning.Entities;
using ODTLearning.Repositories;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace ODTLearning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ODTLearning API", Version = "v1" });
                
            });


            // Register services
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITutorListRepository, TutorListRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IModaretorRepository, ModeratorRepository>();

            // Add DbContext;
            builder.Services.AddDbContext<DbminiCapstoneContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DB_MiniCapStone")));

            // Configure JWT authentication
            var secretKey = builder.Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddGoogle(googleOptions =>
            {
                // Read Google authentication settings from appsettings.json
                IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

                // Set ClientID and ClientSecret to access Google API
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                googleOptions.CallbackPath = "/signin-google"; // Ensure this matches the callback URI set in Google Console
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000")
                                             .WithOrigins("http://localhost:3001")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODTLearning API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();


            // Use CORS
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
