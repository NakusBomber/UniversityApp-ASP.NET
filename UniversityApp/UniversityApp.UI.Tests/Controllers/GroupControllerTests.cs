using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.Services;
using UniversityApp.UI.Controllers;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Tests.Controllers;

public class GroupControllerTests
{
	
	private IRepository<Group> GetRepo(params Group[] groups)
	{
		return new FakeRepository<Group>(groups.ToHashSet());
	}

	private GroupController GetControllerWithRepo(IRepository<Group> repo)
	{
		return new GroupController(new GroupService(repo));
	}

	private GroupController GetController(params Group[] groups)
	{
		var repo = GetRepo(groups);
		return new GroupController(new GroupService(repo));
	}

	private ICourseService GetCourseServiceMock()
	{
		var courseServiceMock = new Mock<ICourseService>();
		courseServiceMock
			.Setup(s => s.GetAsync(null, false))
			.ReturnsAsync(new List<Course>());
		return courseServiceMock.Object;
	}

	[Fact]
	public async Task Group_Get_SuccessWhenValidId()
	{
		var courseId = Guid.NewGuid();
		var group1 = new Group("Name1", courseId);
		var controller = GetController(group1);

		var result = Assert.IsType<ViewResult>(await controller.Group(group1.Id));

		Assert.NotNull(result);
		Assert.Equal(group1, result.Model as Group);
	}


	[Fact]
	public async Task Group_Get_NotFoundWhenInvalidId()
	{
		var controller = GetController();

		var result = Assert.IsType<NotFoundResult>(await controller.Group(Guid.NewGuid()));
	}

	[Fact]
	public async Task Create_Post_SuccessCreateWhenValidData()
	{
		var repo = GetRepo();
		var controller = GetControllerWithRepo(repo);
		var courseService = GetCourseServiceMock();
		var newGroup = new Group("NewGroup", Guid.NewGuid());
		
		var result = Assert
			.IsType<RedirectToActionResult>(await controller.Create(newGroup, courseService));

		Assert.NotNull(await repo.FindAsync(e => e.Id == newGroup.Id));
	}

	[Fact]
	public async Task Create_Post_FailureWhenNotUniqueName()
	{
		var repo = GetRepo(new Group("NewGroup", Guid.NewGuid()));
		var controller = GetControllerWithRepo(repo);
		var courseService = GetCourseServiceMock();
		var newGroup = new Group("NewGroup", Guid.NewGuid());

		var result = Assert
			.IsType<ViewResult>(await controller.Create(newGroup, courseService));

		var expected = new CreateEditGroupViewModel(new List<Course>(), newGroup);
		Assert.NotNull(result);
		var actual = result.Model as CreateEditGroupViewModel;
		Assert.NotNull(actual);
		Assert.False(controller.ModelState.IsValid);
		Assert.Equal(expected.Group, actual.Group);
	}

	[Fact]
	public async Task Delete_Post_SuccessDeleteWhenValidId()
	{
		var groupId = Guid.NewGuid();
		var repo = GetRepo(new Group(groupId, "NewGroup", Guid.NewGuid()));
		var controller = GetControllerWithRepo(repo);

		var result = Assert.IsType<RedirectToActionResult>(await controller.Delete(groupId));

		Assert.Null(await repo.FindAsync(e => e.Id == groupId));
	}

	[Fact]
	public async Task Delete_Post_FailureDeleteWhenCannotDelete()
	{
		var groupId = Guid.NewGuid();
		var groupMock = new Mock<Group>();
		groupMock.SetupProperty(g => g.Id, groupId);
		var student = new Student("FirstName", "LastName", groupId);
		groupMock.SetupProperty(g => g.Students, new List<Student>() { student });
		var repo = GetRepo(groupMock.Object);
		var controller = GetControllerWithRepo(repo);

		var result = Assert.IsType<BadRequestResult>(await controller.Delete(groupId));

		Assert.NotNull(await repo.FindAsync(e => e.Id == groupId));
	}

	[Fact]
	public async Task Delete_Post_FailureDeleteWhenInvalidId()
	{
		var group = new Group(Guid.NewGuid(), "Name", Guid.NewGuid());
		var invalidId = Guid.NewGuid();
		var repo = GetRepo(group);
		var controller = GetControllerWithRepo(repo);

		var result = Assert.IsType<BadRequestResult>(await controller.Delete(invalidId));

		Assert.NotNull(await repo.FindAsync(e => e.Id == group.Id));
	}

	[Fact]
	public async Task Edit_Get_SuccessWhenValidId()
	{
		var groupId = Guid.NewGuid();
		var group = new Group(groupId, "New", Guid.NewGuid());
		var repo = GetRepo(group);
		var controller = GetControllerWithRepo(repo);
		var courseService = GetCourseServiceMock();

		var result = Assert.IsType<ViewResult>(await controller.Edit(groupId, courseService));

		var expected = new CreateEditGroupViewModel(new List<Course>(), group);
		var actual = result.Model as CreateEditGroupViewModel;
		Assert.NotNull(actual);
		
		Assert.Equal(expected.Courses, actual.Courses);
		Assert.Equal(expected.Group, actual.Group);
	}

	[Fact]
	public async Task Edit_Get_FailureWhenInvalidId()
	{
		var groupId = Guid.NewGuid();
		var repo = GetRepo();
		var controller = GetControllerWithRepo(repo);
		var courseService = GetCourseServiceMock();

		Assert.IsType<BadRequestResult>(await controller.Edit(groupId, courseService));
	}

	[Fact]
	public async Task Edit_Post_SuccessEditWhenValidData()
	{
		var group1 = new Group("Name", Guid.NewGuid());
		var group2 = new Group("Name2", Guid.NewGuid());
		var repo = GetRepo(group1, group2);
		var controller = GetControllerWithRepo(repo);
		var courseService = GetCourseServiceMock();
		var newGroup = new Group(group2.Id, "Name3", Guid.NewGuid());

		var result = Assert.IsType<RedirectToActionResult>(await controller.Edit(newGroup, courseService));

		var actual = await repo.FindAsync(e => e.Id == group2.Id);
		Assert.NotNull(actual);
		Assert.Equal("Name3", actual.Name);
		Assert.Equal(newGroup.CourseId, actual.CourseId);
	}

	[Fact]
	public async Task Edit_Post_Status500WhenInvalidId()
	{
		var controller = GetController();
		var courseService = GetCourseServiceMock();
		var group = new Group("Name", Guid.NewGuid());

		var expected = StatusCodes.Status500InternalServerError;
		var result = Assert.IsType<StatusCodeResult>(await controller.Edit(group, courseService));

		Assert.Equal(expected, result.StatusCode);
	}

	[Fact]
	public async Task Edit_Post_FailureWhenNotUniqueName()
	{
		var group1 = new Group("Name", Guid.NewGuid());
		var group2 = new Group("Name2", Guid.NewGuid());
		var repo = GetRepo(group1, group2);
		var controller = GetControllerWithRepo(repo);
		var courseService = GetCourseServiceMock();
		var newGroup = new Group(group2.Id, "Name", Guid.NewGuid());

		var result = Assert.IsType<ViewResult>(await controller.Edit(newGroup, courseService));

		var actual = await repo.FindAsync(e => e.Id == group2.Id);
		Assert.NotNull(actual);
		Assert.Equal("Name2", actual.Name);
		Assert.Equal(group2.CourseId, actual.CourseId);
	}

	[Fact]
	public async Task Edit_Post_ReturnViewWhenModelNotValid()
	{
		var group = new Group("Name", Guid.NewGuid());
		var repo = GetRepo(group);
		var controller = GetControllerWithRepo(repo);
		var courseService = GetCourseServiceMock();
		controller.ModelState.AddModelError("Name", "");
		var newGroup = new Group(group.Id, "NewName", Guid.NewGuid());

		var result = Assert.IsType<ViewResult>(await controller.Edit(newGroup, courseService));
		var expected = new CreateEditGroupViewModel(new List<Course>(), newGroup);

		var actual = result.Model as CreateEditGroupViewModel;
		Assert.NotNull(actual);

		Assert.Equal(expected.Courses, actual.Courses);
		Assert.Equal(expected.Group, actual.Group);
	}
}
