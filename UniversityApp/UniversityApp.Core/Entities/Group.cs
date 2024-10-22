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
	public string Name { get; set; }

	[ForeignKey(nameof(CourseId))]
	public virtual Course Course { get; set; }

	public Guid CourseId { get; set; }

	public virtual List<Student> Students { get; set; } = new List<Student>();

	public Group(string name, Course course)
		: this(Guid.NewGuid(), name, course)
	{
	}

	public Group(Guid id, string name, Course course)
	{
		Id = id;
		Name = name;
		Course = course;
		CourseId = course.Id;
	}

	public override int GetHashCode()
	{
		return (Id, Name, CourseId).GetHashCode();
	}
}
