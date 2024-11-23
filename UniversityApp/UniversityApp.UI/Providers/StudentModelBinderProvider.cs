using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniversityApp.Core.Entities;
using UniversityApp.UI.Binders;

namespace UniversityApp.UI.Providers;

public class StudentModelBinderProvider : IModelBinderProvider
{
	public IModelBinder? GetBinder(ModelBinderProviderContext context)
	{
		var binder = new StudentModelBinder();

		return context.Metadata.ModelType == typeof(Student) ? binder : null;
	}
}
