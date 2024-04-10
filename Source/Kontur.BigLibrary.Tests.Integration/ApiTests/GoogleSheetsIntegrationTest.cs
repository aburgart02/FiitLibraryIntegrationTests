using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration.ApiTests;

public class GoogleSheetsIntegrationTest
{
    private readonly GoogleSheetsIntegration googleSheetsIntegration;
    private readonly string spreadsheetId = "1krG2GsklEyNKlHATY2sPcTqMFqk87Nbma2sPLq7oaZ8";
    public string range = "A1:D4";
    
    public GoogleSheetsIntegrationTest()
    {
        googleSheetsIntegration = new GoogleSheetsIntegration();
    }

    [Test]
    public void Should_Have_ExpectedStructure()
    {
        var expectedStructure = new List<string> { "Название книги", "№", "Описание", "Цена, Р" };
        var table = googleSheetsIntegration.ReadData(spreadsheetId, range);
        table.FirstOrDefault().Should().BeEquivalentTo(expectedStructure);
    }
    
    [Test]
    public void Should_Have_ExpectedBookPrices()
    {
        var expectedPrices = new List<string> { "100", "200", "300" };
        var table = googleSheetsIntegration.ReadData(spreadsheetId, range);
        table.Skip(1).Select(x => x[3]).Should().BeEquivalentTo(expectedPrices);
    }
}