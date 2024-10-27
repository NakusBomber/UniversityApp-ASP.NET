using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;

namespace UniversityApp.UI.Controllers;

public class CourseController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	public CourseController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	[Route("/courses")]
	public async Task<IActionResult> AllCourses()
	{
		var courses = await _unitOfWork.CourseRepository.GetAsync();
		return View(courses);
	}

	[Route("/course")]
	public async Task<IActionResult> Course(Guid id)
	{
		try
		{
			var course = await _unitOfWork.CourseRepository.GetByIdAsync(id);
			return View(course);
		}
		catch (Exception)
		{
			return View(null);
		}
		
	}
}
