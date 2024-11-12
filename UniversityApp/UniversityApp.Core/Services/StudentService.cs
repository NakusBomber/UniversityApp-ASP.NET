using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.Services;

public class StudentService : IStudentService
{
	private readonly IRepository<Student> _studentRepository;

	public StudentService(IRepository<Student> studentRepository)
	{
		_studentRepository = studentRepository;
	}

	public async Task CreateAsync(Student entity) => await _studentRepository.CreateAsync(entity);

	public async Task DeleteAsync(Student entity) => await _studentRepository.DeleteAsync(entity);

	public async Task<Student?> FindAsync(Expression<Func<Student, bool>> expression) =>
		await _studentRepository.FindAsync(expression);

	public async Task<List<Student>> GetAsync(
		Expression<Func<Student, bool>>? filter = null, 
		bool asNoTracking = false)
	{
		return await _studentRepository.GetAsync(filter, asNoTracking);
	}

	public async Task<Student> GetByIdAsync(Guid id) => await _studentRepository.GetByIdAsync(id);

	public async Task UpdateAsync(Student entity) => await _studentRepository.UpdateAsync(entity);
}
