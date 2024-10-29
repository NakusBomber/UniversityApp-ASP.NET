using UniversityApp.Core.Entities;

namespace UniversityApp.Core.Interfaces;

public interface IRepository<T> : ICrudAsync<T>
	where T : Entity
{
}
