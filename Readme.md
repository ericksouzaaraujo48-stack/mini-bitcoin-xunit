# MiniBitcoin

Aplicação de console em C# que simula uma carteira com saldo em reais (BRL) e em Bitcoin, permitindo consultar a cotação, comprar e vender Bitcoin e exibir o extrato de operações.

Projeto acadêmico da disciplina **Garantia da Qualidade de Software** (Gestão e Qualidade de Software), Anima Educação, Professor Daniel Henrique Matos de Paiva. Foi desenvolvido com **TDD** (testes escritos antes da implementação) e com o padrão **Conventional Commits**.

**Autor:** _(coloque seu nome aqui)_

---

## Funcionalidades

- **Carteira:** gerencia o saldo em BRL e o saldo em Bitcoin (guardado em satoshis), sem permitir saldos negativos.
- **Cotação:** consulta o preço atual de 1 BTC em reais (valor simulado).
- **Compra e venda:** converte o valor pela cotação, valida se há saldo suficiente e atualiza os saldos.
- **Extrato:** lista todas as operações realizadas e calcula o patrimônio total consolidado em BRL.

## Tecnologias

- C# com .NET 10
- xUnit (testes unitários)
- Git, com Conventional Commits

## Arquitetura

O projeto tem duas partes: a aplicação de console e o projeto de testes.

```
MiniBitcoin
├── MiniBitcoin.Console          # Aplicação
│   ├── Carteira.cs              # Saldos em BRL e satoshis
│   ├── Corretora.cs             # Lógica de compra e venda com a cotação
│   ├── Extrato.cs               # Histórico e cálculo do patrimônio
│   ├── Operacao.cs              # Registro de uma compra ou venda
│   └── Program.cs               # Menu interativo
├── MiniBitcoin.Tests            # Testes unitários (xUnit)
│   ├── CarteiraTestes.cs
│   ├── CorretoraTestes.cs
│   ├── ExtratoTestes.cs
│   └── OperacaoTestes.cs
└── MiniBitcoin.slnx
```

### Responsabilidade de cada classe

| Classe | Responsabilidade |
|---|---|
| `Carteira` | Guarda `SaldoBrl` e `SaldoSatoshis`, e valida saques e depósitos. |
| `Corretora` | Recebe uma `Carteira` e a cotação, e executa `ComprarBitcoin` e `VenderBitcoin`. |
| `Extrato` | Registra cada operação e calcula o patrimônio total com `CalcularPatrimonioTotal`. |
| `Operacao` | Guarda tipo (compra ou venda), quantidade em satoshis, valor em BRL e data e hora. |
| `Program` | Interface de console, que lê a entrada do usuário e chama as demais classes. |

### Fluxo de uma compra

1. O usuário informa a quantidade de BTC no menu.
2. O `Program` converte a quantidade para satoshis e chama `Corretora.ComprarBitcoin`.
3. A `Corretora` calcula o custo em BRL pela cotação.
4. A `Carteira` debita o BRL (e lança exceção se o saldo for insuficiente) e credita os satoshis.
5. O `Extrato` registra a operação, mas só se ela tiver sido concluída com sucesso.

## Regras de negócio

- Os saldos iniciais não podem ser negativos.
- Depósitos e saques devem ter valor maior que zero.
- Não é possível sacar mais do que o saldo disponível.
- Uma compra exige saldo suficiente em BRL, e uma venda exige saldo suficiente em Bitcoin.
- Se uma operação falhar, nenhum saldo é alterado e nada é registrado no extrato.
- A cotação deve ser maior que zero.
- **1 BTC = 100.000.000 satoshis.** O Bitcoin é guardado em satoshis (`long`) para evitar erros de arredondamento.
- Não há cobrança de taxas, então comprar ou vender não altera o patrimônio total.

## Como executar

### Pré-requisito

- [.NET SDK 10](https://dotnet.microsoft.com/download) instalado. Confira com `dotnet --version`.

### Rodar a aplicação

Na pasta raiz do projeto:

```bash
dotnet run --project MiniBitcoin.Console/MiniBitcoin.Console.csproj
```

A carteira começa com **R$ 10.000,00** e nenhum Bitcoin, e a cotação inicial é de **R$ 300.000,00 por BTC**. Os dois valores podem ser alterados nas primeiras linhas do `Program.cs`.

### Menu

```
===== MINI BITCOIN =====
1 - Consultar cotação
2 - Ver saldos
3 - Comprar Bitcoin
4 - Vender Bitcoin
5 - Exibir extrato
0 - Sair
```

Nas compras e vendas, informe a quantidade em **BTC**, usando vírgula como separador decimal. Por exemplo, `0,01` equivale a 1.000.000 satoshis.

## Testes

Para executar todos os testes unitários:

```bash
dotnet test
```

Os testes cobrem:

- **Carteira:** saldos iniciais, saldos negativos, saques válidos e saques inválidos.
- **Corretora:** compras e vendas com sucesso e com saldo insuficiente, quantidades inválidas e cotação inválida.
- **Extrato:** registro das operações, ordem do histórico, cálculo do patrimônio total e fluxo completo com a corretora.
- **Operação:** dados guardados pelo registro e data e hora gravadas.

## Histórico de desenvolvimento

O desenvolvimento seguiu o ciclo do TDD (primeiro o teste que falha, depois a implementação), com um commit para cada fase, no padrão Conventional Commits (`feat`, `fix`, `test`, `refactor`, `docs`, `chore`).

| Etapa | Conteúdo |
|---|---|
| 1 | Entidade `Carteira` e regras de saldo |
| 2 | Lógica de compra e venda com validação de saldo e cotação |
| 3 | Extrato de operações e cálculo do patrimônio |
| 4 | Menu interativo no `Program.cs` |
| 5 | Refatoração e documentação |