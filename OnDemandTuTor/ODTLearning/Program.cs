using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ODTLearning.Entities;
using ODTLearning.Repositories;
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
        });

        // Register services
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<ITutorRepository, TutorRepository>();
        builder.Services.AddScoped<IStudentRepository, StudentRepository>();
        builder.Services.AddScoped<IModeratorRepository, ModeratorRepository>();
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddSingleton<IVnPayRepository, VnPayRepository>();


        // Add DbContext
        builder.Services.AddDbContext<DbminiCapstoneContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DB_MiniCapStone")));

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

        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}