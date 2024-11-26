using Moq;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Services;

namespace UniversityApp.Core.Tests.Services;

public class StudentServiceTests
{
	private readonly Mock<IRepository<Student>> _repo;
	private readonly IStudentService _service;

	public StudentServiceTests()
	{
		_repo = new Mock<IRepository<Student>>();
		_service = new StudentService(_repo.Object);
	}

	[Fact]
	public async Task StudentService_CreateAsync_ShouldCallRepositoryCreateAsync()
	{
		var student = new Student(string.Empty, string.Empty);
		await _service.CreateAsync(student);

		_repo.Verify(r => r.CreateAsync(student), Times.Once);
	}

	[Fact]
	public async Task StudentService_DeleteAsync_ShouldCallRepositoryDeleteAsync()
	{
		var student = new Student(string.Empty, string.Empty);
		await _service.DeleteAsync(student);

		_repo.Verify(r => r.DeleteAsync(student), Times.Once);
	}

	[Fact]
	public async Task StudentService_FindAsync_ShouldCallRepositoryFindAsync()
	{
		await _service.FindAsync(s => true);

		_repo.Verify(r => r.FindAsync(s => true), Times.Once);
	}

	[Fact]
	public async Task StudentService_GetAsync_ShouldCallRepositoryGetAsync()
	{
		await _service.GetAsync();

		_repo.Verify(r => r.GetAsync(null, false), Times.Once);
	}

	[Fact]
	public async Task StudentService_GetByIdAsync_ShouldCallRepositoryGetByIdAsync()
	{
		await _service.GetByIdAsync(Guid.Empty);

		_repo.Verify(r => r.GetByIdAsync(Guid.Empty), Times.Once);
	}

	[Fact]
	public async Task StudentService_UpdateAsync_ShouldCallRepositoryUpdateAsync()
	{
		var student = new Student(string.Empty, string.Empty);
		await _service.UpdateAsync(student);

		_repo.Verify(r => r.UpdateAsync(student), Times.Once);
	}
}
