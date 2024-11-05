using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using UniversityApp.Core.Entities;
using UniversityApp.Core.Interfaces;
using UniversityApp.Infrastructure.Binders;

namespace UniversityApp.Infrastructure.Providers;

public class GroupModelBinderProvider : IModelBinderProvider
{
	public IModelBinder GetBinder(ModelBinderProviderContext context)
	{
		var unitOfWork = context.Services.GetRequiredService<IUnitOfWork>();
		var binder = new GroupModelBinder(unitOfWork);
		Type type = context.Metadata.ModelType;
		var result = type == typeof(Group) ? binder : null;
		return result!;
	}
}
