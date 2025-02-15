﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.Entities;

[Table("Courses")]
public class Course : Entity
{
	[Key]
	public override Guid Id {  get; set; }

	[Required]
	[StringLength(50)]
	[RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Name should not start or end with a space.")]
	public string Name { get; set; }

	[StringLength(300)]
	public string? Description { get; set; }

	public virtual List<Group> Groups { get; set; } = new List<Group>();

	protected Course()
	{
		Name = string.Empty;
	}

	public Course(string name, string? description = null)
		: this(Guid.NewGuid(), name, description)
	{
	}

	public Course(Guid id, string name, string? description = null)
	{
		Id = id;
		Name = name;
		Description = string.IsNullOrEmpty(description) ? null : description;
	}

	public override int GetHashCode()
	{
		return (Id, Name, Description).GetHashCode();
	}
	public bool CanDelete()
	{
		return this.Groups.Count == 0;
	}
}
