using Codeflix.Catalog.Application.Usecases.Category.GetCategory;
using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.GetCategory;

[Collection(nameof(GetCategoryUsecaseTestFixture))]
public class GetCategoryInputValidatorTest
{
    private readonly GetCategoryUsecaseTestFixture _fixture;
    
    public GetCategoryInputValidatorTest(GetCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(ValidationOk))]
    public void ValidationOk()
    {
        var input = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidator();
        
        var validationResult = validator.Validate(input);
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);
    }
    
    [Fact(DisplayName = nameof(InvalidWhenEmptyId))]
    public void InvalidWhenEmptyId()
    {
        var input = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidator();
        
        var validationResult = validator.Validate(input);
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}