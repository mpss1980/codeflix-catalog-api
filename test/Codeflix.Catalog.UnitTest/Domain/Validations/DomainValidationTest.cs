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
}