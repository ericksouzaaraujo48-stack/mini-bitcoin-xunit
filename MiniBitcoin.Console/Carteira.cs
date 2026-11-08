using System;

public class Carteira
{
    public decimal SaldoBrl { get; private set; }
    public long SaldoSatoshis { get; private set; }

    public Carteira(decimal saldoBrl, long saldoSatoshis)
    {
        if (saldoBrl < 0 || saldoSatoshis < 0)
            throw new ArgumentException("Os saldos iniciais não podem ser negativos.");

        SaldoBrl = saldoBrl;
        SaldoSatoshis = saldoSatoshis;
    }

    public void SacarBrl(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do saque deve ser maior que zero.");
        if (valor > SaldoBrl)
            throw new InvalidOperationException("Saldo em BRL insuficiente.");

        SaldoBrl -= valor;
    }

    public void SacarSatoshis(long valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do saque deve ser maior que zero.");
        if (valor > SaldoSatoshis)
            throw new InvalidOperationException("Saldo em Satoshis insuficiente.");

        SaldoSatoshis -= valor;
    }

    // NOVOS na Etapa 2: necessários para creditar o resultado de compras e vendas
    public void DepositarBrl(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do depósito deve ser maior que zero.");

        SaldoBrl += valor;
    }

    public void DepositarSatoshis(long valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do depósito deve ser maior que zero.");

        SaldoSatoshis += valor;
    }
}