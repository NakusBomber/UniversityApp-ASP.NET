using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.Services;

public class CourseService : ICourseService
{
	private readonly IRepository<Course> _courseRepository;

	public CourseService(IRepository<Course> courseRepository)
	{
		_courseRepository = courseRepository;
	}

	public async Task CreateAsync(Course entity) => await _courseRepository.CreateAsync(entity);

	public async Task DeleteAsync(Course entity) => await _courseRepository.DeleteAsync(entity);

	public async Task<Course?> FindAsync(Expression<Func<Course, bool>> expression) =>
		await _courseRepository.FindAsync(expression);

	public async Task<List<Course>> GetAsync(
		Expression<Func<Course, bool>>? filter = null, 
		bool asNoTracking = false)
	{
		return await _courseRepository.GetAsync(filter, asNoTracking);
	}

	public async Task<Course> GetByIdAsync(Guid id) => await _courseRepository.GetByIdAsync(id);

	public async Task UpdateAsync(Course entity) => await _courseRepository.UpdateAsync(entity);
}
