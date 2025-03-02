using Microsoft.OpenApi.Models;

namespace JWT_Authentication.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Creates a Swagger document named "v1".
            // Title = "JWT API" => Sets the API name in Swagger UI.
            // Version = "v1" => Defines API version.
            // This is the general Swagger setup.
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

            // Enable JWT Authentication in Swagger
            // This enables a "Authorize [Lock Icon]" button in Swagger, where users can enter a JWT token.
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",                 //Purpose: Header name where the token is passed.
                Type = SecuritySchemeType.Http,         //Purpose: Defines the scheme type as HTTP authentication.
                Scheme = "Bearer",                      //Purpose: Uses Bearer authentication (i.e., Authorization: Bearer <token>).
                BearerFormat = "JWT",                   //Purpose: Specifies that the token format is JWT.
                In = ParameterLocation.Header,          //Purpose: JWT token is sent in the header of requests.
                Description = "Enter JWT Token in the format: Bearer {your_token}"          //Purpose: Instructions for users in Swagger UI.
            });

            // This applies JWT authentication to all API endpoints in Swagger.
            // It tells Swagger that the "Bearer" security definition should be used when calling APIs.
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
            // Now, protected API endpoints will require authentication via JWT token in Swagger.
        });
        return services;
    }
}
