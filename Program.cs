using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PostsAPI.Data;
using PostsAPI.Entities;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
            
    });
    
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("PostsAPI");
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

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
   var roleManager = 
       scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
   
   var roles = new[] {"Admin", "User"};

   foreach (var role in roles)
   {
       if (!await roleManager.RoleExistsAsync(role))
           await roleManager.CreateAsync(new IdentityRole(role));
   }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = 
        scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    const string email = "admin@admin.com";
    string password = "posts#Da5";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new User();
        user.Email = email;
        user.UserName = email;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
    
    
}


app.Run();
