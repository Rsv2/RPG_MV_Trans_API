using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RPG_MV_Trans_API.ContextFolder;
using RPG_MV_Trans_API.Models;
using RPG_MV_Trans_API.Models.JWT;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

// Add services to the container.

builder.Services.AddLogging();

builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("IdentityConnection"),
            new MySqlServerVersion(new Version(8, 0, 25))));

builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>();

const string signingSecurityKey = "YourPrivateSigningSecurityKey";
var signingKey = new SigningSymmetricKey(signingSecurityKey);
builder.Services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

const string encodingSecurityKey = "YourPrivateEncodingSecurityKey";
var encryptionEncodingKey = new EncryptingSymmetricKey(encodingSecurityKey);
builder.Services.AddSingleton<IJwtEncryptingEncodingKey>(encryptionEncodingKey);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RPG Maker MV/MZ Translation Api", Version = "v1.0.0" });
    var filePath = Path.Combine(AppContext.BaseDirectory, "RPG_MV_Trans_API.xml");
    options.IncludeXmlComments(filePath);
    options.IncludeXmlComments(filePath, includeControllerXmlComments: true);

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

    options.AddSecurityRequirement(securityRequirement);

});

var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;
var encryptingDecodingKey = (IJwtEncryptingDecodingKey)encryptionEncodingKey;
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "JwtBearer";
        options.DefaultChallengeScheme = "JwtBearer";
    })
    .AddJwtBearer("JwtBearer", jwtBearerOptions =>
    {
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingDecodingKey.GetKey(),
            TokenDecryptionKey = encryptingDecodingKey.GetKey(),

            ValidateIssuer = true,
            ValidIssuer = "TranslatorApi",

            ValidateAudience = true,
            ValidAudience = "TranslatiorClient",

            ValidateLifetime = false,

            ClockSkew = TimeSpan.FromSeconds(5)
        };
    });
//Для авторизации запросов независимо от роли переводчика, удобно добавить в БД Identity какое либо идентифицирующее утверждение Claim.
//В таком случае Вам необходимо будет добавить или использовать одно из утверждений Claim в Вашей базе данных Identity.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TransRPGMakerMVMZ", policy => policy.RequireClaim("game", "TransRPGMakerMVMZ"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();
