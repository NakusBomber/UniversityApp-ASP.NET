using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.Services;

public class GroupService : IGroupService
{
	private readonly IRepository<Group> _groupRepository;

	public GroupService(IRepository<Group> groupRepository)
	{
		_groupRepository = groupRepository;
	}

	public async Task CreateAsync(Group entity) => await _groupRepository.CreateAsync(entity);

	public async Task DeleteAsync(Group entity) => await _groupRepository.DeleteAsync(entity);

	public async Task<Group?> FindAsync(Expression<Func<Group, bool>> expression) =>
		await _groupRepository.FindAsync(expression);

	public async Task<List<Group>> GetAsync(
		Expression<Func<Group, bool>>? filter = null,
		bool asNoTracking = false)
	{
		return await _groupRepository.GetAsync(filter, asNoTracking);
	}

	public async Task<Group> GetByIdAsync(Guid id) => await _groupRepository.GetByIdAsync(id);

	public async Task UpdateAsync(Group entity) => await _groupRepository.UpdateAsync(entity);
}
