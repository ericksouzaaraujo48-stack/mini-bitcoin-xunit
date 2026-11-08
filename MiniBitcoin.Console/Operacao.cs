using System;

public enum TipoOperacao
{
    Compra,
    Venda
}

public record Operacao(TipoOperacao Tipo, long Satoshis, decimal ValorBrl, DateTime DataHora);