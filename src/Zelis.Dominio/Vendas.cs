namespace Zelis.Dominio;

/// <summary>Promoção por percentual (RN07 e RN08).</summary>
public class Promocao : EntidadeDaEmpresa
{
    public AlvoPromocao AlvoTipo { get; set; }

    /// <summary>Id do item, da categoria ou da coleção, conforme AlvoTipo.</summary>
    public int AlvoId { get; set; }

    public decimal Percentual { get; set; }
    public bool Ativa { get; set; } = true;
    public DateOnly Inicio { get; set; }
    public DateOnly Fim { get; set; }
}

/// <summary>Venda recebida do PDV. Caixa + NumeroPdv identificam a venda na sincronização offline.</summary>
public class Venda : EntidadeDaEmpresa
{
    public string Caixa { get; set; } = "";
    public long NumeroPdv { get; set; }

    public int OperadorId { get; set; }
    public Operador? Operador { get; set; }

    public DateTime DataHora { get; set; }
    public decimal Total { get; set; }
    public string FormaPagamento { get; set; } = "";

    public List<VendaItem> Itens { get; set; } = [];
}

public class VendaItem : Entidade
{
    public int VendaId { get; set; }
    public Venda? Venda { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public decimal Quantidade { get; set; }
    public decimal Preco { get; set; }
    public decimal Desconto { get; set; }
}
