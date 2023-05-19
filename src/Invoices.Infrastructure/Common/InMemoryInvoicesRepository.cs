using Invoices.Application.Common;
using Invoices.Domain;

namespace Invoices.Infrastructure.Common;

public class InMemoryInvoicesRepository : IInvoicesRepository
{
    private readonly Dictionary<string, Invoice> _invoices = new();
    public Task<List<Invoice>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Invoice> GetByNumber(string invoiceNumber)
    {
        return _invoices.TryGetValue(invoiceNumber, out var invoice) ? Task.FromResult(invoice) : null!;
    }

    public Task<bool> Add(Invoice invoice)
    {
        _invoices[invoice.InvoiceNumber] = invoice;
        return Task.FromResult(true);
    }
}