using System.Collections.Generic;

namespace Invoices.Application.Invoices;

public record CreateInvoiceCommand
{
    public string InvoiceNumber { get; init; }
    public string CustomerName { get; init; }
    public decimal Amount { get; init; }
    public List<CreatePaymentCommand> Payments { get; init; }
}

public record CreatePaymentCommand
{
    public string PaymentNumber { get; init; }
    public decimal Amount { get; init; }
}