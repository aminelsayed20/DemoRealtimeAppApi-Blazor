using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.RealTimeServices.Connections;
using WebApi.Repository;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddSignalR();

builder.Services.AddControllers(); // Remove the second instance of this
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin() // allow any client
               .AllowAnyHeader()
               .AllowAnyMethod();
        // .AllowCredentials(); // Removed to avoid security issues
    });
});

builder.Services.AddSingleton<UserConnectionManager>();

builder.Services.AddTransient<IUserRepository, UserRepository>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
app.MapIdentityApi<AppUser>();
app.UseHttpsRedirection(); 
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapControllers();
app.MapFallbackToFile("index.html");


app.MapHub<SignalRConnectionHub>("/connect");

app.Run();
