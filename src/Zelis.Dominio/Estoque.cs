namespace Zelis.Dominio;

public class Recebimento : EntidadeDaEmpresa
{
    public int FornecedorId { get; set; }
    public Fornecedor? Fornecedor { get; set; }

    public string Documento { get; set; } = "";
    public DateOnly Data { get; set; }
    public StatusRecebimento Status { get; set; } = StatusRecebimento.Aberto;

    public List<RecebimentoItem> Itens { get; set; } = [];
}

public class RecebimentoItem : Entidade
{
    public int RecebimentoId { get; set; }
    public Recebimento? Recebimento { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int? EmbalagemId { get; set; }
    public ItemEmbalagem? Embalagem { get; set; }

    public decimal Quantidade { get; set; }
    public DateOnly? Validade { get; set; }
    public string? Lote { get; set; }

    public int? LocalizacaoDestinoId { get; set; }
    public Localizacao? LocalizacaoDestino { get; set; }

    public Divergencia Divergencia { get; set; } = Divergencia.Nenhuma;
}

/// <summary>RN03: saldo por item, localização e validade. O lote fica só na movimentação.</summary>
public class Saldo : EntidadeDaEmpresa
{
    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int LocalizacaoId { get; set; }
    public Localizacao? Localizacao { get; set; }

    public DateOnly? Validade { get; set; }
    public SituacaoSaldo Situacao { get; set; } = SituacaoSaldo.Disponivel;
    public decimal Quantidade { get; set; }
}

/// <summary>
/// RN11: ficha de movimentações (Kardex). Nada é apagado: correções são estornos
/// que apontam para a movimentação original em EstornoDeId.
/// </summary>
public class Movimentacao : EntidadeDaEmpresa
{
    public TipoMovimentacao Tipo { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int? OrigemId { get; set; }
    public Localizacao? Origem { get; set; }

    public int? DestinoId { get; set; }
    public Localizacao? Destino { get; set; }

    public DateOnly? Validade { get; set; }
    public string? Lote { get; set; }
    public decimal Quantidade { get; set; }
    public MotivoPerda? Motivo { get; set; }
    public string? Documento { get; set; }

    public int OperadorId { get; set; }
    public Operador? Operador { get; set; }

    public DateTime DataHora { get; set; }

    public int? EstornoDeId { get; set; }
    public Movimentacao? EstornoDe { get; set; }
}

public class Inventario : EntidadeDaEmpresa
{
    public TipoInventario Tipo { get; set; }
    public StatusInventario Status { get; set; } = StatusInventario.Aberto;
    public string? ArquivoImportado { get; set; }

    /// <summary>RN09: ajuste só entra no estoque depois da aprovação.</summary>
    public int? AprovadoPor { get; set; }
    public DateTime? AprovadoEm { get; set; }

    public List<InventarioItem> Itens { get; set; } = [];
}

public class InventarioItem : Entidade
{
    public int InventarioId { get; set; }
    public Inventario? Inventario { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int LocalizacaoId { get; set; }
    public Localizacao? Localizacao { get; set; }

    public DateOnly? Validade { get; set; }
    public decimal QtdSistema { get; set; }
    public decimal QtdContada { get; set; }
    public decimal Diferenca { get; set; }

    /// <summary>Diferença × custo.</summary>
    public decimal Valor { get; set; }
}

public class Pendencia : EntidadeDaEmpresa
{
    public TipoPendencia Tipo { get; set; }

    /// <summary>Identifica o que gerou a pendência (ex.: "item:1001", "recebimento:42").</summary>
    public string Referencia { get; set; } = "";
    public DateOnly? Prazo { get; set; }
    public StatusPendencia Status { get; set; } = StatusPendencia.Aberta;
    public int? ResolvidoPor { get; set; }
    public DateTime? ResolvidoEm { get; set; }
}
