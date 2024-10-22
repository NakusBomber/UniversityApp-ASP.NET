using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UniversityApp.Core.Entities;

namespace UniversityApp.Infrastructure;

public class ApplicationContext : DbContext
{
	private const string _pathToAppSettings = "appsettings.json";
	private const string _connectionName = "DefaultConnection";

	public DbSet<Student> Students { get; set; }
	public DbSet<Group> Groups { get; set; }
	public DbSet<Course> Courses { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile(_pathToAppSettings)
			.SetBasePath(Directory.GetCurrentDirectory())
			.Build();

		optionsBuilder
			.UseLazyLoadingProxies()
			.UseSqlServer(config.GetConnectionString(_connectionName));
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Student>()
			.HasIndex("FirstName", "LastName");

		modelBuilder.Entity<Group>()
			.HasIndex("Name");

		modelBuilder.Entity<Course>()
			.HasIndex("Name");
	}
}
