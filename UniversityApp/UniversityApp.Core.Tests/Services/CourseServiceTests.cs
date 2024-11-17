using Moq;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.Services;

namespace UniversityApp.Core.Tests.Services;

public class CourseServiceTests
{
	private readonly Mock<IRepository<Course>> _repo;
	private readonly ICourseService _service;

	public CourseServiceTests()
	{
		_repo = new Mock<IRepository<Course>>();
		_service = new CourseService(_repo.Object);
	}

	[Fact]
	public async Task CourseService_CreateAsync_ShouldCallRepositoryCreateAsync()
	{
		var course = new Course(string.Empty);
		await _service.CreateAsync(course);

		_repo.Verify(r => r.CreateAsync(course), Times.Once);
	}

	[Fact]
	public async Task CourseService_DeleteAsync_ShouldCallRepositoryDeleteAsync()
	{
		var course = new Course(string.Empty);
		await _service.DeleteAsync(course);

		_repo.Verify(r => r.DeleteAsync(course), Times.Once);
	}

	[Fact]
	public async Task CourseService_FindAsync_ShouldCallRepositoryFindAsync()
	{
		await _service.FindAsync(c => true);

		_repo.Verify(r => r.FindAsync(c => true), Times.Once);
	}

	[Fact]
	public async Task CourseService_GetAsync_ShouldCallRepositoryGetAsync()
	{
		await _service.GetAsync();

		_repo.Verify(r => r.GetAsync(null, false), Times.Once);
	}

	[Fact]
	public async Task CourseService_GetByIdAsync_ShouldCallRepositoryGetByIdAsync()
	{
		await _service.GetByIdAsync(Guid.Empty);

		_repo.Verify(r => r.GetByIdAsync(Guid.Empty), Times.Once);
	}

	[Fact]
	public async Task CourseService_UpdateAsync_ShouldCallRepositoryUpdateAsync()
	{
		var course = new Course(string.Empty);
		await _service.UpdateAsync(course);

		_repo.Verify(r => r.UpdateAsync(course), Times.Once);
	}
}
