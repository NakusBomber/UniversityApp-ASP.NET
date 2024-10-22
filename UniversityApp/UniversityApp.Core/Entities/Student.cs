using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.Entities;

[Table("Students")]
public class Student : Entity
{
	[Key]
	public override Guid Id { get ; set ; }

	[Required]
	[StringLength(50)]
	public string FirstName { get; set; }

	[Required]
	[StringLength(50)]
	public string LastName { get; set; }

	[ForeignKey(nameof(GroupId))]
	public virtual Group? Group { get; set; }

	public Guid? GroupId { get; set; }

	public Student(Guid id, string firstName, string lastName, Group? group = null)
	{
		Id = id;
		FirstName = firstName;
		LastName = lastName;
		Group = group;
		GroupId = group?.Id;
	}

	public Student(string firstName, string lastName, Group? group = null)
		: this(Guid.NewGuid(), firstName, lastName, group)
	{
	}

	public override int GetHashCode()
	{
		return (Id, GroupId, FirstName, LastName).GetHashCode();
	}

}
