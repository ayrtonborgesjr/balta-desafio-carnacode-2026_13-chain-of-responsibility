using Reembolsos.Console.Domain.Approvers;
using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Tests.Domain.Approvers;

[Collection("Console Output Tests")]
public class SupervisorApproverTests
{
    [Fact]
    public void Constructor_ShouldSetApprovalLimit()
    {
        // Arrange
        var limit = 100m;

        // Act
        var approver = new SupervisorApprover(limit);

        // Assert
        Assert.NotNull(approver);
    }

    [Theory]
    [InlineData(50.00)]
    [InlineData(100.00)]
    [InlineData(99.99)]
    public void Approve_ShouldApproveWhenAmountIsWithinLimit(decimal amount)
    {
        // Arrange
        var limit = 100m;
        var approver = new SupervisorApprover(limit);
        var request = new ExpenseRequest("Test User", amount, "Test Purpose", "Test Dept");
        var output = new StringWriter();
        System.Console.SetOut(output);

        // Act
        approver.Approve(request);

        // Assert
        var result = output.ToString();
        Assert.Contains("[Supervisor] Analisando pedido...", result);
        Assert.Contains("APROVADA", result);
    }

    [Fact]
    public void Approve_ShouldForwardWhenAmountExceedsLimit()
    {
        // Arrange
        var limit = 100m;
        var supervisor = new SupervisorApprover(limit);
        var manager = new ManagerApprover(500m);
        supervisor.SetNext(manager);
        var request = new ExpenseRequest("Test User", 150m, "Test Purpose", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            supervisor.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("Valor acima do limite. Encaminhando para próximo nível...", result);
            Assert.Contains("[Gerente] Analisando pedido...", result);
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
        var approver = new SupervisorApprover(100m);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => approver.Approve(null!));
    }

    [Fact]
    public void SetNext_ShouldReturnNextApprover()
    {
        // Arrange
        var supervisor = new SupervisorApprover(100m);
        var manager = new ManagerApprover(500m);

        // Act
        var result = supervisor.SetNext(manager);

        // Assert
        Assert.Same(manager, result);
    }

    [Fact]
    public void SetNext_ShouldAllowFluentChaining()
    {
        // Arrange
        var supervisor = new SupervisorApprover(100m);
        var manager = new ManagerApprover(500m);
        var director = new DirectorApprover(5000m);

        // Act
        supervisor
            .SetNext(manager)
            .SetNext(director);

        // Assert - If no exception is thrown, chaining works
        Assert.NotNull(supervisor);
    }
}

