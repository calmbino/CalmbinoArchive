using FluentValidation.Results;

namespace CalmbinoArchive.Application.Extensions;

public static class ValidationResultExtensions
{
    public static KeyValuePair<string, object?> ToErrors(this ValidationResult validationResult)
    {
        return new("errors", validationResult.ToDictionary());
    }
}