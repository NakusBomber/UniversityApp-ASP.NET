using System.Linq.Expressions;

namespace UniversityApp.Core.Interfaces;

public interface ICRUDAsync<T>
{
	public Task<List<T>> GetAsync(
		Expression<Func<T, bool>>? filter = null,
		bool asNoTracking = false);
	public Task<T> GetByIdAsync(Guid id);
	public Task CreateAsync(T entity);
	public Task UpdateAsync(T entity);
	public Task DeleteAsync(T entity);
}
