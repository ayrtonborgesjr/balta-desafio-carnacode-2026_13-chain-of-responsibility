using Reembolsos.Console.Domain.Approvers;
using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Tests.Domain.Approvers;

[Collection("Console Output Tests")]
public class ApprovalChainTests
{
    [Fact]
    public void ApprovalChain_ShouldRouteToCorrectApprover_ForSmallExpense()
    {
        // Arrange
        var chain = BuildApprovalChain();
        var request = new ExpenseRequest("Test User", 50m, "Small expense", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            chain.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("[Supervisor] Analisando pedido...", result);
            Assert.DoesNotContain("[Gerente] Analisando pedido...", result);
            Assert.DoesNotContain("[Diretor] Analisando pedido...", result);
            Assert.DoesNotContain("[CEO] Analisando pedido...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ApprovalChain_ShouldRouteToCorrectApprover_ForMediumExpense()
    {
        // Arrange
        var chain = BuildApprovalChain();
        var request = new ExpenseRequest("Test User", 350m, "Medium expense", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            chain.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("Encaminhando para próximo nível...", result);
            Assert.Contains("[Gerente] Analisando pedido...", result);
            Assert.DoesNotContain("[Diretor] Analisando pedido...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ApprovalChain_ShouldRouteToCorrectApprover_ForLargeExpense()
    {
        // Arrange
        var chain = BuildApprovalChain();
        var request = new ExpenseRequest("Test User", 2500m, "Large expense", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            chain.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("[Diretor] Analisando pedido...", result);
            Assert.DoesNotContain("[CEO] Analisando pedido...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ApprovalChain_ShouldRouteToCorrectApprover_ForStrategicExpense()
    {
        // Arrange
        var chain = BuildApprovalChain();
        var request = new ExpenseRequest("Test User", 15000m, "Strategic expense", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            chain.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("[CEO] Analisando pedido...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ApprovalChain_ShouldPassThroughAllLevels_ForVeryLargeExpense()
    {
        // Arrange
        var chain = BuildApprovalChain();
        var request = new ExpenseRequest("Test User", 10000m, "Very large expense", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            chain.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("[SupervisorApprover] Valor acima do limite. Encaminhando", result);
            Assert.Contains("[ManagerApprover] Valor acima do limite. Encaminhando", result);
            Assert.Contains("[DirectorApprover] Valor acima do limite. Encaminhando", result);
            Assert.Contains("[CEO] Analisando pedido...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ApprovalChain_ShouldHandleEdgeCases_AtExactLimit()
    {
        // Arrange
        var chain = BuildApprovalChain();
        var request = new ExpenseRequest("Test User", 100m, "Exact limit", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            chain.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("[Supervisor] Analisando pedido...", result);
            Assert.DoesNotContain("[Gerente] Analisando pedido...", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ApprovalChain_ShouldReportWhenNoApproverAvailable()
    {
        // Arrange
        var supervisor = new SupervisorApprover(100m);
        var request = new ExpenseRequest("Test User", 200m, "Above limit", "Test Dept");
        using var output = new StringWriter();
        var originalOut = System.Console.Out;
        System.Console.SetOut(output);

        try
        {
            // Act
            supervisor.Approve(request);

            // Assert
            var result = output.ToString();
            Assert.Contains("Nenhum aprovador disponível para esta despesa", result);
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    private SupervisorApprover BuildApprovalChain()
    {
        var supervisor = new SupervisorApprover(100m);
        var manager = new ManagerApprover(500m);
        var director = new DirectorApprover(5000m);
        var ceo = new CEOApprover();

        supervisor
            .SetNext(manager)
            .SetNext(director)
            .SetNext(ceo);

        return supervisor;
    }
}

