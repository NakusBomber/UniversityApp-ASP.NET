using UniversityApp.Core.Entities;

namespace UniversityApp.UI.Models;

public class CreateEditStudentViewModel
{
	public IEnumerable<Group> Groups { get; private set; }
	public Student? Student { get; private set; }

	public CreateEditStudentViewModel(IEnumerable<Group> groups, Student? student = null)
	{
		Groups = groups;
		Student = student;
	}
}
