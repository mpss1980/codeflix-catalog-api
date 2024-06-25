using Bogus;
using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using DomainEntity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Domain.Entities.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture) => _categoryTestFixture = categoryTestFixture;


    [Fact(DisplayName = nameof(Instantiate))]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;
        Thread.Sleep(10);
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));
        category.CreatedAt.Should().BeAfter(datetimeBefore);
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActiveStatus(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;
        Thread.Sleep(10);
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));
        category.CreatedAt.Should().BeAfter(datetimeBefore);
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsInvalid))]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenNameIsInvalid(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(name!, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(validCategory.Name, null!);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameLengthIsSmallerThan3))]
    [MemberData(nameof(GetNameLengthIsSmallerThan3), parameters: 3)]
    public void InstantiateErrorWhenNameLengthIsSmallerThan3(string? invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(invalidName!, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Name should be at least 3 characters long");
    }

    public static IEnumerable<object[]> GetNameLengthIsSmallerThan3(int testQuantity)
    {
        var fixture = new CategoryTestFixture();
        for (var i = 0; i < testQuantity; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] { fixture.GetValidName()[..(isOdd ? 1 : 2)] };
        }
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameLengthIsGreaterThan255))]
    public void InstantiateErrorWhenNameLengthIsGreaterThan255()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "A").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Name should be less or equal to 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionLengthIsGreaterThan10K))]
    public void InstantiateErrorWhenDescriptionLengthIsGreaterThan10K()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10001).Select(_ => "A").ToArray());
        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Description should be less or equal to 10000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    public void Deactivate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    public void Update()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var newValues = new
            { Name = _categoryTestFixture.GetValidName(), Description = _categoryTestFixture.GetValidDescription() };

        validCategory.Update(newValues.Name, newValues.Description);

        validCategory.Name.Should().Be(newValues.Name);
        validCategory.Description.Should().Be(newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    public void UpdateOnlyName()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var newValues = _categoryTestFixture.GetValidCategory();
        var currentDescription = validCategory.Description;

        validCategory.Update(newValues.Name);

        validCategory.Name.Should().Be(newValues.Name);
        validCategory.Description.Should().Be(currentDescription);
    }


    [Fact(DisplayName = nameof(UpdateOnlyDescription))]
    public void UpdateOnlyDescription()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var newValues = _categoryTestFixture.GetValidCategory();
        var currentName = validCategory.Name;

        validCategory.Update(null, newValues.Description);

        validCategory.Name.Should().Be(currentName);
        validCategory.Description.Should().Be(newValues.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsInvalid))]
    [InlineData("")]
    [InlineData("    ")]
    public void UpdateErrorWhenNameIsInvalid(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => validCategory.Update(name!);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameLengthIsSmallerThan3))]
    [InlineData("A")]
    [InlineData("AB")]
    public void UpdateErrorWhenNameLengthIsSmallerThan3(string? invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => validCategory.Update(invalidName!);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameLengthIsGreaterThan255))]
    public void UpdateErrorWhenNameLengthIsGreaterThan255()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        Action action = () => validCategory.Update(invalidName);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Name should be less or equal to 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionLengthIsGreaterThan10K))]
    public void UpdateErrorWhenDescriptionLengthIsGreaterThan10K()
    {
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10001).Select(_ => "A").ToArray());
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => validCategory.Update(null, invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Description should be less or equal to 10000 characters long");
    }
}