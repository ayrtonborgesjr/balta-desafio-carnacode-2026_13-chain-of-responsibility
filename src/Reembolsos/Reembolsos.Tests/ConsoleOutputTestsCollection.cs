namespace Reembolsos.Tests;

/// <summary>
/// Desabilita a execução paralela de testes que manipulam Console.Out
/// </summary>
[CollectionDefinition("Console Output Tests", DisableParallelization = true)]
public class ConsoleOutputTestsCollection
{
}

