using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UniversityApp.Core.Entities;
using UniversityApp.UI.Binders;

namespace UniversityApp.UI.Providers;

public class CourseModelBinderProvider : IModelBinderProvider
{
	public IModelBinder GetBinder(ModelBinderProviderContext context)
	{
		CourseModelBinder binder = new CourseModelBinder();
		Type type = context.Metadata.ModelType;
		var result = type == typeof(Course) ? binder : null;
		return result!;
	}
}
