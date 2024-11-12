using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.Services;
using UniversityApp.Infrastructure;
using UniversityApp.Infrastructure.Providers;

var builder = WebApplication.CreateBuilder(args);

var connectLine = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new ArgumentNullException("Connection string not found");

builder.Services.AddLogging();
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(connectLine);
});
builder.Services.AddScoped<IRepository<Course>, GeneralRepository<Course>>();
builder.Services.AddScoped<IRepository<Group>, GeneralRepository<Group>>();
builder.Services.AddScoped<IRepository<Student>, GeneralRepository<Student>>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllersWithViews(opt =>
{
	opt.ModelBinderProviders.Insert(0, new CourseModelBinderProvider());
	opt.ModelBinderProviders.Insert(0, new GroupModelBinderProvider());
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