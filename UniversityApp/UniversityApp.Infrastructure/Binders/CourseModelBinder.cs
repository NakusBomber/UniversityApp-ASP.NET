using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniversityApp.Core.Entities;

namespace UniversityApp.Infrastructure.Binders;

public class CourseModelBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		var idValues = bindingContext.ValueProvider.GetValue("Id");
		var nameValues = bindingContext.ValueProvider.GetValue("Name");
		var descriptionValues = bindingContext.ValueProvider.GetValue("Description");

		if(nameValues == ValueProviderResult.None)
		{
			throw new ArgumentException("Name required");
		}

		var name = nameValues.FirstValue;
		var id = idValues == ValueProviderResult.None ? null : idValues.FirstValue;
		var description = descriptionValues == ValueProviderResult.None ? null : descriptionValues.FirstValue;

		Course result;
		if(id == null || !Guid.TryParse(id, out var guid))
		{
			result = new Course(name, description);
		}
		else
		{
			result = new Course(guid, name, description);
		}

		bindingContext.Result = ModelBindingResult.Success(result);
		return Task.CompletedTask;
	}
}
