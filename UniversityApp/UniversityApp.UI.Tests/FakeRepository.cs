using System.Linq.Expressions;
using System.Linq;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UniversityApp.UI.Tests;

public class FakeRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
	private readonly HashSet<TEntity> _set;

	public FakeRepository() : this(new HashSet<TEntity>())
	{
	}

	public FakeRepository(HashSet<TEntity> entities)
	{
		_set = entities;
	}

	private void Create(TEntity entity)
	{
		_set.Add(entity);
	}

	public async Task CreateAsync(TEntity entity)
	{
		await Task.Run(() => Create(entity));
	}

	private void Delete(TEntity entity)
	{
		_set.Remove(entity);
	}

	public async Task DeleteAsync(TEntity entity)
	{
		await Task.Run(() => Delete(entity));
	}

	private List<TEntity> Get(
		Expression<Func<TEntity, bool>>? filter = null,
		bool asNoTracking = false)
	{
		IQueryable<TEntity> hashSet = _set.AsQueryable();

		if (filter != null)
		{
			hashSet = hashSet.Where(filter);
		}

		return hashSet.ToList();
	}

	public async Task<List<TEntity>> GetAsync(
		Expression<Func<TEntity, bool>>? filter = null,
		bool asNoTracking = false)
	{
		return await Task.Run(() => Get(filter, asNoTracking));
	}

	private void Update(TEntity entity)
	{
		var entityRemove = _set.FirstOrDefault(e => e.Id == entity.Id);
		if (entityRemove != null)
		{
			_set.Remove(entityRemove);
			_set.Add(entity);
		}
		else
		{
			throw new ArgumentException("Entity not found");
		}
	}

	public async Task UpdateAsync(TEntity entity)
	{
		await Task.Run(() => Update(entity));
	}

	public async Task<TEntity> GetByIdAsync(Guid id)
	{
		var entity = await FindAsync(e => e.Id == id);
		if (entity == null)
		{
			throw new InvalidOperationException("Entity by this Id not found");
		}
		return entity;
	}

	private TEntity? Find(Expression<Func<TEntity, bool>> expression)
	{
		TEntity? entity = _set.FirstOrDefault(expression.Compile());
		return entity;
	}

	public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression)
	{
		return await Task.FromResult(Find(expression));
	}

}
