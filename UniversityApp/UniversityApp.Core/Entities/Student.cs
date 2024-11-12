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

	[NotMapped]
	public string FullName => $"{FirstName} {LastName}";

	protected Student()
	{
		FirstName = string.Empty;
		LastName = string.Empty;
	}

	public Student(Guid id, string firstName, string lastName, Guid? groupId = null)
	{
		Id = id;
		FirstName = firstName;
		LastName = lastName;
		GroupId = groupId;
	}

	public Student(string firstName, string lastName, Guid? groupId = null)
		: this(Guid.NewGuid(), firstName, lastName, groupId)
	{
	}

	public override int GetHashCode()
	{
		return (Id, GroupId, FirstName, LastName).GetHashCode();
	}

}
