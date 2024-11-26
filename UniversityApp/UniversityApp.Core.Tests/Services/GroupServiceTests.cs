using Moq;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Services;

namespace UniversityApp.Core.Tests.Services;

public class GroupServiceTests
{
	private readonly Mock<IRepository<Group>> _repo;
	private readonly IGroupService _service;

	public GroupServiceTests()
	{
		_repo = new Mock<IRepository<Group>>();
		_service = new GroupService(_repo.Object);
	}

	[Fact]
	public async Task GroupService_CreateAsync_ShouldCallRepositoryCreateAsync()
	{
		var group = new Group(string.Empty, Guid.Empty);
		await _service.CreateAsync(group);

		_repo.Verify(r => r.CreateAsync(group), Times.Once);
	}

	[Fact]
	public async Task GroupService_DeleteAsync_ShouldCallRepositoryDeleteAsync()
	{
		var group = new Group(string.Empty, Guid.Empty);
		await _service.DeleteAsync(group);

		_repo.Verify(r => r.DeleteAsync(group), Times.Once);
	}

	[Fact]
	public async Task GroupService_FindAsync_ShouldCallRepositoryFindAsync()
	{
		await _service.FindAsync(g => true);

		_repo.Verify(r => r.FindAsync(g => true), Times.Once);
	}

	[Fact]
	public async Task GroupService_GetAsync_ShouldCallRepositoryGetAsync()
	{
		await _service.GetAsync();

		_repo.Verify(r => r.GetAsync(null, false), Times.Once);
	}

	[Fact]
	public async Task GroupService_GetByIdAsync_ShouldCallRepositoryGetByIdAsync()
	{
		await _service.GetByIdAsync(Guid.Empty);

		_repo.Verify(r => r.GetByIdAsync(Guid.Empty), Times.Once);
	}

	[Fact]
	public async Task GroupService_UpdateAsync_ShouldCallRepositoryUpdateAsync()
	{
		var group = new Group(string.Empty, Guid.Empty);
		await _service.UpdateAsync(group);

		_repo.Verify(r => r.UpdateAsync(group), Times.Once);
	}
}
