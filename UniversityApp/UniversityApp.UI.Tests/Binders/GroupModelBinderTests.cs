using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using UniversityApp.Core.Entities;
using UniversityApp.UI.Binders;

namespace UniversityApp.UI.Tests.Binders;

public class GroupModelBinderTests
{
	private readonly IModelBinder _binder;
	private readonly Group _group;
	private readonly IValueProvider _valueProviderWithId;
	private readonly IValueProvider _valueProviderEmptyId;
	private readonly IValueProvider _valueProviderEmptyName;
	private readonly IValueProvider _valueProviderEmptyCourseId;

	public GroupModelBinderTests()
	{
		_binder = new GroupModelBinder();
		_group = new Group("Name", Guid.NewGuid());

		var mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Group.Id"))
			.Returns(new ValueProviderResult(_group.Id.ToString()));
		mockValueProvider
			.Setup(p => p.GetValue("Group.Name"))
			.Returns(new ValueProviderResult(_group.Name));
		mockValueProvider
			.Setup(p => p.GetValue("Group.CourseId"))
			.Returns(new ValueProviderResult(_group.CourseId.ToString()));
		_valueProviderWithId = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Group.Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Group.Name"))
			.Returns(new ValueProviderResult(_group.Name));
		mockValueProvider
			.Setup(p => p.GetValue("Group.CourseId"))
			.Returns(new ValueProviderResult(_group.CourseId.ToString()));
		_valueProviderEmptyId = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Group.Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Group.Name"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Group.CourseId"))
			.Returns(new ValueProviderResult(_group.CourseId.ToString()));
		_valueProviderEmptyName = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Group.Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Group.Name"))
			.Returns(new ValueProviderResult(_group.Name));
		mockValueProvider
			.Setup(p => p.GetValue("Group.CourseId"))
			.Returns(ValueProviderResult.None);
		_valueProviderEmptyCourseId = mockValueProvider.Object;
	}

	[Fact]
	public void BindModelAsync_EntityWithId()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderWithId;

		_binder.BindModelAsync(context);
		var actual = context.Result.Model as Group;

		var expected = _group;
		Assert.Equal(actual, expected);
	}

	[Fact]
	public void BindModelAsync_EntityEmptyId()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderEmptyId;

		_binder.BindModelAsync(context);
		var actual = context.Result.Model as Group;

		Assert.NotNull(actual);
		Assert.True(actual.Id != _group.Id);
		Assert.True(actual.Name == _group.Name);
		Assert.True(actual.CourseId == _group.CourseId);
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

	[Fact]
	public void BindModelAsync_ThrowsArgumentExceptionWhenCourseIdEmpty()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderEmptyCourseId;

		Assert.ThrowsAsync<ArgumentException>(() =>
		{
			return _binder.BindModelAsync(context);
		});
	}
}
