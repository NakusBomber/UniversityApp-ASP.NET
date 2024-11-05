using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

[Route("groups")]
public class GroupController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public GroupController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> AllGroups(Guid? courseId)
    {
		Expression<Func<Group, bool>>? expression =
				courseId == null ? null
								 : g => g.CourseId == courseId;

		var groups = await _unitOfWork.GroupRepository.GetAsync(expression);
        var vm = new GroupsViewModel(groups, courseId);
		return View(vm);
	}

    [Route("{id:guid}")]
    public async Task<IActionResult> Group(Guid id)
    {
		try
		{
            var group = await _unitOfWork.GroupRepository.GetByIdAsync(id);
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
        var courses = await _unitOfWork.CourseRepository.GetAsync();
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
            await _unitOfWork.GroupRepository.CreateAsync(group);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return RedirectToAction("AllGroups");
    }
}
