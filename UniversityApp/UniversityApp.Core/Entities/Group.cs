using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.Entities;

[Table("Groups")]
public class Group : Entity
{
	[Key]
	public override Guid Id { get; set; }

	[Required]
	[StringLength(50)]
	[RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Name should not start or end with a space.")]
	public string Name { get; set; }

	[ForeignKey(nameof(CourseId))]
	public virtual Course? Course { get; set; }

	public Guid CourseId { get; set; }

	public virtual List<Student> Students { get; set; } = new List<Student>();

	protected Group()
	{
		Name = string.Empty;
		Course = new Course(Guid.NewGuid(), string.Empty);
	}

	public Group(string name, Guid courseId)
		: this(Guid.NewGuid(), name, courseId)
	{
	}

	public Group(Guid id, string name, Guid courseId)
	{
		Id = id;
		Name = name;
		CourseId = courseId;
	}

	public override int GetHashCode()
	{
		return (Id, Name, CourseId).GetHashCode();
	}

	public bool CanDelete()
	{
		return !Students.Any();
	}
}
