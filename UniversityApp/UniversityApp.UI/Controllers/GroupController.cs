using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.Services;
using UniversityApp.UI.Models;

namespace UniversityApp.UI.Controllers;

[Route("groups")]
public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<IActionResult> AllGroups(Guid? courseId)
    {
		Expression<Func<Group, bool>>? expression =
				courseId == null ? null
								 : g => g.CourseId == courseId;

		var groups = await _groupService.GetAsync(expression);
		var course = courseId == null ? null : groups.FirstOrDefault()?.Course;
		var vm = new GroupsViewModel(groups, course);
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
    public async Task<IActionResult> Create([FromServices] ICourseService courseService)
    {
        var courses = await courseService.GetAsync();
        var vm = new CreateEditGroupViewModel(courses);
        return View(vm);
    }

    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> Create(Group group, [FromServices] ICourseService courseService)
    {
        try
        {
			await VerifyUniqueNameAndAddError(group);
			if (!ModelState.IsValid)
			{
				var courses = await courseService.GetAsync();
				var vm = new CreateEditGroupViewModel(courses, group);
				return View(vm);
			}
			await _groupService.CreateAsync(group);
		}
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return RedirectToAction("AllGroups");
    }

	[Route("delete")]
	[HttpPost]
	public async Task<IActionResult> Delete(Guid id)
	{
		try
		{
			var group = await _groupService.GetByIdAsync(id);
			if (!group.CanDelete())
			{
				throw new InvalidOperationException("Cannot delete this group");
			}
			await _groupService.DeleteAsync(group);
		}
		catch (Exception)
		{
			return BadRequest();
		}
		return RedirectToAction("AllGroups");
	}

	[Route("edit")]
	[HttpGet]
	public async Task<IActionResult> Edit(Guid id, [FromServices] ICourseService courseService)
	{
		try
		{
			var courses = await courseService.GetAsync();
			var group = await _groupService.GetByIdAsync(id);
			var vm = new CreateEditGroupViewModel(courses, group);
			return View(vm);
		}
		catch (InvalidOperationException)
		{
			return BadRequest();
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	[Route("edit")]
	[HttpPost]
	public async Task<IActionResult> Edit(Group group, [FromServices] ICourseService courseService)
	{
		try
		{
			var oldGroup = await _groupService.GetByIdAsync(group.Id);
			if (!Entity.AreEntitiesEqual(oldGroup, group))
			{
				if(oldGroup.Name != group.Name)
				{
					await VerifyUniqueNameAndAddError(group);
				}
				if (!ModelState.IsValid)
				{
					var courses = await courseService.GetAsync();
					var vm = new CreateEditGroupViewModel(courses, group);
					return View(vm);
				}
				oldGroup.CourseId = group.CourseId;
				oldGroup.Name = group.Name;
				await _groupService.UpdateAsync(oldGroup);
			}
			return RedirectToAction("AllGroups");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	private async Task VerifyUniqueNameAndAddError(Group group)
	{
		var isUniqueName = await _groupService.FindAsync(g => g.Name == group.Name);
		if(isUniqueName != null)
		{
			var message = group.UniqueNameErrorMessage;
			ModelState.AddModelError<CreateEditGroupViewModel>(vm => vm.Group!.Name, message);
		}
	}
}
