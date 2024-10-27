using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;

namespace UniversityApp.Infrastructure;

public class GeneralRepository<TEntity> : IRepository<TEntity>
	where TEntity : Entity
{
	private readonly ApplicationContext _context;

	public GeneralRepository(ApplicationContext context)
	{
		_context = context;
	}

	public async Task CreateAsync(TEntity entity)
	{
		await _context.AddAsync(entity);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(TEntity entity)
	{
		var entities = _context.Set<TEntity>();
		void Delete(TEntity entity)
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				entities.Attach(entity);
			}
			entities.Remove(entity);
		}

		await Task.Run(() => Delete(entity));
		await _context.SaveChangesAsync();
	}

	public async Task<List<TEntity>> GetAsync(
		Expression<Func<TEntity, bool>>? filter = null, 
		bool asNoTracking = false)
	{
		IQueryable<TEntity> entities = _context.Set<TEntity>();
		if(filter != null)
		{
			entities = entities.Where(filter);
		}

		if(asNoTracking)
		{
			return await entities.AsNoTracking().ToListAsync();
		}

		return await entities.ToListAsync();
	}

	public async Task<TEntity> GetByIdAsync(Guid id)
	{
		var entity = await FindAsync(e => e.Id == id);
		if(entity == null)
		{
			throw new InvalidOperationException("Entity by this Id not found");
		}
		return entity;
	}

	public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression)
	{
		var entities = _context.Set<TEntity>();
		TEntity? entity = await entities.FirstOrDefaultAsync(expression);
		return entity;
	}

	public async Task UpdateAsync(TEntity entity)
	{
		var entities = _context.Set<TEntity>();
		void Update(TEntity entity)
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				entities.Attach(entity);
			}
			_context.Entry(entity).State = EntityState.Modified;
		}

		await Task.Run(() => Update(entity));
	}

}
