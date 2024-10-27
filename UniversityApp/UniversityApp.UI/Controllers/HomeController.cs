using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

public class HomeController : Controller
{

	public HomeController()
	{
	}

	[Route("/")]
	public IActionResult Index()
	{
		return View();
	}


}
