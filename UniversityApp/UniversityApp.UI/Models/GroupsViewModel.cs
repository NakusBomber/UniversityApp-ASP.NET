using UniversityApp.Core.Entities;

namespace UniversityApp.UI.Models;

public class GroupsViewModel
{
	public IEnumerable<Group> Groups { get; set; }
	public Course? Course { get; set; }

	public GroupsViewModel(IEnumerable<Group> groups, Course? course = null)
	{
		Groups = groups;
		Course = course;
	}
}
