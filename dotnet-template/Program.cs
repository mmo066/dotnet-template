using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.Configuration
    .AddJsonFile("./appsettings.local.json");
#endif

builder.Configuration
    .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


var config = builder.Configuration;
var origins = config.GetSection("AllowedOrigins").Get<string[]>();
var authority = config.GetSection("Authority").Get<string>();

// Env variables can be configured through "launchSettings.json" for local development
// and through deployment.yaml for each deployed environment
var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var jwksUrl = authority + "jwk";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,

            ValidateIssuer = true,
            ValidIssuer = authority,

            ValidateAudience = false,

            ValidateIssuerSigningKey = true,
            IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
            {
                using var httpClient = new HttpClient();
                var jwksString = httpClient.GetStringAsync(jwksUrl).Result;
                httpClient.Dispose();
                var jwks = new JsonWebKeySet(jwksString);
                return jwks.Keys;
            }
        };
    });

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (currentEnvironment != "Production")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers().RequireAuthorization();

app.Run();