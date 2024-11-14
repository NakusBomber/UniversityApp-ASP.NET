using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

[Route("students")]
public class StudentController : Controller
{
	private readonly IStudentService _studentService;

	public StudentController(IStudentService studentService)
	{
		_studentService = studentService;
	}

	[HttpGet]
	public async Task<IActionResult> AllStudents(Guid? groupId)
	{
		Expression<Func<Student, bool>>? expression = 
			groupId == null ? null : s => s.GroupId == groupId;

		var students = await _studentService.GetAsync(expression);
		var group = groupId == null ? null : students.FirstOrDefault()?.Group;

		var vm = new StudentsViewModel(students, group);
		return View(vm);
	}

	[Route("{id:guid}")]
	[HttpGet]
	public async Task<IActionResult> Student(Guid id)
	{
		try
		{
			var student = await _studentService.GetByIdAsync(id);
			return View(student);
		}
		catch (InvalidOperationException)
		{
			return NotFound();
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	[Route("create")]
	[HttpGet]
	public async Task<IActionResult> Create([FromServices] IGroupService groupService)
	{
		var groups = await groupService.GetAsync();
		var vm = new CreateEditStudentViewModel(groups);

		return View(vm);
	}

	[Route("create")]
	[HttpPost]
	public async Task<IActionResult> Create(Student student, [FromServices] IGroupService groupService)
	{
		try
		{
			if(!ModelState.IsValid)
			{
				var groups = await groupService.GetAsync();
				var vm = new CreateEditStudentViewModel(groups, student);
				return View(vm);
			}
			await _studentService.CreateAsync(student);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
		return RedirectToAction("AllStudents");
	}

	[Route("delete")]
	[HttpPost]
	public async Task<IActionResult> Delete(Guid id)
	{
		try
		{
			var student = await _studentService.GetByIdAsync(id);
			await _studentService.DeleteAsync(student);
		}
		catch (Exception)
		{
			return BadRequest();
		}
		return RedirectToAction("AllStudents");
	}

	[Route("edit")]
	[HttpGet]
	public async Task<IActionResult> Edit(Guid id, [FromServices] IGroupService groupService)
	{
		try
		{
			var groups = await groupService.GetAsync();
			var student = await _studentService.GetByIdAsync(id);

			var vm = new CreateEditStudentViewModel(groups, student);

			return View(vm);
		}
		catch (InvalidOperationException)
		{
			return BadRequest();
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	[Route("edit")]
	[HttpPost]
	public async Task<IActionResult> Edit(Student student, [FromServices] IGroupService groupService)
	{
		try
		{
			var oldStudent = await _studentService.GetByIdAsync(student.Id);
			if (Entity.AreEntitiesEqual(oldStudent, student))
			{
				return RedirectToAction("AllStudents");
			}

			if(!ModelState.IsValid)
			{
				var groups = await groupService.GetAsync();
				var vm = new CreateEditStudentViewModel(groups, student);
				return View(vm);
			}

			oldStudent.FirstName = student.FirstName;
			oldStudent.LastName = student.LastName;
			oldStudent.GroupId = student.GroupId;

			await _studentService.UpdateAsync(oldStudent);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
		return RedirectToAction("AllStudents");
	}
}
