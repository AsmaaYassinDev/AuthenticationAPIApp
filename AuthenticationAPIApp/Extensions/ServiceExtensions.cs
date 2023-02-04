using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


namespace AuthenticationAPIApp.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureWrapper(this IServiceCollection services)
        {

        }

        public static void ConfigureSwaggerAPI(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents  
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Auth API",
                    Version = "v1",
                    Description = "Auth API",
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { {new OpenApiSecurityScheme   {  Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                          Id = "Bearer" }}, new string[] { }
                    }});
            });

        }
        public static void ConfigureJWToken(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication().AddCookie("DefaultAuthenticationScheme").AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        }
    }
}
