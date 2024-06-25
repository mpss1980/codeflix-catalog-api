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
        var value = Faker.Random.String2(1, 100);
        Action action = () => DomainValidation.NotNull(value, "Value");
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullError))]
    public void NotNullError()
    {
        string? value = null;
        Action action = () => DomainValidation.NotNull(value, "FieldName");
        action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");
        action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null or empty");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Random.String2(1, 100);
        Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [MemberData(nameof(GenerateNamesSmallerThanLength), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        Action action = () => DomainValidation.MinLength(target, minLength, "FieldName");
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"FieldName should be at least {minLength} characters long");
    }
    
    public static IEnumerable<object[]> GenerateNamesSmallerThanLength(int testQuantity)
    {
        var faker = new Faker();
        for (var i = 0; i < testQuantity; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[]{ example, minLength };
        }
    }
    
    
    [Theory(DisplayName = nameof(MinLengthOk))]
    [MemberData(nameof(GenerateNamesGreaterThanLength), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        Action action = () => DomainValidation.MinLength(target, minLength, "FieldName");
        action.Should().NotThrow();
    }
    
    public static IEnumerable<object[]> GenerateNamesGreaterThanLength(int testQuantity)
    {
        var faker = new Faker();
        for (var i = 0; i < testQuantity; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, example.Length - 1);
            yield return new object[]{ example, minLength };
        }
    }
}