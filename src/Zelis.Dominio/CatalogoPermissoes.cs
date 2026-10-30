namespace Zelis.Dominio;

/// <summary>
/// Lista única de permissões (módulo 2). Não há perfis: o administrador ativa ou
/// desativa cada permissão por operador (RN13).
/// </summary>
public static class CatalogoPermissoes
{
    public const string CadastrarItens = "ITEM_CADASTRAR";
    public const string AlterarPreco = "PRECO_ALTERAR";
    public const string CriarPromocao = "PROMOCAO_CRIAR";
    public const string ReceberMercadoria = "RECEBIMENTO_EXECUTAR";
    public const string TransferirEstoque = "ESTOQUE_TRANSFERIR";
    public const string LancarPerda = "PERDA_LANCAR";
    public const string ImportarInventario = "INVENTARIO_IMPORTAR";
    public const string AprovarAjuste = "INVENTARIO_APROVAR";
    public const string VerRelatoriosDeValor = "RELATORIO_VALOR";
    public const string CadastrarOperadores = "OPERADOR_CADASTRAR";

    public static IReadOnlyList<Permissao> Todas { get; } =
    [
        new() { Codigo = CadastrarItens, Descricao = "Cadastrar itens", Modulo = "Cadastro de itens" },
        new() { Codigo = AlterarPreco, Descricao = "Alterar preço", Modulo = "Preços e promoções" },
        new() { Codigo = CriarPromocao, Descricao = "Criar promoção", Modulo = "Preços e promoções" },
        new() { Codigo = ReceberMercadoria, Descricao = "Receber mercadoria", Modulo = "Recebimento" },
        new() { Codigo = TransferirEstoque, Descricao = "Transferir estoque", Modulo = "Estoque" },
        new() { Codigo = LancarPerda, Descricao = "Lançar perda", Modulo = "Estoque" },
        new() { Codigo = ImportarInventario, Descricao = "Importar inventário", Modulo = "Inventário" },
        new() { Codigo = AprovarAjuste, Descricao = "Aprovar ajuste de inventário", Modulo = "Inventário" },
        new() { Codigo = VerRelatoriosDeValor, Descricao = "Ver relatórios de valor", Modulo = "Análise de estoque" },
        new() { Codigo = CadastrarOperadores, Descricao = "Cadastrar operadores", Modulo = "Operadores e permissões" },
    ];
}
