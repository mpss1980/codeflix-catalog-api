using Bogus;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Domain.Validations;
using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Domain.Validations;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    public void NotNullOk()
    {
        var fieldName = Faker.Random.String2(1, 10);
        var value = Faker.Random.String2(1, 100);
        Action action = () => DomainValidation.NotNull(value, fieldName);
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullError))]
    public void NotNullError()
    {
        string? value = null;
        var fieldName = Faker.Random.String2(1, 10);
        Action action = () => DomainValidation.NotNull(value, fieldName);
        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        var fieldName = Faker.Random.String2(1, 10);
        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Random.String2(1, 100);
        var fieldName = Faker.Random.String2(1, 10);
        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [MemberData(nameof(GenerateNamesSmallerThanMinLength), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        var fieldName = Faker.Random.String2(1, 10);
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters long");
    }

    public static IEnumerable<object[]> GenerateNamesSmallerThanMinLength(int testQuantity)
    {
        var faker = new Faker();
        for (var i = 0; i < testQuantity; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }


    [Theory(DisplayName = nameof(MinLengthOk))]
    [MemberData(nameof(GenerateNamesGreaterThanMinLength), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        var fieldName = Faker.Random.String2(1, 10);
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GenerateNamesGreaterThanMinLength(int testQuantity)
    {
        var faker = new Faker();
        for (var i = 0; i < testQuantity; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, example.Length - 1);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [MemberData(nameof(GenerateNamesGreaterThanMaxLength), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        var fieldName = Faker.Random.String2(1, 10);
        Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal to {maxLength} characters long");
    }

    public static IEnumerable<object[]> GenerateNamesGreaterThanMaxLength(int testQuantity)
    {
        var faker = new Faker();
        for (var i = 0; i < testQuantity; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - (new Random()).Next(1, example.Length - 1);
            yield return new object[] { example, maxLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [MemberData(nameof(GenerateNamesSmallerThanMaxLength), parameters: 10)]
    public void MaxLengthOk(string target, int maxLength)
    {
        var fieldName = Faker.Random.String2(1, 10);
        Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GenerateNamesSmallerThanMaxLength(int testQuantity)
    {
        var faker = new Faker();
        for (var i = 0; i < testQuantity; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, maxLength };
        }
    }
}