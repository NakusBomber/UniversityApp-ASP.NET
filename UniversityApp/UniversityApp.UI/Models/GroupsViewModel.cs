using UniversityApp.Core.Entities;

namespace UniversityApp.UI.Models;

public class GroupsViewModel
{
	public IEnumerable<Group> Groups { get; set; }
	public Guid? CourseId { get; set; }

	public GroupsViewModel(IEnumerable<Group> groups, Guid? courseId = null)
	{
		Groups = groups;
		CourseId = courseId;
	}
}
