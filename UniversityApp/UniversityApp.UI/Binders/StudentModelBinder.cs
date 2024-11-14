using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniversityApp.Core.Entities;

namespace UniversityApp.UI.Binders;

public class StudentModelBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		var idValues = bindingContext.ValueProvider.GetValue("Student.Id");
		var firstNameValues = bindingContext.ValueProvider.GetValue("Student.FirstName");
		var lastNameValues = bindingContext.ValueProvider.GetValue("Student.LastName");
		var groupIdValues = bindingContext.ValueProvider.GetValue("Student.GroupId");

		if (firstNameValues == ValueProviderResult.None ||
			string.IsNullOrEmpty(firstNameValues.FirstValue) ||
			lastNameValues == ValueProviderResult.None ||
			string.IsNullOrEmpty(lastNameValues.FirstValue))
		{
			throw new ArgumentException("First and last name are required");
		}

		var id = idValues == ValueProviderResult.None ? null : idValues.FirstValue;
		var firstName = firstNameValues.FirstValue;
		var lastName = lastNameValues.FirstValue;
		Guid? groupId;

		if (groupIdValues != ValueProviderResult.None &&
			!string.IsNullOrEmpty(groupIdValues.FirstValue))
		{
			if (!Guid.TryParse(groupIdValues.FirstValue, out var groupIdParse))
			{
				throw new ArgumentException("GroupId not valid");
			}
			groupId = groupIdParse;
		}
		else
		{
			groupId = null;
		}

		Student result;

		if (id == null || !Guid.TryParse(id, out var guid))
		{
			result = new Student(firstName, lastName, groupId);
		}
		else
		{
			result = new Student(guid, firstName, lastName, groupId);
		}

		bindingContext.Result = ModelBindingResult.Success(result);
		return Task.CompletedTask;
	}
}
