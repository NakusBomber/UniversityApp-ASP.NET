using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.Entities;

public abstract class Entity
{
	[NotMapped]
	public string UniqueNameErrorMessage => $"{GetType().Name} with same name already exists";

	abstract public Guid Id { get; set; }

	public override bool Equals(object? obj)
	{
		if (obj is not Entity other || GetType() != obj.GetType())
		{
			return false;
		}

		return this.Id == other.Id;
	}

	public static bool operator ==(Entity? left, Entity? right)
	{
		if (ReferenceEquals(left, right))
		{
			return true;
		}

		if (left is null || right is null)
		{
			return false;
		}

		return left.Equals(right);
	}
	public static bool operator !=(Entity? left, Entity? right)
	{
		return !(left == right);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}

	public static bool AreEntitiesEqual<T>(T entity1, T entity2)
	{
		var type = typeof(T);
		var properties = type.GetProperties(
			System.Reflection.BindingFlags.Public |
			System.Reflection.BindingFlags.Instance
		);

		foreach (var property in properties)
		{
			var value1 = property.GetValue(entity1);
			var value2 = property.GetValue(entity2);

			if(typeof(Entity).IsAssignableFrom(property.PropertyType))
			{
				continue;
			}

			if (value1 == null && value2 != null || value1 != null && value2 == null)
			{
				return false;
			}

			if (value1 != null && value2 != null)
			{
				if (value1 is IEnumerable<Entity> && value1.GetType() != typeof(string))
				{
					var collection1 = ((IEnumerable<Entity>)value1).Cast<object>();
					var collection2 = ((IEnumerable<Entity>)value2).Cast<object>();

					if (!collection1.SequenceEqual(collection2))
					{
						return false;
					}
				}
				else
				{
					if (!value1.Equals(value2))
					{
						return false;
					}
				}
			}
		}

		return true;
	}
}

