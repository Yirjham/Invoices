using System.Collections.Generic;
using System.Threading.Tasks;
using Invoices.Domain;

namespace Invoices.Application.Common;

public interface IInvoicesRepository
{
    Task<List<Invoice>> GetAll();
    Task<Invoice> GetByNumber(string invoiceNumber);
    Task<bool> Add(Invoice invoice);
}