# Testes Unitários - Sistema de Aprovação de Despesas

Este projeto contém testes unitários completos para o sistema de aprovação de despesas utilizando o padrão **Chain of Responsibility**.

## Cobertura de Testes

### 📊 Estatísticas
- **Total de Testes**: 47
- **Taxa de Sucesso**: 100%
- **Framework**: xUnit

## Estrutura dos Testes

### 1. Testes de Entidades

#### `ExpenseRequestTests` (3 testes)
- ✅ `Constructor_ShouldInitializeAllProperties` - Verifica inicialização correta
- ✅ `Constructor_ShouldAcceptDifferentValues` - Testa diferentes valores (Theory)
- ✅ `Properties_ShouldBeImmutable` - Valida imutabilidade das propriedades

### 2. Testes de Aprovadores

#### `SupervisorApproverTests` (6 testes)
- ✅ `Constructor_ShouldSetApprovalLimit` - Verifica construtor
- ✅ `Approve_ShouldApproveWhenAmountIsWithinLimit` - Testa aprovação dentro do limite
- ✅ `Approve_ShouldForwardWhenAmountExceedsLimit` - Testa encaminhamento
- ✅ `Approve_ShouldThrowExceptionWhenRequestIsNull` - Valida null handling
- ✅ `SetNext_ShouldReturnNextApprover` - Testa configuração da cadeia
- ✅ `SetNext_ShouldAllowFluentChaining` - Valida API fluente

#### `ManagerApproverTests` (5 testes)
- ✅ `Constructor_ShouldSetApprovalLimit`
- ✅ `Approve_ShouldApproveWhenAmountIsWithinLimit`
- ✅ `Approve_ShouldForwardWhenAmountExceedsLimit`
- ✅ `Approve_ShouldThrowExceptionWhenRequestIsNull`
- ✅ `Approve_ShouldPerformAdditionalChecks` - Verifica validações extras

#### `DirectorApproverTests` (5 testes)
- ✅ `Constructor_ShouldSetApprovalLimit`
- ✅ `Approve_ShouldApproveWhenAmountIsWithinLimit`
- ✅ `Approve_ShouldForwardWhenAmountExceedsLimit`
- ✅ `Approve_ShouldThrowExceptionWhenRequestIsNull`
- ✅ `Approve_ShouldPerformStrategicAlignmentCheck` - Verifica validações estratégicas

#### `CEOApproverTests` (4 testes)
- ✅ `Constructor_ShouldCreateInstance`
- ✅ `Approve_ShouldApproveAnyAmount` - Testa aprovação sem limite
- ✅ `Approve_ShouldThrowExceptionWhenRequestIsNull`
- ✅ `Approve_ShouldPerformAllChecks` - Verifica todas as validações
- ✅ `Approve_ShouldNotForwardToNext` - Confirma que é o último na cadeia

### 3. Testes da Cadeia de Aprovação

#### `ApprovalChainTests` (7 testes)
- ✅ `ApprovalChain_ShouldRouteToCorrectApprover_ForSmallExpense`
- ✅ `ApprovalChain_ShouldRouteToCorrectApprover_ForMediumExpense`
- ✅ `ApprovalChain_ShouldRouteToCorrectApprover_ForLargeExpense`
- ✅ `ApprovalChain_ShouldRouteToCorrectApprover_ForStrategicExpense`
- ✅ `ApprovalChain_ShouldPassThroughAllLevels_ForVeryLargeExpense`
- ✅ `ApprovalChain_ShouldHandleEdgeCases_AtExactLimit`
- ✅ `ApprovalChain_ShouldReportWhenNoApproverAvailable`

### 4. Testes de Configuração

#### `ApprovalLimitsSettingsTests` (3 testes)
- ✅ `Constructor_ShouldInitializeWithDefaultValues`
- ✅ `Properties_ShouldBeSettable`
- ✅ `Properties_ShouldAcceptDifferentValues`

## Execução dos Testes

### Executar Todos os Testes
```bash
dotnet test
```

### Executar com Detalhes
```bash
dotnet test --verbosity normal
```

### Executar com Cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Detalhes Técnicos

### Manipulação de Console.Out
Os testes que verificam a saída do console utilizam redirecionamento de `System.Console.Out` para capturar e validar as mensagens de log. Para evitar problemas de concorrência, todos os testes que manipulam o console foram agrupados em uma collection especial que desabilita a execução paralela:

```csharp
[Collection("Console Output Tests")]
```

### Padrão de Testes
Todos os testes seguem o padrão **AAA** (Arrange-Act-Assert):
- **Arrange**: Preparação do cenário de teste
- **Act**: Execução da ação sendo testada
- **Assert**: Verificação dos resultados

### Data-Driven Tests
Utilizamos `[Theory]` e `[InlineData]` para testes parametrizados, permitindo validar múltiplos cenários com o mesmo código de teste.

## Cenários Testados

### ✅ Casos de Sucesso
- Aprovação dentro dos limites de cada nível
- Encaminhamento correto para níveis superiores
- Configuração de cadeia fluente
- CEO aprovando qualquer valor

### ✅ Casos de Erro
- Requisições nulas (ArgumentNullException)
- Ausência de aprovador na cadeia
- Valores acima de todos os limites

### ✅ Casos Limite
- Valores exatamente no limite
- Despesas muito grandes
- Validações específicas de cada nível

## Manutenção

Ao adicionar novos aprovadores ou modificar a lógica:

1. Crie testes para o novo aprovador em `Domain/Approvers/`
2. Adicione o atributo `[Collection("Console Output Tests")]` se usar console
3. Siga o padrão AAA
4. Teste casos de sucesso, erro e limites
5. Execute `dotnet test` para validar

## Referências

- Framework de Testes: [xUnit](https://xunit.net/)
- Padrão de Design: [Chain of Responsibility](https://refactoring.guru/design-patterns/chain-of-responsibility)

