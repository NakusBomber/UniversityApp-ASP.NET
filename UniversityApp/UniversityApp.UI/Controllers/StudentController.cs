using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

public class StudentController : Controller
{
	private readonly IStudentService _studentService;

	public StudentController(IStudentService studentService)
	{
		_studentService = studentService;
	}

	[Route("/students")]
	public async Task<IActionResult> AllStudents(Guid? groupId)
	{
		Expression<Func<Student, bool>>? expression = 
			groupId == null ? null : s => s.GroupId == groupId;
		var students = await _studentService.GetAsync(expression);
		var vm = new StudentsViewModel(students, groupId);
		return View(vm);
	}

	[Route("/students/{id:guid}")]
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
}
