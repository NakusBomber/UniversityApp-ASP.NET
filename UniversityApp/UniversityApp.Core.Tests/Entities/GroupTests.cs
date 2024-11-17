using UniversityApp.Core.Entities;

namespace UniversityApp.Core.Tests.Entities;

public class GroupTests
{
	public static IEnumerable<object[]> GetData()
	{
		var groupEmpty = new Group("NewGroup", Guid.NewGuid());
		var groupWithStudent = new Group("NoEmptyGroup", Guid.NewGuid());
		groupWithStudent.Students.Add(new Student("Name", "LastName"));

		return [[groupEmpty, true], [groupWithStudent, false]];
	}

	[Theory]
	[MemberData(nameof(GetData))]
	public void Group_CanDelete_Basic(Group group, bool expected)
	{
		var actual = group.CanDelete();

		Assert.Equal(actual, expected);
	}
}
