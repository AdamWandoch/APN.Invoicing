using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace APN.Invoicing.API.Validation
{
    public static class Shared
    {
        public const string MonthMsg = $"Month must be between 1 and 12.";
        public const int MinMonth = 1;
        public const int MaxMonth = 12;
    }

    public class CalculateInvoiceParams
    {
        [BindRequired]
        [Range(Shared.MinMonth, Shared.MaxMonth, ErrorMessage = Shared.MonthMsg)]
        public short Month { get; set; }

        [BindRequired]
        public short Year { get; set; }
    }

    public class GetInvoiceParams
    {
        [BindRequired]
        public short CustomerID { get; set; }

        [BindRequired]
        [Range(Shared.MinMonth, Shared.MaxMonth, ErrorMessage = Shared.MonthMsg)]
        public short Month { get; set; }

        [BindRequired]
        public short Year { get; set; }
    }
}
