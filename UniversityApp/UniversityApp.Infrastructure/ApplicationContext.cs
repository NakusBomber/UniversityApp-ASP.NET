using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UniversityApp.Core.Entities;

namespace UniversityApp.Infrastructure;

public class ApplicationContext : DbContext
{
	public DbSet<Student> Students { get; set; }
	public DbSet<Group> Groups { get; set; }
	public DbSet<Course> Courses { get; set; }

	public ApplicationContext(DbContextOptions<ApplicationContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Student>(e =>
		{
			e.HasIndex("FirstName", "LastName");
		});

		modelBuilder.Entity<Group>(e =>
		{
			e.HasIndex("Name").IsUnique(true);
		});

		modelBuilder.Entity<Course>(e =>
		{
			e.HasIndex("Name").IsUnique(true);
		});
	}
}
