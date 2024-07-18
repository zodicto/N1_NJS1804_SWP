using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ODTLearning.DAL.Entities;
using ODTLearning.BLL.Repositories;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
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
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        // Register DbContext
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DbminiCapstoneContext>(options =>
            options.UseSqlServer(connectionString));

        // Register services
        builder.Services.AddScoped< AccountRepository>();
        builder.Services.AddScoped< TutorRepository>();
        builder.Services.AddScoped<IStudentRepository, StudentRepository>();       
        builder.Services.AddScoped<IRevenueRepository, RevenueRepository>();
        builder.Services.AddScoped< RequestRepository>();
        builder.Services.AddScoped<ServiceOfTutorRepository>();
        builder.Services.AddSingleton<IVnPayRepository, VnPayRepository>();
        builder.Services.AddScoped<TutorProfileRepository>();
        builder.Services.AddScoped<TransactionRepository>();
        builder.Services.AddScoped<ReviewRepository>();
        builder.Services.AddScoped<ComplaintRepository>();
        builder.Services.AddScoped<NotificationRepository>();
        builder.Services.AddScoped<ClassRepository>();

        // Configure JWT authentication
        var secretKey = builder.Configuration["AppSettings:SecretKey"];
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddGoogle(googleOptions =>
        {
            IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
            googleOptions.ClientId = googleAuthNSection["ClientId"];
            googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
            googleOptions.CallbackPath = "/google-callback";
            googleOptions.Scope.Add("profile");
            googleOptions.Scope.Add("email");
            googleOptions.SaveTokens = true;

            googleOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
            googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            googleOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
            googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
            googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture");
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
                                  policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:7133")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();
                              });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
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
