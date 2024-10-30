using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.Interfaces;
using UniversityApp.Infrastructure;
using UniversityApp.Infrastructure.Providers;

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

builder.Services.AddControllersWithViews(opt =>
{
	opt.ModelBinderProviders.Insert(0, new CourseModelBinderProvider());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
