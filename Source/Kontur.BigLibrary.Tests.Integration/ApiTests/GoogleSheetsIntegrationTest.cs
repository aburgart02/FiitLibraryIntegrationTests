using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration.ApiTests;

public class GoogleSheetsIntegrationTest
{
    private readonly GoogleSheetsIntegration googleSheetsIntegration;
    private readonly string spreadsheetId = "1krG2GsklEyNKlHATY2sPcTqMFqk87Nbma2sPLq7oaZ8";
    private readonly string range = "A1:D4";

    public GoogleSheetsIntegrationTest()
    {
        googleSheetsIntegration = new GoogleSheetsIntegration();
    }

    [Test]
    public void Should_Have_ExpectedStructure()
    {
        var expectedStructure = new List<string> { "Название книги", "№", "Описание", "Цена, Р" };
        var table = googleSheetsIntegration.ReadData(spreadsheetId, range);
        Assert.That(table.FirstOrDefault(), Is.EqualTo(expectedStructure));
    }
    
    [Test]
    public void Should_Have_ExpectedBookPrices()
    {
        var expectedPrices = new List<Tuple<string, string>>
        {
            new("Книга1", "100"),
            new("Книга2", "200"),
            new("Книга3", "300")
        };
        var table = googleSheetsIntegration.ReadData(spreadsheetId, range);
        table.Skip(1).Select(x => new Tuple<object, object>(x[0], x[3])).Should().BeEquivalentTo(expectedPrices);
    }
}