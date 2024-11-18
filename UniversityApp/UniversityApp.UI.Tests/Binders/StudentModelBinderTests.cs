using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using UniversityApp.Core.Entities;
using UniversityApp.UI.Binders;

namespace UniversityApp.UI.Tests.Binders;

public class StudentModelBinderTests
{
	private readonly IModelBinder _binder;
	private readonly Student _student;
	private readonly IValueProvider _valueProviderWithId;
	private readonly IValueProvider _valueProviderEmptyId;
	private readonly IValueProvider _valueProviderEmptyFirstName;
	private readonly IValueProvider _valueProviderEmptyLastName;
	private readonly IValueProvider _valueProviderNotValidGroupId;

	public StudentModelBinderTests()
	{
		_binder = new StudentModelBinder();
		_student = new Student("First", "Last", Guid.NewGuid());

		var mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Student.Id"))
			.Returns(new ValueProviderResult(_student.Id.ToString()));
		mockValueProvider
			.Setup(p => p.GetValue("Student.FirstName"))
			.Returns(new ValueProviderResult(_student.FirstName));
		mockValueProvider
			.Setup(p => p.GetValue("Student.LastName"))
			.Returns(new ValueProviderResult(_student.LastName));
		mockValueProvider
			.Setup(p => p.GetValue("Student.GroupId"))
			.Returns(new ValueProviderResult(_student.GroupId.ToString()));
		_valueProviderWithId = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Student.Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Student.FirstName"))
			.Returns(new ValueProviderResult(_student.FirstName));
		mockValueProvider
			.Setup(p => p.GetValue("Student.LastName"))
			.Returns(new ValueProviderResult(_student.LastName));
		mockValueProvider
			.Setup(p => p.GetValue("Student.GroupId"))
			.Returns(new ValueProviderResult(_student.GroupId.ToString()));
		_valueProviderEmptyId = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Student.Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Student.FirstName"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Student.LastName"))
			.Returns(new ValueProviderResult(_student.LastName));
		mockValueProvider
			.Setup(p => p.GetValue("Student.GroupId"))
			.Returns(new ValueProviderResult(_student.GroupId.ToString()));
		_valueProviderEmptyFirstName = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Student.Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Student.FirstName"))
			.Returns(new ValueProviderResult(_student.FirstName));
		mockValueProvider
			.Setup(p => p.GetValue("Student.LastName"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Student.GroupId"))
			.Returns(new ValueProviderResult(_student.GroupId.ToString()));
		_valueProviderEmptyLastName = mockValueProvider.Object;

		mockValueProvider = new Mock<IValueProvider>();
		mockValueProvider
			.Setup(p => p.GetValue("Student.Id"))
			.Returns(ValueProviderResult.None);
		mockValueProvider
			.Setup(p => p.GetValue("Student.FirstName"))
			.Returns(new ValueProviderResult(_student.FirstName));
		mockValueProvider
			.Setup(p => p.GetValue("Student.LastName"))
			.Returns(new ValueProviderResult(_student.LastName));
		mockValueProvider
			.Setup(p => p.GetValue("Student.GroupId"))
			.Returns(new ValueProviderResult("NotValidId"));
		_valueProviderNotValidGroupId = mockValueProvider.Object;
	}

	[Fact]
	public void BindModelAsync_EntityWithId()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderWithId;

		_binder.BindModelAsync(context);
		var actual = context.Result.Model as Student;

		var expected = _student;
		Assert.Equal(actual, expected);
	}

	[Fact]
	public void BindModelAsync_EntityEmptyId()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderEmptyId;

		_binder.BindModelAsync(context);
		var actual = context.Result.Model as Student;

		Assert.NotNull(actual);
		Assert.True(actual.Id != _student.Id);
		Assert.True(actual.FirstName == _student.FirstName);
		Assert.True(actual.GroupId == _student.GroupId);
	}

	[Fact]
	public void BindModelAsync_ThrowsArgumentExceptionWhenFirstNameEmpty()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderEmptyFirstName;

		Assert.ThrowsAsync<ArgumentException>(() =>
		{
			return _binder.BindModelAsync(context);
		});
	}

	[Fact]
	public void BindModelAsync_ThrowsArgumentExceptionWhenLastNameEmpty()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderEmptyLastName;

		Assert.ThrowsAsync<ArgumentException>(() =>
		{
			return _binder.BindModelAsync(context);
		});
	}

	[Fact]
	public void BindModelAsync_ThrowsArgumentExceptionWhenCourseIdEmpty()
	{
		var context = new DefaultModelBindingContext();
		context.ValueProvider = _valueProviderNotValidGroupId;

		Assert.ThrowsAsync<ArgumentException>(() =>
		{
			return _binder.BindModelAsync(context);
		});
	}
}
