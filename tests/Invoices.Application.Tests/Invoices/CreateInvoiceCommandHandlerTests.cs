using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Invoices.Application.Common;
using Invoices.Application.Invoices;
using Invoices.Domain;
using Moq;
using Xunit;

namespace Invoices.Application.Tests.Invoices;

public class CreateInvoiceCommandHandlerTests
{
    private readonly CreateInvoiceCommandHandler _sut;
    private readonly Mock<IInvoicesRepository> _repository;

    public CreateInvoiceCommandHandlerTests()
    {
        _repository = new();
        _sut = new(_repository.Object);
    }

    [Fact]
    public async Task ReturnsValidInvoice_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateInvoiceCommand()
        {
            CustomerName = "Joe",
            InvoiceNumber = "123",
            Amount = 34
        };
        _repository.Setup(x => x.GetByNumber(command.InvoiceNumber)).ReturnsAsync((Invoice)null!);
        _repository.Setup(x => x.Add(It.IsAny<Invoice>())).ReturnsAsync(true);

        // Act
        var result = await _sut.Create(command);

        // Assert
        var expected = Map(command);
        var actual = result.Match<Invoice>(i => i, _ => throw new Exception());

        actual.Should().BeEquivalentTo(expected);
    }

    private static Invoice Map(CreateInvoiceCommand command)
    {
        return new Invoice
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