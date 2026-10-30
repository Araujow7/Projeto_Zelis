namespace Zelis.Dominio;

public enum TipoLocalizacao
{
    Picking,
    Reposicao
}

/// <summary>RN05: só o saldo Disponivel sai no PDV.</summary>
public enum SituacaoSaldo
{
    Disponivel,
    Bloqueado,
    Avariado,
    Vencido
}

public enum TipoMovimentacao
{
    Entrada,
    Venda,
    Transferencia,
    Perda,
    AjusteInventario,
    Estorno
}

/// <summary>RN10: toda perda ou avaria exige um motivo.</summary>
public enum MotivoPerda
{
    Quebra,
    Vencimento,
    Furto,
    ConsumoInterno
}

public enum StatusRecebimento
{
    Aberto,
    Finalizado,
    Cancelado
}

public enum Divergencia
{
    Nenhuma,
    QuantidadeAMais,
    QuantidadeAMenos,
    ItemNaoEsperado
}

public enum AlvoPromocao
{
    Item,
    Categoria,
    Colecao
}

public enum TipoInventario
{
    Geral,
    Rotativo
}

public enum StatusInventario
{
    Aberto,
    Importado,
    Aprovado,
    Cancelado
}

public enum TipoPendencia
{
    RecebimentoAberto,
    Reposicao,
    ItemVencendo,
    ItemVencido,
    AbaixoDoMinimo,
    VendaNegativa
}

public enum StatusPendencia
{
    Aberta,
    Resolvida
}
