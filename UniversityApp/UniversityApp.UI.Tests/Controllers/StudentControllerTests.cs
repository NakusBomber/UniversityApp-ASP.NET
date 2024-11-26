using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Runtime.InteropServices;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.Services;
using UniversityApp.UI.Controllers;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Tests.Controllers;

public class StudentControllerTests
{
	private static IRepository<Student> GetRepo(params Student[] students)
	{
		return new FakeRepository<Student>(students.ToHashSet());
	}
	
	private static StudentController GetController(params Student[] students)
	{
		var repo = GetRepo(students);
		return GetControllerWithRepo(repo);
	}

	private static StudentController GetControllerWithRepo(IRepository<Student> repository)
	{
		var studentService = new StudentService(repository);
		return new StudentController(studentService);
	}

	[Fact]
	public async Task Student_Get_SuccessWhenValidId()
	{
		var student = new Student("FirstName", "LastName");
		var controller = GetController(student);

		var result = Assert.IsType<ViewResult>(await controller.Student(student.Id));

		Assert.NotNull(result);
		var actual = Assert.IsType<Student>(result.Model);
		Assert.Equal(student, actual);
	}

	[Fact]
	public async Task Student_Get_NotFoundWhenInvalidId()
	{
		var controller = GetController();

		Assert.IsType<NotFoundResult>(await controller.Student(Guid.NewGuid()));
	}

	[Fact]
	public async Task Student_Get_Status500WhenUnhandledException()
	{
		var studentServiceMock = new Mock<IStudentService>();
		studentServiceMock
			.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
			.ThrowsAsync(new Exception());
		var studentService = studentServiceMock.Object;
		var controller = new StudentController(studentService);

		var result = Assert.IsType<StatusCodeResult>(await controller.Student(Guid.NewGuid()));
		Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
	}

	[Fact]
	public async Task Create_Post_SuccessCreateWhenModelValid()
	{
		var repo = GetRepo();
		var controller = GetControllerWithRepo(repo);
		var student = new Student("FirstName", "LastName");
		var groupService = new Mock<IGroupService>().Object;

		var result = Assert.IsType<RedirectToActionResult>(await controller.Create(student, groupService));

		var actual = await repo.FindAsync(e => e.Id == student.Id);
		Assert.NotNull(actual);
	}

	[Fact]
	public async Task Create_Post_ReturnViewWhenErrorInModel()
	{
		var repo = GetRepo();
		var controller = GetControllerWithRepo(repo);
		var groupServiceMock = new Mock<IGroupService>();
		groupServiceMock.Setup(s => s.GetAsync(null, false)).ReturnsAsync(new List<Group>());
		var groupService = groupServiceMock.Object;
		var student = new Student("Name", "LastName");

		controller.ModelState.AddModelError("FirstName", string.Empty);
		var result = Assert.IsType<ViewResult>(await controller.Create(student, groupService));

		var actual = await repo.FindAsync(e => e.Id == student.Id);
		Assert.Null(actual);
		var vm = Assert.IsType<CreateEditStudentViewModel>(result.Model);
		Assert.Equal(new List<Group>(), vm.Groups);
		Assert.Equal(student, vm.Student);
	}

	[Fact]
	public async Task Create_Post_Status500WhenUnhandledException()
	{
		var groupService = new Mock<IGroupService>().Object;
		var studentServiceMock = new Mock<IStudentService>();
		studentServiceMock
			.Setup(s => s.CreateAsync(It.IsAny<Student>()))
			.ThrowsAsync(new Exception());
		var studentService = studentServiceMock.Object;
		var controller = new StudentController(studentService);

		var result = Assert.IsType<StatusCodeResult>(await controller.Create(new Student("", ""), groupService));

		Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
	}

	[Fact]
	public async Task Delete_Post_SuccessDeleteWhenValidId()
	{
		var student = new Student("FirstName", "LastName");
		var repo = GetRepo(student);
		var controller = GetControllerWithRepo(repo);

		var result = Assert.IsType<RedirectToActionResult>(await controller.Delete(student.Id));

		var actual = await repo.FindAsync(e => e.Id == student.Id);
		Assert.Null(actual);
	}

	[Fact]
	public async Task Delete_Post_BadRequestWhenInvalidId()
	{
		var controller = GetController();

		var result = Assert.IsType<BadRequestResult>(await controller.Delete(Guid.NewGuid()));
	}

	[Fact]
	public async Task Edit_Get_SuccessWhenValidId()
	{
		var groupService = new Mock<IGroupService>().Object;
		var student = new Student("FirstName", "LastName");
		var repo = GetRepo(student);
		var controller = GetControllerWithRepo(repo);
		var expected = new CreateEditStudentViewModel(null!, student);

		var result = Assert.IsType<ViewResult>(await controller.Edit(student.Id, groupService));

		var actual = Assert.IsType<CreateEditStudentViewModel>(result.Model);
		Assert.Equal(expected.Groups, actual.Groups);
		Assert.Equal(expected.Student, actual.Student);
	}

	[Fact]
	public async Task Edit_Get_BadRequestWhenInvalidId()
	{
		var controller = GetController();
		var guid = Guid.NewGuid();
		var groupService = new Mock<IGroupService>().Object;

		Assert.IsType<BadRequestResult>(await controller.Edit(guid, groupService));
	}

	[Fact]
	public async Task Edit_Get_Status500WhenUnhandledException()
	{
		var controller = GetController();
		var guid = Guid.NewGuid();
		var groupServiceMock = new Mock<IGroupService>();
		groupServiceMock
			.Setup(s => s.GetAsync(null, false))
			.ThrowsAsync(new Exception());
		var groupService = groupServiceMock.Object;

		var result = Assert.IsType<StatusCodeResult>(await controller.Edit(guid, groupService));
		Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
	}

	[Fact]
	public async Task Edit_Post_SuccessEditWhenModelValid()
	{
		var student = new Student("FirstName", "LastName");
		var repo = GetRepo(student);
		var controller = GetControllerWithRepo(repo);
		var groupService = new Mock<IGroupService>().Object;
		var newStudent = new Student(student.Id, "NewName", "NewLast", Guid.NewGuid());

		var result = Assert.IsType<RedirectToActionResult>(await controller.Edit(newStudent, groupService));

		var actual = await repo.GetByIdAsync(student.Id);
		Assert.Equal(newStudent.FirstName, actual.FirstName);
		Assert.Equal(newStudent.LastName, actual.LastName);
		Assert.Equal(newStudent.GroupId, actual.GroupId);
	}

	[Fact]
	public async Task Edit_Post_ReturnViewWhenModelInvalid()
	{
		var student = new Student("FirstName", "LastName");
		var repo = GetRepo(student);
		var controller = GetControllerWithRepo(repo);
		var groupService = new Mock<IGroupService>().Object;
		var newStudent = new Student(student.Id, "NewName", "NewLast", Guid.NewGuid());
		controller.ModelState.AddModelError("FirstName", string.Empty);
		var expected = new CreateEditStudentViewModel(null!, newStudent);

		var result = Assert.IsType<ViewResult>(await controller.Edit(newStudent, groupService));

		var actual = Assert.IsType<CreateEditStudentViewModel>(result.Model);

		Assert.Equal(expected.Groups, actual.Groups);
		Assert.Equal(expected.Student, actual.Student);
		var s = await repo.GetByIdAsync(student.Id);
		Assert.Equal(student, s);
	}

	[Fact]
	public async Task Edit_Post_Status500WhenUnhandledException()
	{
		var studentServiceMock = new Mock<IStudentService>();
		studentServiceMock
			.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
			.ThrowsAsync(new Exception());
		var studentService = studentServiceMock.Object;
		var controller = new StudentController(studentService);
		var groupService = new Mock<IGroupService>().Object;
		var student = new Student("FirstName", "LastName");

		var result = Assert.IsType<StatusCodeResult>(await controller.Edit(student, groupService));

		Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
	}
}
