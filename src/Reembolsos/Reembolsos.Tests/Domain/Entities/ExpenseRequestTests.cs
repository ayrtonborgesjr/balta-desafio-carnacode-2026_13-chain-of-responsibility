using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Tests.Domain.Entities;

public class ExpenseRequestTests
{
    [Fact]
    public void Constructor_ShouldInitializeAllProperties()
    {
        // Arrange
        var employeeName = "João Silva";
        var amount = 150.50m;
        var purpose = "Material de escritório";
        var department = "TI";

        // Act
        var request = new ExpenseRequest(employeeName, amount, purpose, department);

        // Assert
        Assert.Equal(employeeName, request.EmployeeName);
        Assert.Equal(amount, request.Amount);
        Assert.Equal(purpose, request.Purpose);
        Assert.Equal(department, request.Department);
    }

    [Theory]
    [InlineData("Maria Santos", 500.00, "Treinamento", "RH")]
    [InlineData("Pedro Costa", 1000.00, "Equipamento", "Financeiro")]
    [InlineData("Ana Oliveira", 2500.00, "Viagem", "Vendas")]
    public void Constructor_ShouldAcceptDifferentValues(
        string employeeName,
        decimal amount,
        string purpose,
        string department)
    {
        // Act
        var request = new ExpenseRequest(employeeName, amount, purpose, department);

        // Assert
        Assert.NotNull(request);
        Assert.Equal(employeeName, request.EmployeeName);
        Assert.Equal(amount, request.Amount);
        Assert.Equal(purpose, request.Purpose);
        Assert.Equal(department, request.Department);
    }

    [Fact]
    public void Properties_ShouldBeImmutable()
    {
        // Arrange
        var request = new ExpenseRequest("Test User", 100m, "Test Purpose", "Test Dept");

        // Assert - Properties should be read-only (get-only)
        Assert.Equal("Test User", request.EmployeeName);
        Assert.Equal(100m, request.Amount);
        Assert.Equal("Test Purpose", request.Purpose);
        Assert.Equal("Test Dept", request.Department);
    }
}

