using E_commerce.Abstractions;
using E_commerce.Data;
using E_commerce.Emails;
using E_commerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-commerce API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (e.g. 'Bearer your-token')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<DatabaseSeedingService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllKnownOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:7233",
                "https://localhost:5001",
                "http://localhost"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailSender, EmailService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    var seedingService = scope.ServiceProvider.GetRequiredService<DatabaseSeedingService>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        await context.Database.MigrateAsync();

        await seedingService.SeedProductsAsync();

        await seedingService.SeedUsersAsync();

        await seedingService.SeedRatingsAsync();

        logger.LogInformation("Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-commerce API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAllKnownOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

await app.RunAsync();