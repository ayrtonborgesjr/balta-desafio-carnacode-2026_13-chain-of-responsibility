using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Console.Domain.Approvers;

public class ManagerApprover : ExpenseApproverBase
{
    private readonly decimal _approvalLimit;

    public ManagerApprover(decimal approvalLimit)
    {
        _approvalLimit = approvalLimit;
    }

    protected override bool CanApprove(ExpenseRequest request)
    {
        return request.Amount <= _approvalLimit;
    }

    protected override void ProcessApproval(ExpenseRequest request)
    {
        System.Console.WriteLine("[Gerente] Analisando pedido...");

        if (ValidateReceipt(request) &&
            CheckBudget(request.Department, request.Amount) &&
            CheckPolicy(request))
        {
            System.Console.WriteLine(
                $"✅ [Gerente] Despesa de R$ {request.Amount:N2} APROVADA");
        }
        else
        {
            System.Console.WriteLine(
                $"❌ [Gerente] Despesa de R$ {request.Amount:N2} REJEITADA");
        }
    }
}