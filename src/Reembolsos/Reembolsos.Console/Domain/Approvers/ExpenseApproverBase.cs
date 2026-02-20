using Reembolsos.Console.Domain.Entities;
using Reembolsos.Console.Domain.Interfaces;

namespace Reembolsos.Console.Domain.Approvers;

public abstract class ExpenseApproverBase : IExpenseApprover
{
    private IExpenseApprover? _next;

    public IExpenseApprover SetNext(IExpenseApprover next)
    {
        _next = next;
        return next; // Permite encadeamento fluente
    }

    public void Approve(ExpenseRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (CanApprove(request))
        {
            ProcessApproval(request);
        }
        else if (_next is not null)
        {
            System.Console.WriteLine(
                $"[{GetType().Name}] Valor acima do limite. Encaminhando para próximo nível...");

            _next.Approve(request);
        }
        else
        {
            System.Console.WriteLine("❌ Nenhum aprovador disponível para esta despesa.");
        }
    }

    /// <summary>
    /// Define se o nível atual pode aprovar o valor.
    /// </summary>
    protected abstract bool CanApprove(ExpenseRequest request);

    /// <summary>
    /// Executa a lógica de aprovação do nível atual.
    /// </summary>
    protected abstract void ProcessApproval(ExpenseRequest request);

    #region Métodos Compartilhados (Simulação de Regras)

    protected bool ValidateReceipt(ExpenseRequest request)
    {
        System.Console.WriteLine("  → Validando nota fiscal...");
        return true;
    }

    protected bool CheckBudget(string department, decimal amount)
    {
        System.Console.WriteLine($"  → Verificando orçamento do departamento {department}...");
        return true;
    }

    protected bool CheckPolicy(ExpenseRequest request)
    {
        System.Console.WriteLine("  → Verificando conformidade com política...");
        return true;
    }

    protected bool CheckStrategicAlignment(ExpenseRequest request)
    {
        System.Console.WriteLine("  → Verificando alinhamento estratégico...");
        return true;
    }

    protected bool CheckBoardApproval(ExpenseRequest request)
    {
        System.Console.WriteLine("  → Verificando aprovação do conselho...");
        return true;
    }

    #endregion
}