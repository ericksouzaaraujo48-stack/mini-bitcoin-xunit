using System.Globalization;

const long SatoshisPorBitcoin = 100000000;

var cultura = new CultureInfo("pt-BR");
var carteira = new Carteira(10000m, 0);      // começa com R$ 10.000 e nenhum Bitcoin
var corretora = new Corretora(carteira, 300000m); // 1 BTC = R$ 300.000

bool executando = true;

while (executando)
{
    ExibirMenu();
    string opcao = Console.ReadLine() ?? "";
    Console.WriteLine();

    switch (opcao)
    {
        case "1": ConsultarCotacao(); break;
        case "2": VerSaldos(); break;
        case "3": ComprarBitcoin(); break;
        case "4": VenderBitcoin(); break;
        case "5": ExibirExtrato(); break;
        case "0": executando = false; break;
        default: Console.WriteLine("Opção inválida."); break;
    }

    Console.WriteLine();
}

Console.WriteLine("Até logo!");

// ---------- Funções ----------

void ExibirMenu()
{
    Console.WriteLine("===== MINI BITCOIN =====");
    Console.WriteLine("1 - Consultar cotação");
    Console.WriteLine("2 - Ver saldos");
    Console.WriteLine("3 - Comprar Bitcoin");
    Console.WriteLine("4 - Vender Bitcoin");
    Console.WriteLine("5 - Exibir extrato");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");
}

void ConsultarCotacao()
{
    Console.WriteLine($"Cotação atual: 1 BTC = {corretora.CotacaoBrl.ToString("C", cultura)}");
}

void VerSaldos()
{
    Console.WriteLine($"Saldo em BRL: {carteira.SaldoBrl.ToString("C", cultura)}");
    Console.WriteLine($"Saldo em Bitcoin: {FormatarBtc(carteira.SaldoSatoshis)} ({carteira.SaldoSatoshis} satoshis)");
}

void ComprarBitcoin()
{
    long? satoshis = LerQuantidadeEmSatoshis("Quantos BTC deseja comprar? ");
    if (satoshis == null) return;

    try
    {
        corretora.ComprarBitcoin(satoshis.Value);
        Console.WriteLine("Compra realizada com sucesso!");
    }
    catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

void VenderBitcoin()
{
    long? satoshis = LerQuantidadeEmSatoshis("Quantos BTC deseja vender? ");
    if (satoshis == null) return;

    try
    {
        corretora.VenderBitcoin(satoshis.Value);
        Console.WriteLine("Venda realizada com sucesso!");
    }
    catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

void ExibirExtrato()
{
    Console.WriteLine("===== EXTRATO =====");

    if (corretora.Extrato.Operacoes.Count == 0)
    {
        Console.WriteLine("Nenhuma operação realizada.");
    }

    foreach (var operacao in corretora.Extrato.Operacoes)
    {
        Console.WriteLine($"{operacao.DataHora:dd/MM/yyyy HH:mm:ss} | {operacao.Tipo,-6} | " +
                          $"{FormatarBtc(operacao.Satoshis)} | {operacao.ValorBrl.ToString("C", cultura)}");
    }

    decimal patrimonio = corretora.Extrato.CalcularPatrimonioTotal(carteira, corretora.CotacaoBrl);
    Console.WriteLine($"Patrimônio total: {patrimonio.ToString("C", cultura)}");
}

// Lê um valor em BTC (ex.: 0,01) e converte para satoshis
long? LerQuantidadeEmSatoshis(string mensagem)
{
    Console.Write(mensagem);
    string entrada = Console.ReadLine() ?? "";

    if (!decimal.TryParse(entrada, NumberStyles.Number, cultura, out decimal btc))
    {
        Console.WriteLine("Valor inválido. Digite um número, por exemplo: 0,01");
        return null;
    }

    return (long)Math.Round(btc * SatoshisPorBitcoin);
}

string FormatarBtc(long satoshis)
{
    return (satoshis / (decimal)SatoshisPorBitcoin).ToString("0.00000000", cultura) + " BTC";
}