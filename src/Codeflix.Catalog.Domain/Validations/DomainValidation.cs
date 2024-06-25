using Codeflix.Catalog.Domain.Exceptions;

namespace Codeflix.Catalog.Domain.Validations;

public class DomainValidation
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target == null)
            throw new EntityValidationException($"{fieldName} should not be null");
    }
}