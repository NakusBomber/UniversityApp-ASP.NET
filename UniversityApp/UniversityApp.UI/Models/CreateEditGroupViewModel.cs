using UniversityApp.Core.Entities;

namespace UniversityApp.UI.Models;

public class CreateEditGroupViewModel
{
	public Group? Group { get; set; }
	public IEnumerable<Course> Courses { get; set; }

	public CreateEditGroupViewModel(IEnumerable<Course> courses, Group? group = null)
	{
		Group = group;
		Courses = courses;
	}
}
