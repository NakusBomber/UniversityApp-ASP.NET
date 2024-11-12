using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Infrastructure.Binders;

public class GroupModelBinder : IModelBinder
{
	private readonly ICourseService _courseService;

	public GroupModelBinder(ICourseService courseService)
	{
		_courseService = courseService;
	}

	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
		var idValues = bindingContext.ValueProvider.GetValue("Group.Id");
		var nameValues = bindingContext.ValueProvider.GetValue("Group.Name");
		var courseIdValues = bindingContext.ValueProvider.GetValue("Group.CourseId");

		if(nameValues == ValueProviderResult.None || 
			string.IsNullOrEmpty(nameValues.FirstValue) ||
			courseIdValues == ValueProviderResult.None)
		{
			throw new ArgumentException("Name and courseId are required");
		}

		var id = idValues == ValueProviderResult.None ? null : idValues.FirstValue;
		var name = nameValues.FirstValue;
		var courseIdStr = courseIdValues.FirstValue;

		if(!Guid.TryParse(courseIdStr, out var courseId))
		{
			throw new ArgumentException("CourseId not valid");
		}

		var course = await _courseService.GetByIdAsync(courseId);
		
		Group result;
		if(id == null || !Guid.TryParse(id, out var guid))
		{
			result = new Group(name, course);
		}
		else
		{
			result = new Group(guid, name, course);
		}

		bindingContext.Result = ModelBindingResult.Success(result);
	}
}
