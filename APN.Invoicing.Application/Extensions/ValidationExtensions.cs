using FluentValidation.Results;
using System.Text;

namespace APN.Invoicing.Application.Extensions;

public static class ValidationExtensions
{
    public static string AsString(this ValidationResult validation)
    {
        var stringBuilder = new StringBuilder();

        foreach (var error in validation.ToDictionary())
        {
            stringBuilder.Append($"{error.Key}: ");
            foreach (var message in error.Value)
            {
                stringBuilder.Append($"{message} ");
            }
        }

        return stringBuilder.ToString().Trim();
    }
}
