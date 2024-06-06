using Domain.Models;
using Microsoft.EntityFrameworkCore;
using DatabaseAccess;
using Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Common.Encryption;
using Common.RedisImplementation;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddLog4Net();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<EmployeeTrackerContext>(optionsBuilder => optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DBString")));
builder.Services.AddStackExchangeRedisCache(options=>
{
    options.Configuration = builder.Configuration.GetConnectionString("ReddisString");
});
builder.Services.AddScoped<RedisService>();
builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection("EncryptionSettings"));
builder.Services.Configure<DatabaseOperations>(builder.Configuration.GetSection("DatabaseOperations"));
builder.Services.AddScoped<DatabaseOperations, DatabaseOperations>();
builder.Services.Configure<LoginAuthentication>(builder.Configuration.GetSection("DatabaseOperations"));
builder.Services.AddScoped<LoginAuthentication, LoginAuthentication>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true
		};
	});	
builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Please log in to access this resource.");
        }
        else
        {
            string loginUrl = $"/Login/Login";
            context.Response.Redirect(loginUrl);
        }
    }
    });
app.Use(async (context, next) =>
{

	var jwtTokenCookie = context.Request.Cookies["jwtToken"];
	if (!string.IsNullOrEmpty(jwtTokenCookie))
	{
		context.Request.Headers.Append("Authorization", "Bearer " + jwtTokenCookie);
	}
	await next();
});

app.UseRouting();
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization(); // Add authorization middleware
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
