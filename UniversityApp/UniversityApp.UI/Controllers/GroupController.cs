using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

public class GroupController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public GroupController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Route("/groups")]
    public async Task<IActionResult> AllGroups(Guid? courseId)
    {
		Expression<Func<Group, bool>>? expression =
				courseId == null ? null
								 : g => g.CourseId == courseId;

		var groups = await _unitOfWork.GroupRepository.GetAsync(expression);
        var vm = new GroupsViewModel(groups, courseId);
		return View(vm);
	}

    [Route("/groups/{id:guid}")]
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
}
