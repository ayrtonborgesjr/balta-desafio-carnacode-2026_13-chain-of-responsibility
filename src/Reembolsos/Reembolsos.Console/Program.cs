using Microsoft.Extensions.Configuration;
using Reembolsos.Console.Domain.Approvers;
using Reembolsos.Console.Domain.Configuration;
using Reembolsos.Console.Domain.Entities;

Console.WriteLine("=== Sistema de Aprovação de Despesas ===\n");

// Carregar configurações
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var approvalLimits = new ApprovalLimitsSettings();
configuration.GetSection("ApprovalLimits").Bind(approvalLimits);

var approvalChain = BuildApprovalChain(approvalLimits);

RunScenario("Despesa pequena (Supervisor)", approvalChain,
    new ExpenseRequest("João Silva", 50m, "Material de escritório", "TI"));

RunScenario("Despesa média (Manager)", approvalChain,
    new ExpenseRequest("Maria Santos", 350m, "Curso de capacitação", "RH"));

RunScenario("Despesa alta (Director)", approvalChain,
    new ExpenseRequest("Pedro Oliveira", 2500m, "Notebook", "TI"));

RunScenario("Despesa estratégica (CEO)", approvalChain,
    new ExpenseRequest("Ana Costa", 15000m, "Servidor para Datacenter", "TI"));

Console.WriteLine("\n=== Fim da Simulação ===");

/// <summary>
/// Monta a cadeia de aprovação.
/// </summary>
SupervisorApprover BuildApprovalChain(ApprovalLimitsSettings limits)
{
    var supervisor = new SupervisorApprover(limits.Supervisor);
    var manager = new ManagerApprover(limits.Manager);
    var director = new DirectorApprover(limits.Director);
    var ceo = new CEOApprover();

    supervisor
        .SetNext(manager)
        .SetNext(director)
        .SetNext(ceo);

    return supervisor; // Primeiro da cadeia
}

/// <summary>
/// Executa um cenário específico.
/// </summary>
void RunScenario(
    string scenarioName,
    SupervisorApprover approvalChain,
    ExpenseRequest request)
{
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"CENÁRIO: {scenarioName}");
    Console.WriteLine("--------------------------------------------------");

    Console.WriteLine($"Funcionário : {request.EmployeeName}");
    Console.WriteLine($"Valor       : R$ {request.Amount:N2}");
    Console.WriteLine($"Propósito   : {request.Purpose}");
    Console.WriteLine($"Departamento: {request.Department}\n");

    approvalChain.Approve(request);

    Console.WriteLine();
}