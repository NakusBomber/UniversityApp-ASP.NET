using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;

namespace UniversityApp.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationContext _context = new ApplicationContext();
	
	public IRepository<Student> StudentRepository { get; }
	public IRepository<Group> GroupRepository {  get; }
	public IRepository<Course> CourseRepository { get; }

	public UnitOfWork()
	{
		StudentRepository = new GeneralRepository<Student>(_context);
		GroupRepository = new GeneralRepository<Group>(_context);
		CourseRepository = new GeneralRepository<Course>(_context);
	}

}
