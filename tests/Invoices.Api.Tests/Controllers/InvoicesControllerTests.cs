using System.Threading.Tasks;
using FluentAssertions;
using Invoices.Api.Controllers;
using Invoices.Application.Common;
using Invoices.Application.Invoices;
using Invoices.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Invoices.Api.Tests.Controllers;

public class InvoicesControllerTests
{
    private readonly InvoicesController _sut; // system under test

    private readonly Mock<ICreateInvoiceCommandHandler> _handler;

    public InvoicesControllerTests()
    {
        _handler = new();
        _sut = new(_handler.Object);
    }

    // Convention for naming WhatItDoes_When

    // Fixtures, Bogus

    [Fact]
    public async Task ReturnsAccepted_WhenHandlerReturnSuccess()
    {
        // Arrange
        var invoice = new Invoice();
        var command = new CreateInvoiceCommand();

        _handler.Setup(x => x.Create(command)).ReturnsAsync(invoice);

        // Act
        var response = await _sut.Create(command);

        //Assert
        var expected = new AcceptedResult { Value = invoice };
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenHandlerReturnsFailure()
    {
        //Arrange
        var errors = new ValidationErrors
        {
            "Bang boom bam!!!",
            "Boom boom"
        };
        var command = new CreateInvoiceCommand();

        _handler.Setup(x => x.Create(command)).ReturnsAsync(errors);

        //Act
        var response = await _sut.Create(command);

        //Assert
        var expected = new BadRequestObjectResult(errors);
        response.Should().BeEquivalentTo(expected);
    }
    
}