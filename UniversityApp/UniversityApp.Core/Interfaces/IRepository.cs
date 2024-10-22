using UniversityApp.Core.Entities;

namespace UniversityApp.Core.Interfaces;

public interface IRepository<T> : ICRUDAsync<T>
	where T : Entity
{
}
