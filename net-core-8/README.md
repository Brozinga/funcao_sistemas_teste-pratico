# Função Sistemas - Teste prático - .NET 8

Este projeto é uma prova de conceito que simula a modernização de uma aplicação legada em .NET Framework para .NET 8. A migração mantém a integridade da base de dados original, promovendo uma reformulação completa da arquitetura estrutural e da interface.

## O que mudou em relação ao projeto original

| Original | Agora |
| --- | --- |
| .NET Framework + MVC 5 | .NET 8 + ASP.NET Core MVC |
| Bootstrap 3 | Bootstrap 5.3 |
| jQuery, jQuery Validate, jQuery Mask, jTable | JavaScript puro (módulos ES), [IMask](https://imask.js.org) para as máscaras |
| ADO.NET manual | Dapper chamando as mesmas stored procedures |

## Estrutura

```
net-core-8/
├── FI.AtividadeEntrevista.sln
├── src/
│   ├── FI.AtividadeEntrevista.Domain          Entidades, validação de CPF e contratos dos repositórios
│   ├── FI.AtividadeEntrevista.Application     Regras de negócio (ClienteServico)
│   ├── FI.AtividadeEntrevista.Infrastructure  Acesso ao banco (Dapper) e scripts SQL (pasta Database)
│   └── FI.AtividadeEntrevista.Web             Telas MVC, controllers e arquivos estáticos
└── tests/
    └── FI.AtividadeEntrevista.Tests           Testes unitários das regras de negócio
```

As dependências seguem a arquitetura limpa: `Web` e `Infrastructure` dependem de `Application`, que depende de `Domain`. O `Domain` não depende de ninguém.

## Como rodar os testes

```bash
dotnet test
```

## Como executar

### Windows

No Windows o banco é acessado pelo LocalDB, que já vem junto com o Visual Studio. Não é preciso Docker.

**O que precisa estar instalado**

- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou superior, com a carga de trabalho *Desenvolvimento para ASP.NET e Web* e o componente *Armazenamento e processamento de dados* (inclui o LocalDB)
- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)

Abra `FI.AtividadeEntrevista.sln` no Visual Studio, defina o projeto `FI.AtividadeEntrevista.Web` como projeto de inicialização e pressione **F5**.

   Se preferir pelo terminal, na raiz do projeto:

   ```powershell
   dotnet run --project src/FI.AtividadeEntrevista.Web --urls http://localhost:5080
   ```

   Depois acesse `http://localhost:5080`.

### macOS e Linux

Aqui o LocalDB não existe, então o banco roda em um SQL Server dentro do Docker (veja [Banco de dados](#banco-de-dados-linux-e-macos)).

**O que precisa estar instalado**

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://docs.docker.com/get-docker/) com Docker Compose

**Passo a passo**

```bash
docker compose up -d
dotnet run --project src/FI.AtividadeEntrevista.Web --urls http://localhost:5080
```

Depois acesse `http://localhost:5080`.


