# Script de Execução de Testes

# Executar todos os testes
Write-Host "=== Executando Todos os Testes ===" -ForegroundColor Cyan
dotnet test

Write-Host "`n"

# Executar com verbosidade normal
Write-Host "=== Executando com Verbosidade Normal ===" -ForegroundColor Cyan
dotnet test --verbosity normal

Write-Host "`n"

# Executar com logger detalhado
Write-Host "=== Executando com Logger Detalhado ===" -ForegroundColor Cyan
dotnet test --logger "console;verbosity=detailed"

Write-Host "`n"

# Executar testes de uma classe específica
Write-Host "=== Executando Testes do SupervisorApprover ===" -ForegroundColor Cyan
dotnet test --filter "FullyQualifiedName~SupervisorApproverTests"

Write-Host "`n"

# Executar com cobertura de código
Write-Host "=== Executando com Cobertura de Código ===" -ForegroundColor Cyan
dotnet test --collect:"XPlat Code Coverage"

Write-Host "`n=== Testes Concluídos ===" -ForegroundColor Green

