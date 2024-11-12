using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

[Route("groups")]
public class GroupController : Controller
{
    private readonly IGroupService _groupService;
    private readonly ICourseService _courseService;

    public GroupController(IGroupService groupService, ICourseService courseService)
    {
        _groupService = groupService;
        _courseService = courseService;
    }

    public async Task<IActionResult> AllGroups(Guid? courseId)
    {
		Expression<Func<Group, bool>>? expression =
				courseId == null ? null
								 : g => g.CourseId == courseId;

		var groups = await _groupService.GetAsync(expression);
        var vm = new GroupsViewModel(groups, courseId);
		return View(vm);
	}

    [Route("{id:guid}")]
    public async Task<IActionResult> Group(Guid id)
    {
		try
		{
            var group = await _groupService.GetByIdAsync(id);
            return View(group);
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
    public async Task<IActionResult> Create()
    {
        var courses = await _courseService.GetAsync();
        var vm = new CreateEditGroupViewModel(courses);
        return View(vm);
    }

    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> Create(Group group)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return View(group);
            }
            await _groupService.CreateAsync(group);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return RedirectToAction("AllGroups");
    }
}
