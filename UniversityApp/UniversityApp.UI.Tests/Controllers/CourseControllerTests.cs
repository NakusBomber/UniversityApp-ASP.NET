using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Services;
using UniversityApp.UI.Controllers;

namespace UniversityApp.UI.Tests.Controllers;

public class CourseControllerTests
{

	private IRepository<Course> GetRepo(params Course[] courses)
	{
		var fakeRepo = new FakeRepository<Course>(courses.ToHashSet());
		return fakeRepo;
	}

	private CourseController GetCourseController(params Course[] courses)
	{
		var repo = GetRepo(courses);
		return new CourseController(new CourseService(repo));
	}

	private CourseController GetCourseControllerWithRepo(IRepository<Course> repo)
	{
		var service = new CourseService(repo);
		return new CourseController(service);
	}

	[Fact]
	public async Task Course_Get_ValidId()
	{
		var course1 = new Course("History");
		var course2 = new Course("Math");
		var controller = GetCourseController(course1, course2);

		var result = (await controller.Course(course1.Id)) as ViewResult;
		
		Assert.NotNull(result);
		Assert.Equal(course1, result.Model);
	}

	[Fact]
	public async Task Course_Get_InvalidId()
	{
		var course1 = new Course("History");
		var course2 = new Course("Math");
		var controller = GetCourseController(course1, course2);

		var result = await controller.Course(Guid.NewGuid());
		
		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task Create_Post_SuccessCreateWhenValidData()
	{
		var course1 = new Course("History");
		var repo = GetRepo(course1);
		var newCourse = new Course("Math");
		var controller = GetCourseControllerWithRepo(repo);

		var result = await controller.Create(newCourse);

		Assert.NotNull(await repo.FindAsync(c => c.Name == "Math"));
		Assert.IsType<RedirectToActionResult>(result);
	}

	[Fact]
	public async Task Create_Post_FailureCreateWhenNotUniqueName()
	{
		var course1 = new Course("History");
		var repo = GetRepo(course1);
		var newCourse = new Course(course1.Name);
		var controller = GetCourseControllerWithRepo(repo);

		var result = Assert.IsType<ViewResult>(await controller.Create(newCourse));

		Assert.NotNull(result);
		Assert.False(controller.ModelState.IsValid);
		Assert.Equal(newCourse, result.Model as Course);
	}

	[Fact]
	public async Task Edit_Get_ValidId()
	{
		var course1 = new Course("History");
		var course2 = new Course("Math");
		var controller = GetCourseController(course1, course2);

		var result = Assert.IsType<ViewResult>(await controller.Edit(course2.Id));

		Assert.NotNull(result);
		Assert.Equal(course2, result.Model as Course);
	}

	[Fact]
	public async Task Edit_Get_InvalidId()
	{
		var course1 = new Course("History");
		var repo = GetRepo(course1);
		var course2 = new Course("Math");
		var controller = GetCourseControllerWithRepo(repo);

		var result = await controller.Edit(course2.Id);

		Assert.IsType<BadRequestResult>(result);
	}

	[Fact]
	public async Task Edit_Post_SuccessEditWhenValidData()
	{
		var course1 = new Course("History");
		var repo = GetRepo(course1);
		var controller = GetCourseControllerWithRepo(repo);

		course1.Name = "Name";
		course1.Description = "Desc";
		var result = await controller.Edit(course1);
		var actual = await repo.FindAsync(e => e.Id == course1.Id);

		Assert.NotNull(actual);
		Assert.Equal(course1.Name, actual.Name);
		Assert.Equal(course1.Description, actual.Description);
		Assert.IsType<RedirectToActionResult>(result);
	}

	[Fact]
	public async Task Edit_Post_FailureEditWhenNotUniqueName()
	{
		var course1 = new Course("History");
		var course2 = new Course("Math");
		var repo = GetRepo(course1, course2);
		var controller = GetCourseControllerWithRepo(repo);

		var editCourse = new Course(course2.Id, "History", "Desc");
		var result = Assert.IsType<ViewResult>(await controller.Edit(editCourse));

		Assert.NotNull(result);
		Assert.False(controller.ModelState.IsValid);
		Assert.Equal(course2, result.Model as Course);
	}

	[Fact]
	public async Task Delete_Post_SuccessDeleteWhenValidId()
	{
		var course1 = new Course("History");
		var course2 = new Course("Math");
		var repo = GetRepo(course1, course2);
		var controller = GetCourseControllerWithRepo(repo);

		var result = Assert.IsType<RedirectToActionResult>(await controller.Delete(course2.Id));

		Assert.Null(await repo.FindAsync(e => e.Id == course2.Id));
	}

	[Fact]
	public async Task Delete_Post_BadRequestWhenCannotDelete()
	{
		var course1 = new Course("History");
		var guid = Guid.NewGuid();
		var course2 = new Course(guid, "Math")
		{
			Groups = new List<Group>() { new Group("NewName", guid) }
		};
		var repo = GetRepo(course1, course2);
		var controller = GetCourseControllerWithRepo(repo);

		var result = Assert.IsType<BadRequestResult>(await controller.Delete(course2.Id));

		Assert.NotNull(await repo.FindAsync(e => e.Id == course2.Id));
	}

	[Fact]
	public async Task Delete_Post_BadRequestWhenInvalidId()
	{
		var course1 = new Course("History");
		var newGuid = Guid.NewGuid();
		var repo = GetRepo(course1);
		var controller = GetCourseControllerWithRepo(repo);

		Assert.IsType<BadRequestResult>(await controller.Delete(newGuid));
	}
}
