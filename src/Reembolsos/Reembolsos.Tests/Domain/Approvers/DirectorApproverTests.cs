using Reembolsos.Console.Domain.Approvers;
using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Tests.Domain.Approvers;

[Collection("Console Output Tests")]
public class DirectorApproverTests
{
    [Fact]
    public void Constructor_ShouldSetApprovalLimit()
    {
        // Arrange
        var limit = 5000m;

        // Act
        var approver = new DirectorApprover(limit);

        // Assert
        Assert.NotNull(approver);
    }

    [Theory]
    [InlineData(1000.00)]
    [InlineData(5000.00)]
    [InlineData(4999.99)]
    public void Approve_ShouldApproveWhenAmountIsWithinLimit(decimal amount)
    {
        // Arrange
        var limit = 5000m;
        var approver = new DirectorApprover(limit);
        var request = new ExpenseRequest("Test User", amount, "Test Purpose", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            approver.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("[Diretor] Analisando pedido...", result);
            Assert.Contains("APROVADA", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void Approve_ShouldForwardWhenAmountExceedsLimit()
    {
        // Arrange
        var limit = 5000m;
        var director = new DirectorApprover(limit);
        var ceo = new CEOApprover();
        director.SetNext(ceo);
        var request = new ExpenseRequest("Test User", 15000m, "Test Purpose", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            director.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("Valor acima do limite. Encaminhando para próximo nível...", result);
            Assert.Contains("[CEO] Analisando pedido...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void Approve_ShouldThrowExceptionWhenRequestIsNull()
    {
        // Arrange
        var approver = new DirectorApprover(5000m);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => approver.Approve(null!));
    }

    [Fact]
    public void Approve_ShouldPerformStrategicAlignmentCheck()
    {
        // Arrange
        var approver = new DirectorApprover(5000m);
        var request = new ExpenseRequest("Test User", 3000m, "Test Purpose", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            approver.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("Validando nota fiscal...", result);
            Assert.Contains("Verificando orçamento do departamento", result);
            Assert.Contains("Verificando conformidade com política...", result);
            Assert.Contains("Verificando alinhamento estratégico...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }
}

