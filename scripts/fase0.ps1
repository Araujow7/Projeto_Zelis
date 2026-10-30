# Zelis — prepara e testa a fase 0 no Windows.
# Uso (PowerShell, na pasta do projeto):  .\scripts\fase0.ps1
# Requer o .NET 10 SDK: https://dotnet.microsoft.com/download/dotnet/10.0

$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)

function Etapa($texto) { Write-Host "`n== $texto ==" -ForegroundColor Cyan }
function Executar { & $args[0] $args[1..($args.Length - 1)]; if ($LASTEXITCODE -ne 0) { throw "Falhou: $args" } }

Etapa "Versão do .NET"
Executar dotnet --version

Etapa "Ferramentas do projeto (dotnet-ef)"
Executar dotnet tool restore

Etapa "Pacotes"
Executar dotnet restore Zelis.slnx

if (-not (Test-Path "src/Zelis.Dados/Migracoes")) {
    Etapa "Gerando a migração inicial do banco"
    Executar dotnet ef migrations add Inicial `
        --project src/Zelis.Dados --startup-project src/Zelis.Dados --output-dir Migracoes
}

Etapa "Compilando a solução"
Executar dotnet build Zelis.slnx -c Release --no-restore

Etapa "Rodando os testes"
Executar dotnet test Zelis.slnx -c Release --no-build

Write-Host "`nFase 0 OK: solução compilada e testes aprovados." -ForegroundColor Green
