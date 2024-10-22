using UniversityApp.Core.Entities;

namespace UniversityApp.Core.Interfaces;

public interface IUnitOfWork
{
	public IRepository<Student> StudentRepository { get; }
	public IRepository<Group> GroupRepository { get; }
	public IRepository<Course> CourseRepository { get; }
}
