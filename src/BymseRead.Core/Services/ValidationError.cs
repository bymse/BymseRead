using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BymseRead.Core.Services;

public static class ValidationError
{
    [DoesNotReturn]
    public static void Throw(string message)
    {
        throw new ValidationException(new ValidationResult(message, []), null, null);
    }
}