using System;
using Xunit;

public class CarteiraTestes
{
    [Fact]
    public void CriarCarteira_ComSaldosValidos_DefineSaldosIniciais()
    {
        var carteira = new Carteira(1000m, 50000);

        Assert.Equal(1000m, carteira.SaldoBrl);
        Assert.Equal(50000, carteira.SaldoSatoshis);
    }

    [Fact]
    public void CriarCarteira_ComSaldoBrlNegativo_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Carteira(-1m, 0));
    }

    [Fact]
    public void CriarCarteira_ComSaldoSatoshisNegativo_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Carteira(0m, -1));
    }

    [Fact]
    public void SacarBrl_ComSaldoSuficiente_DiminuiSaldo()
    {
        var carteira = new Carteira(100m, 0);

        carteira.SacarBrl(40m);

        Assert.Equal(60m, carteira.SaldoBrl);
    }

    [Fact]
    public void SacarBrl_MaiorQueSaldo_LancaExcecao()
    {
        var carteira = new Carteira(100m, 0);

        Assert.Throws<InvalidOperationException>(() => carteira.SacarBrl(200m));
    }

    [Fact]
    public void SacarBrl_ComValorZeroOuNegativo_LancaExcecao()
    {
        var carteira = new Carteira(100m, 0);

        Assert.Throws<ArgumentException>(() => carteira.SacarBrl(0m));
        Assert.Throws<ArgumentException>(() => carteira.SacarBrl(-10m));
    }

    [Fact]
    public void SacarSatoshis_ComSaldoSuficiente_DiminuiSaldo()
    {
        var carteira = new Carteira(0m, 1000);

        carteira.SacarSatoshis(400);

        Assert.Equal(600, carteira.SaldoSatoshis);
    }

    [Fact]
    public void SacarSatoshis_MaiorQueSaldo_LancaExcecao()
    {
        var carteira = new Carteira(0m, 1000);

        Assert.Throws<InvalidOperationException>(() => carteira.SacarSatoshis(2000));
    }

    [Fact]
    public void SacarSatoshis_ComValorZeroOuNegativo_LancaExcecao()
    {
        var carteira = new Carteira(0m, 1000);

        Assert.Throws<ArgumentException>(() => carteira.SacarSatoshis(0));
        Assert.Throws<ArgumentException>(() => carteira.SacarSatoshis(-10));
    }
}