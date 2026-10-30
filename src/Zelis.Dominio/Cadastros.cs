namespace Zelis.Dominio;

public class Categoria : EntidadeDaEmpresa
{
    public string Nome { get; set; } = "";
}

public class Colecao : EntidadeDaEmpresa
{
    public string Nome { get; set; } = "";
}

/// <summary>RN01: o código interno (CodigoProduto) é a chave de todo o tráfego do produto.</summary>
public class Item : EntidadeDaEmpresa
{
    public string CodigoProduto { get; set; } = "";
    public string Descricao { get; set; } = "";

    /// <summary>EAN do fabricante, usado só na conferência.</summary>
    public string? EanUnidade { get; set; }

    public int? CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public decimal PrecoVenda { get; set; }
    public decimal Custo { get; set; }
    public bool ControlaValidade { get; set; }

    /// <summary>RN14: item inativo não pode ser vendido nem recebido.</summary>
    public bool Ativo { get; set; } = true;

    public List<ItemEmbalagem> Embalagens { get; set; } = [];
    public List<ItemColecao> Colecoes { get; set; } = [];
}

/// <summary>RN02: a embalagem converte para unidades pelo fator Quantidade.</summary>
public class ItemEmbalagem : EntidadeDaEmpresa
{
    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public string CodigoEmbalagem { get; set; } = "";
    public string? EanEmbalagem { get; set; }
    public int Quantidade { get; set; }
}

public class ItemColecao
{
    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int ColecaoId { get; set; }
    public Colecao? Colecao { get; set; }
}

public class Fornecedor : EntidadeDaEmpresa
{
    public string RazaoSocial { get; set; } = "";
    public string? Cnpj { get; set; }
    public string? Contato { get; set; }
}

public class Localizacao : EntidadeDaEmpresa
{
    /// <summary>Código de endereço, ex.: A-01-02.</summary>
    public string Codigo { get; set; } = "";
    public TipoLocalizacao Tipo { get; set; }
    public bool Ativa { get; set; } = true;
}

/// <summary>Picking de cada item, com mínimo e máximo (RN15).</summary>
public class ItemLocalizacao : EntidadeDaEmpresa
{
    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int LocalizacaoId { get; set; }
    public Localizacao? Localizacao { get; set; }

    public decimal Minimo { get; set; }
    public decimal Maximo { get; set; }
}
