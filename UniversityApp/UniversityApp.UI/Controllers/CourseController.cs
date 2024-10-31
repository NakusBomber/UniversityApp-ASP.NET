using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

[Route("courses")]
public class CourseController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	public CourseController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<IActionResult> AllCourses()
	{
		var courses = await _unitOfWork.CourseRepository.GetAsync();
		var courseVms = courses.Select(c => new CourseViewModel(c, c.CanDelete()));
		return View(courseVms);
	}

	[Route("{id:guid}")]
	public async Task<IActionResult> Course(Guid id)
	{
		try
		{
			var course = await _unitOfWork.CourseRepository.GetByIdAsync(id);
			return View(course);
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
	public IActionResult Create() => View();

	[Route("create")]
	[HttpPost]
	public async Task<IActionResult> Create(Course course)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return View(course);
			}
			await _unitOfWork.CourseRepository.CreateAsync(course);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
		return RedirectToAction("AllCourses");
	}

	[Route("edit")]
	[HttpGet]
	public async Task<IActionResult> Edit(Guid id)
	{
		try
		{
			var course = await _unitOfWork.CourseRepository.GetByIdAsync(id);

			return View(course);
		}
		catch (InvalidOperationException)
		{
			return BadRequest();
		}
		catch(Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	[Route("edit")]
	[HttpPost]
	public async Task<IActionResult> Edit(Course course)
	{
		try
		{
			if(!ModelState.IsValid)
			{
				return View(course);
			}
			await _unitOfWork.CourseRepository.UpdateAsync(course);

			return RedirectToAction("AllCourses");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	[Route("delete")]
	[HttpPost]
	public async Task<IActionResult> Delete(Guid id)
	{
		try
		{
			var course = await _unitOfWork.CourseRepository.GetByIdAsync(id);
			if(!course.CanDelete())
			{
				throw new InvalidOperationException("Cannot delete this course");
			}
			await _unitOfWork.CourseRepository.DeleteAsync(course);
		}
		catch (Exception)
		{
			return BadRequest();
		}
		return RedirectToAction("AllCourses");
	}

}
