using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Console.Domain.Approvers;

public class SupervisorApprover : ExpenseApproverBase
{
    private readonly decimal _approvalLimit;

    public SupervisorApprover(decimal approvalLimit)
    {
        _approvalLimit = approvalLimit;
    }

    protected override bool CanApprove(ExpenseRequest request)
    {
        return request.Amount <= _approvalLimit;
    }

    protected override void ProcessApproval(ExpenseRequest request)
    {
        System.Console.WriteLine("[Supervisor] Analisando pedido...");

        if (ValidateReceipt(request) &&
            CheckBudget(request.Department, request.Amount))
        {
            System.Console.WriteLine(
                $"✅ [Supervisor] Despesa de R$ {request.Amount:N2} APROVADA");
        }
        else
        {
            System.Console.WriteLine(
                $"❌ [Supervisor] Despesa de R$ {request.Amount:N2} REJEITADA");
        }
    }
}