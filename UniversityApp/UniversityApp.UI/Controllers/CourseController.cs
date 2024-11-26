using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.UI.Controllers;

[Route("courses")]
public class CourseController : Controller
{
	private readonly ICourseService _courseService;

	public CourseController(ICourseService courseService)
	{
		_courseService = courseService;
	}

	public async Task<IActionResult> AllCourses()
	{
		var courses = await _courseService.GetAsync();
		return View(courses);
	}

	[Route("{id:guid}")]
	public async Task<IActionResult> Course(Guid id)
	{
		try
		{
			var course = await _courseService.GetByIdAsync(id);
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
			await VerifyUniqueNameAndAddError(course);
			if (!ModelState.IsValid)
			{
				return View(course);
			}
			await _courseService.CreateAsync(course);
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
			var course = await _courseService.GetByIdAsync(id);

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
			var oldCourse = await _courseService.GetByIdAsync(course.Id);
			if(!Entity.AreEntitiesEqual(oldCourse, course))
			{
				if (oldCourse.Name != course.Name)
				{
					await VerifyUniqueNameAndAddError(course);
				}
				if (!ModelState.IsValid)
				{
					return View(course);
				}
				oldCourse.Name = course.Name;
				oldCourse.Description = course.Description;
				await _courseService.UpdateAsync(oldCourse);
			}
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
			var course = await _courseService.GetByIdAsync(id);
			if(!course.CanDelete())
			{
				throw new InvalidOperationException("Cannot delete this course");
			}
			await _courseService.DeleteAsync(course);
		}
		catch (Exception)
		{
			return BadRequest();
		}
		return RedirectToAction("AllCourses");
	}

	private async Task VerifyUniqueNameAndAddError(Course course)
	{
		var isUniqueName = await _courseService.FindAsync(c => c.Name == course.Name);
		if(isUniqueName != null)
		{
			var message = course.UniqueNameErrorMessage;
			ModelState.AddModelError<Course>(c => c.Name, message);
		}
	}
}
