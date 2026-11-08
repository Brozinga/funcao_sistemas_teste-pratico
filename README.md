# Teste Prático - Função Sistemas

Neste repositório entrego o que foi solicitado no teste, no projeto original em **.NET Framework (MVC 5)**. Como adicional, também fiz uma releitura do projeto em **.NET 8**.

## O que foi solicitado e entregue

### 1. Campo CPF no cadastro e alteração de clientes
- Incluí o campo **CPF** na tela, seguindo o mesmo padrão visual dos demais campos.
- O campo é obrigatório e possui a máscara `999.999.999-99`.
- Valido o CPF pelo cálculo dos dígitos verificadores.
- Não permito CPF duplicado: a validação é feita no sistema e também no banco, com um índice único na coluna `CPF` da tabela `CLIENTES`.

### 2. Beneficiários
- Incluí o botão **Beneficiários** na tela de cliente, no mesmo padrão visual dos demais botões.
- O botão abre um pop-up com os campos **CPF** (com máscara e validação do dígito verificador) e **Nome**.
- No pop-up há um grid com os beneficiários já incluídos, onde é possível **alterar** e **excluir**.
- Não permito dois beneficiários com o mesmo CPF para o mesmo cliente.
- Os beneficiários só são gravados no banco quando clico em **Salvar** na tela de cliente.
- Banco de dados: criei a tabela `BENEFICIARIOS` com os campos `ID`, `CPF`, `NOME` e `IDCLIENTE`, além das procedures de inclusão, alteração, consulta e exclusão.
- Os scripts SQL estão em `full-framework/SQLs`.

## Como foi desenvolvido

Sou usuário Linux, então fiz o desenvolvimento em uma **máquina virtual**. A implementação no **Full Framework** foi feita **de forma manual**.

## Adicional: releitura em .NET 8

Além do que foi pedido, fiz uma releitura do projeto em **.NET 8**, na pasta `net-core-8`. Atualizei os pacotes e a arquitetura, mantendo as mesmas funcionalidades e o mesmo banco de dados. Essa parte foi feita **com auxílio de automação**.

> **Observação:** Optei pelo .NET 8 por ser compatível com o Visual Studio 2022, solicitado no teste. Dessa forma, basta o instalador do Visual Studio 2022, sem precisar instalar nenhum SDK separadamente.

Resumo das mudanças:
- ASP.NET Core MVC no lugar do MVC 5.
- Bootstrap 5 e JavaScript puro no lugar de Bootstrap 3 e jQuery.
- Dapper chamando as mesmas stored procedures.
- Projeto dividido em camadas (Domain, Application, Infrastructure e Web) seguindo o padrão de arquitetura limpa e com testes unitários.

As instruções para executar essa versão estão em [`net-core-8/README.md`](net-core-8/README.md).

## Estrutura

```
├── full-framework/   Projeto original (.NET Framework) com as funcionalidades pedidas
└── net-core-8/       Releitura em .NET 8 (adicional)
```
