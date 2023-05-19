using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoices.Application.Common;
using Invoices.Domain;

namespace Invoices.Application.Invoices;

public interface ICreateInvoiceCommandHandler
{
    Task<Result<Invoice>> Create(CreateInvoiceCommand command);
}

public class CreateInvoiceCommandHandler : ICreateInvoiceCommandHandler
{
    private readonly IInvoicesRepository _repository;

    public CreateInvoiceCommandHandler(IInvoicesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Invoice>> Create(CreateInvoiceCommand command)
    {
        var errors = await Validate(command);

        if (errors.Any())
        {
            return new(errors);
        }

        var invoice = Map(command);
        var isSuccess = await _repository.Add(invoice);

        if (!isSuccess) return new(new List<string> { "Not saved."});
        return new() {Entity = invoice};
    }

    private async Task<List<string>> Validate(CreateInvoiceCommand command)
    {
        var result = new List<string>();

        if (command is null)
        {
            result.Add("Command is null.");
            return result;
        }

        if (string.IsNullOrEmpty(command.InvoiceNumber)) 
            result.Add("Invoice number is required");

        if (string.IsNullOrEmpty(command.CustomerName))
            result.Add("The customer name is required");

        if (command.Amount <= 0) result.Add("The amount must be positive");

        if (command.Payments?.Sum(p => p.Amount) > command.Amount)
            result.Add("The sum of the payments must less or equal to the total invoice amount.");

        if (command.Payments?.Any(p => p.Amount <= 0 || string.IsNullOrEmpty(p.PaymentNumber)) ?? false)
            result.Add("The payments are invalid.");

        if (string.IsNullOrEmpty(command.InvoiceNumber) && await _repository.GetByNumber(command.InvoiceNumber) is not null)
            result.Add("The invoice already exists.");

        return result;
    }

    private static Invoice Map(CreateInvoiceCommand command)
    {
        return new Invoice()
        {
            Amount = command.Amount,
            CustomerName = command.CustomerName,
            InvoiceNumber = command.InvoiceNumber,
            Payments = command.Payments?.Select(p => new Payment
            {
                Amount = p.Amount, 
                PaymentNumber = p.PaymentNumber
            }).ToList()
        };
        
    }
}