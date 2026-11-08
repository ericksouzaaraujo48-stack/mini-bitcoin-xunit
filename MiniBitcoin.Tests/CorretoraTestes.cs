using System;
using Xunit;

public class CorretoraTestes
{
    // Cotação usada nos testes: 1 BTC = R$ 300.000
    // Logo, 1.000.000 satoshis (0,01 BTC) = R$ 3.000
    private const decimal Cotacao = 300000m;

    [Fact]
    public void CriarCorretora_ComCotacaoZeroOuNegativa_LancaExcecao()
    {
        var carteira = new Carteira(0m, 0);

        Assert.Throws<ArgumentException>(() => new Corretora(carteira, 0m));
        Assert.Throws<ArgumentException>(() => new Corretora(carteira, -1m));
    }

    [Fact]
    public void ComprarBitcoin_ComSaldoSuficiente_AtualizaSaldos()
    {
        var carteira = new Carteira(5000m, 0);
        var corretora = new Corretora(carteira, Cotacao);

        corretora.ComprarBitcoin(1000000);

        Assert.Equal(2000m, carteira.SaldoBrl);
        Assert.Equal(1000000, carteira.SaldoSatoshis);
    }

    [Fact]
    public void ComprarBitcoin_ComSaldoBrlInsuficiente_LancaExcecaoEMantemSaldos()
    {
        var carteira = new Carteira(1000m, 0);
        var corretora = new Corretora(carteira, Cotacao);

        Assert.Throws<InvalidOperationException>(() => corretora.ComprarBitcoin(1000000));

        Assert.Equal(1000m, carteira.SaldoBrl);
        Assert.Equal(0, carteira.SaldoSatoshis);
    }

    [Fact]
    public void ComprarBitcoin_ComQuantidadeZeroOuNegativa_LancaExcecao()
    {
        var carteira = new Carteira(5000m, 0);
        var corretora = new Corretora(carteira, Cotacao);

        Assert.Throws<ArgumentException>(() => corretora.ComprarBitcoin(0));
        Assert.Throws<ArgumentException>(() => corretora.ComprarBitcoin(-5));
    }

    [Fact]
    public void VenderBitcoin_ComSaldoSuficiente_AtualizaSaldos()
    {
        var carteira = new Carteira(0m, 2000000);
        var corretora = new Corretora(carteira, Cotacao);

        corretora.VenderBitcoin(1000000);

        Assert.Equal(3000m, carteira.SaldoBrl);
        Assert.Equal(1000000, carteira.SaldoSatoshis);
    }

    [Fact]
    public void VenderBitcoin_ComSaldoBtcInsuficiente_LancaExcecaoEMantemSaldos()
    {
        var carteira = new Carteira(0m, 500000);
        var corretora = new Corretora(carteira, Cotacao);

        Assert.Throws<InvalidOperationException>(() => corretora.VenderBitcoin(1000000));

        Assert.Equal(0m, carteira.SaldoBrl);
        Assert.Equal(500000, carteira.SaldoSatoshis);
    }

    [Fact]
    public void VenderBitcoin_ComQuantidadeZeroOuNegativa_LancaExcecao()
    {
        var carteira = new Carteira(0m, 1000000);
        var corretora = new Corretora(carteira, Cotacao);

        Assert.Throws<ArgumentException>(() => corretora.VenderBitcoin(0));
        Assert.Throws<ArgumentException>(() => corretora.VenderBitcoin(-5));
    }
}