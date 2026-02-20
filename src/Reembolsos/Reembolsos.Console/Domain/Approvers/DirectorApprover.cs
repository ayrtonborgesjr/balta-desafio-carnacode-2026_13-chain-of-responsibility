using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Console.Domain.Approvers;

public class DirectorApprover : ExpenseApproverBase
{
    private readonly decimal _approvalLimit;

    public DirectorApprover(decimal approvalLimit)
    {
        _approvalLimit = approvalLimit;
    }

    protected override bool CanApprove(ExpenseRequest request)
    {
        return request.Amount <= _approvalLimit;
    }

    protected override void ProcessApproval(ExpenseRequest request)
    {
        System.Console.WriteLine("[Diretor] Analisando pedido...");

        if (ValidateReceipt(request) &&
            CheckBudget(request.Department, request.Amount) &&
            CheckPolicy(request) &&
            CheckStrategicAlignment(request))
        {
            System.Console.WriteLine(
                $"✅ [Diretor] Despesa de R$ {request.Amount:N2} APROVADA");
        }
        else
        {
            System.Console.WriteLine(
                $"❌ [Diretor] Despesa de R$ {request.Amount:N2} REJEITADA");
        }
    }
}