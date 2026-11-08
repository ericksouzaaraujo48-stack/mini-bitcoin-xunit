using System;

public class Corretora
{
    private const long SatoshisPorBitcoin = 100000000;

    private readonly Carteira _carteira;
    public decimal CotacaoBrl { get; private set; } // preço de 1 BTC em reais
    public Extrato Extrato { get; } = new Extrato(); // NOVO na Etapa 3

    public Corretora(Carteira carteira, decimal cotacaoBrl)
    {
        if (cotacaoBrl <= 0)
            throw new ArgumentException("A cotação deve ser maior que zero.");

        _carteira = carteira;
        CotacaoBrl = cotacaoBrl;
    }

    public void ComprarBitcoin(long satoshis)
    {
        if (satoshis <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        decimal custo = CalcularValorBrl(satoshis);

        _carteira.SacarBrl(custo); // lança exceção se o saldo em BRL for insuficiente
        _carteira.DepositarSatoshis(satoshis);
        Extrato.Registrar(TipoOperacao.Compra, satoshis, custo); // NOVO na Etapa 3
    }

    public void VenderBitcoin(long satoshis)
    {
        if (satoshis <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        decimal valor = CalcularValorBrl(satoshis);

        _carteira.SacarSatoshis(satoshis); // lança exceção se o saldo em BTC for insuficiente
        _carteira.DepositarBrl(valor);
        Extrato.Registrar(TipoOperacao.Venda, satoshis, valor); // NOVO na Etapa 3
    }

    private decimal CalcularValorBrl(long satoshis)
    {
        return Math.Round(satoshis * CotacaoBrl / SatoshisPorBitcoin, 2);
    }
}