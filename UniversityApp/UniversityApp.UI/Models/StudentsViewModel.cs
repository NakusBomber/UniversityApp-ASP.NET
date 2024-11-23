using UniversityApp.Core.Entities;

namespace UniversityApp.UI.Models;

public class StudentsViewModel
{
	public IEnumerable<Student> Students { get; set; }
	public Group? Group { get; set; }

	public StudentsViewModel(IEnumerable<Student> students, Group? group = null)
	{
		Students = students;
		Group = group;
	}
}
