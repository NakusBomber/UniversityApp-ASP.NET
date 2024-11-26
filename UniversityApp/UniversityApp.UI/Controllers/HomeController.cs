using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

public class HomeController : Controller
{

	[Route("/")]
	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Error(int statusCode)
	{
		return View("Error", statusCode);
	}

}
