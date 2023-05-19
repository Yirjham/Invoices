using System.Collections.Generic;

namespace Invoices.Domain;

public class Invoice
{
    public string InvoiceNumber { get; set; }
    public string CustomerName { get; set; }
    public decimal Amount { get; set; }
    public List<Payment> Payments { get; set; }
}