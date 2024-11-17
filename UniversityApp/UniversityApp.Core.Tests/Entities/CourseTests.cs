using UniversityApp.Core.Entities;

namespace UniversityApp.Core.Tests.Entities;

public class CourseTests
{
	public static IEnumerable<object[]> GetData()
	{
		var courseNoGroups = new Course("Test");
		var courseWithGroup = new Course("Test2");
		courseWithGroup.Groups.Add(new Group("NewGroup", courseWithGroup.Id));

		return
		[
			[ courseNoGroups, true ],
			[ courseWithGroup, false ]
		];
	}

	[Theory]
	[MemberData(nameof(GetData))]
	public void Course_CanDelete_Basic(Course course, bool expected)
	{
		var actual = course.CanDelete();

		Assert.Equal(actual, expected);
	}
}
