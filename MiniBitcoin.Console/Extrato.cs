using System;
using System.Collections.Generic;

public class Extrato
{
    private const long SatoshisPorBitcoin = 100000000;

    private readonly List<Operacao> _operacoes = new();

    public IReadOnlyList<Operacao> Operacoes => _operacoes;

    public void Registrar(TipoOperacao tipo, long satoshis, decimal valorBrl)
    {
        if (satoshis <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        _operacoes.Add(new Operacao(tipo, satoshis, valorBrl, DateTime.Now));
    }

    public decimal CalcularPatrimonioTotal(Carteira carteira, decimal cotacaoBrl)
    {
        decimal bitcoinEmBrl = carteira.SaldoSatoshis * cotacaoBrl / SatoshisPorBitcoin;

        return Math.Round(carteira.SaldoBrl + bitcoinEmBrl, 2);
    }
}