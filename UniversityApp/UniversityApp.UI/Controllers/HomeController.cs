using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;

namespace UniversityApp.UI.Controllers;

public class HomeController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	public HomeController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<IActionResult> Index()
	{
		var courses = await _unitOfWork.CourseRepository.GetAsync();
		return View(courses);
	}

}
