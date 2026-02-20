using Reembolsos.Console.Domain.Approvers;
using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Tests.Domain.Approvers;

[Collection("Console Output Tests")]
public class ManagerApproverTests
{
    [Fact]
    public void Constructor_ShouldSetApprovalLimit()
    {
        // Arrange
        var limit = 500m;

        // Act
        var approver = new ManagerApprover(limit);

        // Assert
        Assert.NotNull(approver);
    }

    [Theory]
    [InlineData(100.00)]
    [InlineData(500.00)]
    [InlineData(499.99)]
    public void Approve_ShouldApproveWhenAmountIsWithinLimit(decimal amount)
    {
        // Arrange
        var limit = 500m;
        var approver = new ManagerApprover(limit);
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
            Assert.Contains("[Gerente] Analisando pedido...", result);
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
        var limit = 500m;
        var manager = new ManagerApprover(limit);
        var director = new DirectorApprover(5000m);
        manager.SetNext(director);
        var request = new ExpenseRequest("Test User", 1500m, "Test Purpose", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            manager.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("Valor acima do limite. Encaminhando para próximo nível...", result);
            Assert.Contains("[Diretor] Analisando pedido...", result);
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
        var approver = new ManagerApprover(500m);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => approver.Approve(null!));
    }

    [Fact]
    public void Approve_ShouldPerformAdditionalChecks()
    {
        // Arrange
        var approver = new ManagerApprover(500m);
        var request = new ExpenseRequest("Test User", 300m, "Test Purpose", "Test Dept");
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
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }
}

