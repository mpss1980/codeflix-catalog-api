using Codeflix.Catalog.Application.Usecases.Category.DeleteCategory;
using Codeflix.Catalog.Application.Usecases.Category.GetCategory;
using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryUsecaseTestFixture))]
public class DeleteCategoryInputValidatorTest
{
    private readonly DeleteCategoryUsecaseTestFixture _fixture;
    
    public DeleteCategoryInputValidatorTest(DeleteCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(ValidationOk))]
    public void ValidationOk()
    {
        var input = new DeleteCategoryInput(Guid.NewGuid());
        var validator = new DeleteCategoryInputValidator();
        
        var validationResult = validator.Validate(input);
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);
    }
    
    [Fact(DisplayName = nameof(InvalidWhenEmptyId))]
    public void InvalidWhenEmptyId()
    {
        var input = new DeleteCategoryInput(Guid.Empty);
        var validator = new DeleteCategoryInputValidator();
        
        var validationResult = validator.Validate(input);
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}