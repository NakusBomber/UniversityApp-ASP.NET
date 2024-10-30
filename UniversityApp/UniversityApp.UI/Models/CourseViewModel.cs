using UniversityApp.Core.Entities;

namespace UniversityApp.UI.Models;

public class CourseViewModel
{
	public Course Course { get; set; }
	public bool IsCanDelete { get; set; }

	public CourseViewModel(Course course, bool isCanDelete)
	{
		Course = course;
		IsCanDelete = isCanDelete;
	}
}
