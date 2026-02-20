namespace Reembolsos.Console.Domain.Entities;

public class ExpenseRequest
{
    public string EmployeeName { get; }
    public decimal Amount { get; }
    public string Purpose { get; }
    public string Department { get; }

    public ExpenseRequest(string employeeName, decimal amount, string purpose, string department)
    {
        EmployeeName = employeeName;
        Amount = amount;
        Purpose = purpose;
        Department = department;
    }
}