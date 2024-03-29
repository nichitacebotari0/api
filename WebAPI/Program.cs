using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Authorization;
using WebAPI.Models;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
const string localCorsPolicy = "allow_local";
const string fangsbuilderCorsPolicy = "allow_fangsbuilder";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: localCorsPolicy,
                      policy =>
                      {
                          policy.WithOrigins(
                              "http://localhost:4200",
                              "https://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
    options.AddPolicy(name: fangsbuilderCorsPolicy,
                      policy =>
                      {
                          policy.WithOrigins(
                              builder.Configuration.GetValue<string>("BaseUrl"),
                              "https:://api.fangsbuilder.com")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});

builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(
    builder.Configuration.GetConnectionString("Database")));

builder.Services.AddIdentityCore<ApplicationUser>(
    options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddHostedService<SqliteBackupHostedService>();

builder.Services.AddAuthentication(options =>
{
    //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:EncryptionKey"))),
        ValidateLifetime = true
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["auth_token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSingleton<IAuthorizationHandler, AugmentEditAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, BuildEditAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Super", policy => policy.AddRequirements(new SuperAuthorizationRequirement()));
    options.AddPolicy("AugmentEdit", policy => policy.AddRequirements(new AugmentEditRequirement()));
    options.AddPolicy("UserBuildEdit", policy => policy.AddRequirements(new BuildEditRequirement()));
});
builder.Services.AddScoped<IChangeLogger, ChangeLogger>();

var app = builder.Build();

// Apply migration on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    context.SaveChanges();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (app.Environment.IsDevelopment())
{
    app.UseCors(localCorsPolicy);
}
app.UseCors(fangsbuilderCorsPolicy);

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
