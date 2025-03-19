using Microsoft.EntityFrameworkCore;
using PastebinBackend.Data;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!String.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString: connectionString, new MySqlServerVersion(new Version(8, 0, 40)))
    );
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<FakeAnalyticDataService>();
builder.Services.AddHostedService<CleanupService>();

//CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseCors("CorsPolicy");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Paste}/{action=Create}"
);

app.Run();
