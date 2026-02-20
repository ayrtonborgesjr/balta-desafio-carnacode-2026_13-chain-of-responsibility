# Sistema de Aprovação de Despesas

Este projeto implementa o padrão **Chain of Responsibility** para aprovação de despesas em diferentes níveis hierárquicos.

## Configuração de Limites de Aprovação

Os limites de aprovação foram externalizados para o arquivo `appsettings.json`, permitindo fácil configuração sem necessidade de recompilar o código.

### Arquivo de Configuração

O arquivo `appsettings.json` contém os limites de aprovação para cada nível:

```json
{
  "ApprovalLimits": {
    "Supervisor": 100.00,
    "Manager": 500.00,
    "Director": 5000.00
  }
}
```

### Níveis de Aprovação

- **Supervisor**: Aprova despesas até o limite configurado (padrão: R$ 100,00)
- **Gerente (Manager)**: Aprova despesas até o limite configurado (padrão: R$ 500,00)
- **Diretor (Director)**: Aprova despesas até o limite configurado (padrão: R$ 5.000,00)
- **CEO**: Aprova despesas de qualquer valor (sem limite)

### Como Alterar os Limites

1. Edite o arquivo `appsettings.json` na pasta do projeto
2. Altere os valores conforme necessário
3. Execute a aplicação novamente

**Nota**: Não é necessário recompilar o projeto após alterar os limites!

## Estrutura do Projeto

```
Reembolsos.Console/
├── Domain/
│   ├── Approvers/
│   │   ├── SupervisorApprover.cs
│   │   ├── ManagerApprover.cs
│   │   ├── DirectorApprover.cs
│   │   ├── CEOApprover.cs
│   │   └── ExpenseApproverBase.cs
│   ├── Configuration/
│   │   └── ApprovalLimitsSettings.cs
│   ├── Entities/
│   │   └── ExpenseRequest.cs
│   └── Interfaces/
│       └── IExpenseApprover.cs
├── appsettings.json
└── Program.cs
```

## Pacotes Utilizados

- `Microsoft.Extensions.Configuration` - Carregamento de configurações
- `Microsoft.Extensions.Configuration.Json` - Suporte para arquivos JSON
- `Microsoft.Extensions.Configuration.Binder` - Binding de configurações para objetos
- `Microsoft.Extensions.Configuration.FileExtensions` - Extensões para arquivos de configuração

## Executar o Projeto

```bash
dotnet run
```

