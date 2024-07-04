using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;
using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryUsecaseTestFixture))]
public class UpdateCategoryInputValidatorTest
{
    private readonly UpdateCategoryUsecaseTestFixture _fixture;

    public UpdateCategoryInputValidatorTest(UpdateCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(DoNotValidateWhenGuidIsEmpty))]
    public void DoNotValidateWhenGuidIsEmpty()
    {
        var input = _fixture.GetValidInput(Guid.Empty);
        var validator = new UpdateCategoryInputValidator();
        var result = validator.Validate(input);
        
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
    
    [Fact(DisplayName = nameof(ValidateWhenGuidIsValid))]
    public void ValidateWhenGuidIsValid()
    {
        var input = _fixture.GetValidInput();
        var validator = new UpdateCategoryInputValidator();
        var result = validator.Validate(input);
        
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }
}