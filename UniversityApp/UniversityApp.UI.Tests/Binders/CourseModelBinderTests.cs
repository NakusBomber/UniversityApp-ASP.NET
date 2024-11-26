using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using UniversityApp.Core.Entities;
using UniversityApp.UI.Binders;

namespace UniversityApp.UI.Tests.Binders;

public class CourseModelBinderTests
{
	private readonly IModelBinder _binder;
	private readonly Course _course;
	private readonly IValueProvider _valueProviderWithId;
	private readonly IValueProvider _valueProviderEmptyId;
	private readonly IValueProvider _valueProviderEmptyName;

	public CourseModelBinderTests()
	{
		_binder = new CourseModelBinder();
		_course = new Course("Name", "Info");

		var mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Id"))
			.Returns(new ValueProviderResult(_course.Id.ToString()));
		mockValueProvider
			.Setup(p => p.GetValue("Name"))
			.Returns(new ValueProviderResult(_course.Name));
		mockValueProvider
			.Setup(p => p.GetValue("Description"))
			.Returns(new ValueProviderResult(_course.Description));

		_valueProviderWithId = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Name"))
			.Returns(new ValueProviderResult(_course.Name));
		mockValueProvider
			.Setup(p => p.GetValue("Description"))
			.Returns(new ValueProviderResult(_course.Description));

		_valueProviderEmptyId = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Name"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Description"))
			.Returns(new ValueProviderResult(_course.Description));

		_valueProviderEmptyName = mockValueProvider.Object;
	}

	[Fact]
	public void BindModelAsync_EntityWithId()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderWithId;

		_binder.BindModelAsync(context);
		var actual = context.Result.Model as Course;

		var expected = _course;
		Assert.Equal(actual, expected);
	}

	[Fact]
	public void BindModelAsync_EntityEmptyId()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderEmptyId;

		_binder.BindModelAsync(context);
		var actual = context.Result.Model as Course;

		Assert.NotNull(actual);
		Assert.True(actual.Id != _course.Id);
		Assert.True(actual.Name == _course.Name);
		Assert.True(actual.Description == _course.Description);
	}

	[Fact]
	public void BindModelAsync_ThrowsArgumentExceptionWhenNameEmpty()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderEmptyName;

		Assert.ThrowsAsync<ArgumentException>(() =>
		{
			return _binder.BindModelAsync(context);
		});
	}
}
