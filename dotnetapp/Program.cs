using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using dotnetapp.Data;
using dotnetapp.Models;
using dotnetapp.Services;
using System;
using System.IO;
using log4net;


var logger = LogManager.GetLogger(typeof(Program));
logger.Info("Application started - testing log creation.");

// At the very start of your Main method, add:
Console.WriteLine("Current working directory: " + Directory.GetCurrentDirectory());

var builder = WebApplication.CreateBuilder(args);

// Add log4net support (ensure the log4net.config file is in your project root)
builder.Logging.AddLog4Net(".config/log4net.config");


// Add Controllers
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();//default token for each identity

// Configure JSON options (preserve property names)
builder.Services.AddMvc().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Add Authentication - JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    // Allow SignalR to retrieve the JWT token from the query string.
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Update CORS policy to allow specific origins and credentials
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://8081-aabcfacbacdffcbffdbecdcbacfecbecaeebe.premiumproject.examly.io", //Venky
                           "https://8081-afceddbaaffcbffdbecdcbacfecbecaeebe.premiumproject.examly.io", //Sanjay
                           "https://8081-ceeececeecbeefcbffdbecdcbacfecbecaeebe.premiumproject.examly.io", //ramakrishna
                           "https://8081-bafbfdacfcbffdbecdcbacfecbecaeebe.premiumproject.examly.io", //yakshith
                           "https://8081-dcecbcadabfcbffdbecdcbacfecbecaeebe.premiumproject.examly.io", //gowtham
                           "https://8081-effbdcbbdcfffdbfcbffdbecdcbacfecbecaeebe.premiumproject.examly.io") //vaishnavi
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Swagger + JWT Support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Register custom services
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<AnnouncementService>();
builder.Services.AddTransient<BlogPostService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddTransient<IEmailService, EmailService>();


builder.Services.AddEndpointsApiExplorer(); 

// Register SignalR services
builder.Services.AddSignalR();

var app = builder.Build();

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use the specific CORS policy defined above
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map SignalR hub endpoint.
app.MapHub<ChatHub>("/chatHub");

app.Run();
