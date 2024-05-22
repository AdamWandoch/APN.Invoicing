using APN.Invoicing.Application.DTO;
using APN.Invoicing.Application.ServiceInterfaces;
using APN.Invoicing.Domain.Entities;
using FluentValidation;

namespace APN.Invoicing.Application.Validation;

public class OperationValidator : AbstractValidator<OperationPostDTO>
{
    private readonly List<short> allowedTypesShort =
        Enum.GetValues(typeof(EnumOperationType)).Cast<short>().ToList();
    private readonly string typesDesc = $"({string.Join(", ", Enum.GetValues(typeof(EnumOperationType)).Cast<EnumOperationType>().ToList())})";
    private readonly List<int> allowedMonths = Enumerable.Range(1, 12).ToList();
    private readonly IOperationValidationService _validationService;

    public OperationValidator(IOperationValidationService validationService)
    {
        _validationService = validationService;

        RuleFor(o => o.ServiceID)
            .NotNull()
            .NotEmpty();

        RuleFor(o => o.CustomerID)
            .NotNull()
            .NotEmpty();

        RuleFor(o => o.Quantity)
            .NotNull().
            NotEmpty();

        RuleFor(o => o.Date)
            .NotNull()
            .NotEmpty();

        RuleFor(o => o.Month)
            .NotNull()
            .NotEmpty()
            .Must(month => allowedMonths.Contains(month))
                .WithMessage($"Month must have one of the following values: {string.Join(", ", allowedMonths)}");

        RuleFor(o => o.Year)
            .NotNull()
            .NotEmpty();

        RuleFor(o => o.Type)
            .NotNull()
            .NotEmpty()
            .Must(type => allowedTypesShort.Contains((short)type))
                .WithMessage($"Type must have one of the following values: {string.Join(", ", allowedTypesShort)} {typesDesc}");

        RuleFor(o => o.PricePerDay)
                .NotNull()
                .When(o => o.Type == EnumOperationType.Start)
                .WithMessage("PricePerDay cannot be null when Type is 1 (Start).");

        RuleFor(o => o)
                .MustAsync((operation, token) => _validationService.IsOperationAllowedAsync(operation, token))
                .WithName(operation => operation.GetType().Name)
                .WithMessage(operation => $"The operation of type {(short)operation.Type} ({operation.Type.ToString().ToLower()}) is not allowed "
                    + $"based on the history of operations for service ID {operation.ServiceID} and customer ID {operation.CustomerID} "
                    + $"for month {operation.Month} in the year {operation.Year}.");
    }
}
