using UniversityApp.Core.Entities;

namespace UniversityApp.UI.Models;

public class StudentsViewModel
{
	public IEnumerable<Student> Students { get; set; }
	public Guid? GroupId { get; set; }

	public StudentsViewModel(IEnumerable<Student> students, Guid? groupId)
	{
		Students = students;
		GroupId = groupId;
	}
}
