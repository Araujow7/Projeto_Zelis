# Zelis

WMS enxuto para pequenas empresas de varejo (farmácias, mercearias, lojas), com PDV separado.
A especificação completa está em [docs/especificacao.md](docs/especificacao.md).

**Versão atual:** v0.1 — fase 0 (estrutura, banco inicial e testes).

## Estrutura

```
Projeto_Zelis/
├── assets/marca/       logotipos e ícone .ico
├── docs/               especificação e regras de negócio
├── scripts/            fase0.ps1: compila e testa no Windows
├── src/
│   ├── Zelis.Api/      servidor (API) — única porta de entrada do banco
│   ├── Zelis.Dominio/  entidades e regras de negócio
│   ├── Zelis.Dados/    banco SQLite, tabelas, migrações
│   ├── Zelis.Desktop/  WMS para Windows (.exe)
│   ├── Zelis.PDV/      PDV separado (.exe)
│   └── Compartilhado/  telas comuns ao WMS e ao PDV
└── tests/Zelis.Testes  testes automatizados
```

## Como rodar (Windows)

Requisitos: [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) e, se quiser, o Visual Studio Community.

```powershell
.\scripts\fase0.ps1          # restaura, gera a migração inicial, compila e testa
dotnet run --project src/Zelis.Api      # sobe a API em http://localhost:5080
dotnet run --project src/Zelis.Desktop  # abre o WMS e testa a conexão
```

A API cria o banco em `dados/zelis.db`, ao lado do executável, na primeira vez que sobe.

## Banco inicial

23 tabelas em snake_case, como na especificação. Toda tabela de operação tem
`empresa_id`, `criado_em` e `criado_por`. Os enums são gravados como texto
(ex.: `Picking`), e nada é apagado em cascata.

## Commits

Um commit por funcionalidade, em português, citando a regra de negócio quando houver
(ex.: "RN11"). Cada fase termina com uma tag (v0.1, v0.2...).
