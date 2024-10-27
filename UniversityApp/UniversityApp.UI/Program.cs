using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.Interfaces;
using UniversityApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectLine = builder.Configuration.GetConnectionString("DefaultConnection");
if(connectLine == null)
{
    throw new ArgumentNullException("Connection string not found");
}

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(connectLine);
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

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

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
