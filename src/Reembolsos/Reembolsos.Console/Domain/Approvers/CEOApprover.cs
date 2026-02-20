using Reembolsos.Console.Domain.Entities;

namespace Reembolsos.Console.Domain.Approvers;

public class CEOApprover : ExpenseApproverBase
{
    protected override bool CanApprove(ExpenseRequest request)
    {
        // Último nível da cadeia — pode analisar qualquer valor
        return true;
    }

    protected override void ProcessApproval(ExpenseRequest request)
    {
        System.Console.WriteLine("[CEO] Analisando pedido...");

        if (ValidateReceipt(request) &&
            CheckBudget(request.Department, request.Amount) &&
            CheckPolicy(request) &&
            CheckStrategicAlignment(request) &&
            CheckBoardApproval(request))
        {
            System.Console.WriteLine(
                $"✅ [CEO] Despesa de R$ {request.Amount:N2} APROVADA");
        }
        else
        {
            System.Console.WriteLine(
                $"❌ [CEO] Despesa de R$ {request.Amount:N2} REJEITADA");
        }
    }
}