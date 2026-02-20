using Reembolsos.Console.Domain.Approvers;
using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Tests.Domain.Approvers;

[Collection("Console Output Tests")]
public class CEOApproverTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Act
        var approver = new CEOApprover();

        // Assert
        Assert.NotNull(approver);
    }

    [Theory]
    [InlineData(100.00)]
    [InlineData(1000.00)]
    [InlineData(10000.00)]
    [InlineData(100000.00)]
    public void Approve_ShouldApproveAnyAmount(decimal amount)
    {
        // Arrange
        var approver = new CEOApprover();
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
            Assert.Contains("[CEO] Analisando pedido...", result);
            Assert.Contains("APROVADA", result);
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
        var approver = new CEOApprover();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => approver.Approve(null!));
    }

    [Fact]
    public void Approve_ShouldPerformAllChecks()
    {
        // Arrange
        var approver = new CEOApprover();
        var request = new ExpenseRequest("Test User", 50000m, "Strategic Investment", "Test Dept");
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
            Assert.Contains("Verificando aprovação do conselho...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void Approve_ShouldNotForwardToNext()
    {
        // Arrange
        var ceo = new CEOApprover();
        var request = new ExpenseRequest("Test User", 1000000m, "Huge Investment", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            ceo.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("[CEO] Analisando pedido...", result);
            Assert.DoesNotContain("Encaminhando para próximo nível...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }
}

