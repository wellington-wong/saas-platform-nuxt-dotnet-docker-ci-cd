using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore;
using Sazzle.Infrastructure.Persistence;
using Sazzle.Application.Common.Interfaces;
using Sazzle.Application.Organizations;
using Sazzle.Infrastructure.Repositories;
using Sazzle.Application.Auth;
using Sazzle.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Sazzle.Api.Authorization;
using Sazzle.Application.Authorization;

using System.Text;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<SazzleDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SazzleDb")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<OrganizationService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))

    };
});

builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<InvitationService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("members:invite", policy =>
        policy.Requirements.Add(new PermissionRequirement("members:invite")));
    options.AddPolicy("members:remove", policy =>
        policy.Requirements.Add(new PermissionRequirement("members:remove")));
    options.AddPolicy("roles:manage", policy =>

        policy.Requirements.Add(new PermissionRequirement("roles:manage")));
    options.AddPolicy("billing:write", policy =>
        policy.Requirements.Add(new PermissionRequirement("billing:write")));
    options.AddPolicy("members:view", policy =>
        policy.Requirements.Add(new PermissionRequirement("members:view")));
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
});
    
var app = builder.Build();
    
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SazzleDbContext>();

    await db.Database.MigrateAsync();
    await DbSeeder.SeedSystemRolesAsync(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
    
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
