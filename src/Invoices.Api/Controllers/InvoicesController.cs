using Invoices.Application.Invoices;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly ICreateInvoiceCommandHandler _handler;

    public InvoicesController(ICreateInvoiceCommandHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceCommand command)
    {
        var result = await _handler.Create(command);

        return result.Match<IActionResult>(
            invoice => Accepted(invoice),
            errors => BadRequest(errors));
    }
}