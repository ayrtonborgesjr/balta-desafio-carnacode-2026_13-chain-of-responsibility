using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Console.Domain.Interfaces;

public interface IExpenseApprover
{
    IExpenseApprover SetNext(IExpenseApprover next);
    void Approve(ExpenseRequest request);
}