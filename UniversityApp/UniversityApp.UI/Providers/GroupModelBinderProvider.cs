using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.UI.Binders;

namespace UniversityApp.UI.Providers;

public class GroupModelBinderProvider : IModelBinderProvider
{
	public IModelBinder GetBinder(ModelBinderProviderContext context)
	{
		var binder = new GroupModelBinder();
		Type type = context.Metadata.ModelType;
		var result = type == typeof(Group) ? binder : null;
		return result!;
	}
}
