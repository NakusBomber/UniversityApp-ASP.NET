using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

public class StudentController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	public StudentController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	[Route("/students")]
	public async Task<IActionResult> AllStudents(Guid? groupId)
	{
		Expression<Func<Student, bool>>? expression = 
			groupId == null ? null : s => s.GroupId == groupId;
		var students = await _unitOfWork.StudentRepository.GetAsync(expression);
		var vm = new StudentsViewModel(students, groupId);
		return View(vm);
	}

	[Route("/student")]
	public async Task<IActionResult> Student(Guid id)
	{
		try
		{
			var student = await _unitOfWork.StudentRepository.GetByIdAsync(id);
			return View(student);
		}
		catch (Exception)
		{
			return View(null);
		}
	}
}
