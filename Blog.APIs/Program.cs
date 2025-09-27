using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options
    //.UseLazyLoadingProxies()
    .UseSqlServer(
        builder.Configuration.GetConnectionString("BlogDB")
        )
);

// add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();


// DI
builder.Services.AddScoped(typeof(IGenaricReposatory<>), typeof(GenaricReposatory<>));
builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddTransient<ICategoryService, CategoryService>();
//builder.Services.AddSingleton<ICategoryService, CategoryService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddAuthentication(op =>
{
    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});

// Add Policy
builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    op.AddPolicy("ManageOnly", policy => policy.RequireRole("Admin", "Editor"));
    op.AddPolicy("NormalUserOnly", policy => policy.RequireRole("Reader", "Editor"));
    op.AddPolicy("AllRoles", policy => policy.RequireRole("Reader", "Editor","Admin"));
    op.AddPolicy("AllClaims", policy => policy.RequireClaim("Key", "Test"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Db Initializer
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var roleManger = service.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitilazer.SeedRolesAsync(roleManger);
}



app.MapControllers();

app.Run();
