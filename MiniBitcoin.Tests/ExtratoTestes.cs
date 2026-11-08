using System;
using Xunit;

public class ExtratoTestes
{
    // Cotação usada nos testes: 1 BTC = R$ 300.000
    // Logo, 1.000.000 satoshis (0,01 BTC) = R$ 3.000
    private const decimal Cotacao = 300000m;

    // ---------- Registro de operações ----------

    [Fact]
    public void NovoExtrato_NaoPossuiOperacoes()
    {
        var extrato = new Extrato();

        Assert.Empty(extrato.Operacoes);
    }

    [Fact]
    public void Registrar_ComDadosValidos_AdicionaOperacaoNaLista()
    {
        var extrato = new Extrato();

        extrato.Registrar(TipoOperacao.Compra, 1000000, 3000m);

        var operacao = Assert.Single(extrato.Operacoes);
        Assert.Equal(TipoOperacao.Compra, operacao.Tipo);
        Assert.Equal(1000000, operacao.Satoshis);
        Assert.Equal(3000m, operacao.ValorBrl);
    }

    [Fact]
    public void Registrar_ComQuantidadeZeroOuNegativa_LancaExcecao()
    {
        var extrato = new Extrato();

        Assert.Throws<ArgumentException>(() => extrato.Registrar(TipoOperacao.Venda, 0, 100m));
        Assert.Throws<ArgumentException>(() => extrato.Registrar(TipoOperacao.Venda, -1, 100m));
    }

    // ---------- Patrimônio total ----------

    [Fact]
    public void CalcularPatrimonioTotal_SomaSaldoBrlEBitcoinConvertido()
    {
        var carteira = new Carteira(1000m, 1000000);
        var extrato = new Extrato();

        decimal patrimonio = extrato.CalcularPatrimonioTotal(carteira, Cotacao);

        Assert.Equal(4000m, patrimonio); // 1000 + 3000
    }

    [Fact]
    public void CalcularPatrimonioTotal_SemBitcoin_RetornaApenasSaldoBrl()
    {
        var carteira = new Carteira(1500m, 0);
        var extrato = new Extrato();

        decimal patrimonio = extrato.CalcularPatrimonioTotal(carteira, Cotacao);

        Assert.Equal(1500m, patrimonio);
    }

    [Fact]
    public void CalcularPatrimonioTotal_ComCotacaoMaior_AumentaPatrimonio()
    {
        var carteira = new Carteira(1000m, 1000000);
        var extrato = new Extrato();

        decimal patrimonio = extrato.CalcularPatrimonioTotal(carteira, 400000m);

        Assert.Equal(5000m, patrimonio); // 1000 + 4000
    }

    // ---------- Fluxo completo com a Corretora ----------

    [Fact]
    public void Corretora_AoComprarEVender_RegistraOperacoesNaOrdem()
    {
        var carteira = new Carteira(5000m, 0);
        var corretora = new Corretora(carteira, Cotacao);

        corretora.ComprarBitcoin(1000000);
        corretora.VenderBitcoin(500000);

        Assert.Equal(2, corretora.Extrato.Operacoes.Count);
        Assert.Equal(TipoOperacao.Compra, corretora.Extrato.Operacoes[0].Tipo);
        Assert.Equal(3000m, corretora.Extrato.Operacoes[0].ValorBrl);
        Assert.Equal(TipoOperacao.Venda, corretora.Extrato.Operacoes[1].Tipo);
        Assert.Equal(1500m, corretora.Extrato.Operacoes[1].ValorBrl);
    }

    [Fact]
    public void Corretora_AoFalharUmaCompra_NaoRegistraOperacao()
    {
        var carteira = new Carteira(100m, 0);
        var corretora = new Corretora(carteira, Cotacao);

        Assert.Throws<InvalidOperationException>(() => corretora.ComprarBitcoin(1000000));

        Assert.Empty(corretora.Extrato.Operacoes);
    }

    [Fact]
    public void Corretora_AoComprar_PatrimonioTotalPermaneceOMesmo()
    {
        var carteira = new Carteira(5000m, 0);
        var corretora = new Corretora(carteira, Cotacao);

        corretora.ComprarBitcoin(1000000);

        decimal patrimonio = corretora.Extrato.CalcularPatrimonioTotal(carteira, Cotacao);
        Assert.Equal(5000m, patrimonio); // 2000 em BRL + 3000 em BTC
    }
}