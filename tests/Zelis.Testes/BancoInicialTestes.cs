using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Zelis.Dados;
using Zelis.Dominio;

namespace Zelis.Testes;

/// <summary>Fase 0: o banco inicial tem exatamente as tabelas e regras da especificação.</summary>
public class BancoInicialTestes : IDisposable
{
    private readonly BancoEmMemoria _banco = new();

    public static readonly string[] TabelasDaEspecificacao =
    [
        "empresa", "operador", "permissao", "operador_permissao", "categoria", "colecao",
        "item", "item_embalagem", "item_colecao", "fornecedor", "localizacao", "item_localizacao",
        "recebimento", "recebimento_item", "saldo", "movimentacao", "promocao", "inventario",
        "inventario_item", "pendencia", "venda", "venda_item", "log_acesso"
    ];

    private const string SqlTabelas =
        "SELECT name FROM sqlite_master WHERE type = 'table' " +
        "AND name NOT LIKE 'sqlite_%' AND name <> '__EFMigrationsHistory' ORDER BY name";

    public void Dispose() => _banco.Dispose();

    [Fact]
    public void Banco_tem_as_23_tabelas_da_especificacao()
    {
        var tabelas = _banco.Consultar(SqlTabelas);

        Assert.Equal(23, TabelasDaEspecificacao.Length);
        Assert.Equal(TabelasDaEspecificacao.Order(StringComparer.Ordinal), tabelas.Order(StringComparer.Ordinal));
    }

    [Theory]
    [InlineData("operador")]
    [InlineData("item")]
    [InlineData("saldo")]
    [InlineData("movimentacao")]
    [InlineData("recebimento")]
    [InlineData("venda")]
    [InlineData("pendencia")]
    public void Tabelas_de_operacao_tem_empresa_e_auditoria_de_criacao(string tabela)
    {
        var colunas = _banco.Consultar($"SELECT name FROM pragma_table_info('{tabela}')");

        Assert.Contains("empresa_id", colunas);
        Assert.Contains("criado_em", colunas);
        Assert.Contains("criado_por", colunas);
    }

    [Fact]
    public void Colunas_seguem_os_nomes_da_especificacao()
    {
        var colunas = _banco.Consultar("SELECT name FROM pragma_table_info('item')");

        Assert.Contains("codigo_produto", colunas);
        Assert.Contains("ean_unidade", colunas);
        Assert.Contains("preco_venda", colunas);
        Assert.Contains("controla_validade", colunas);
    }

    [Fact]
    public void Lista_unica_de_permissoes_ja_vem_cadastrada()
    {
        using var db = _banco.NovoContexto();

        var codigos = db.Permissoes.Select(p => p.Codigo).AsEnumerable().Order(StringComparer.Ordinal);

        Assert.Equal(CatalogoPermissoes.Todas.Select(p => p.Codigo).Order(StringComparer.Ordinal), codigos);
    }

    [Fact]
    public void RN01_codigo_do_produto_nao_se_repete_na_mesma_empresa()
    {
        using var db = _banco.NovoContexto();
        var empresa = NovaEmpresa(db, "001");
        db.Itens.Add(NovoItem(empresa, "1001"));
        db.SaveChanges();

        db.Itens.Add(NovoItem(empresa, "1001"));

        Assert.Throws<DbUpdateException>(() => db.SaveChanges());
    }

    [Fact]
    public void RN12_empresas_diferentes_podem_usar_o_mesmo_codigo()
    {
        using var db = _banco.NovoContexto();
        var e1 = NovaEmpresa(db, "001");
        var e2 = NovaEmpresa(db, "002");

        db.Itens.AddRange(NovoItem(e1, "1001"), NovoItem(e2, "1001"));
        db.SaveChanges();

        Assert.Equal(2, db.Itens.Count(i => i.CodigoProduto == "1001"));
    }

    [Fact]
    public void Criado_em_e_preenchido_automaticamente()
    {
        using var db = _banco.NovoContexto();
        var empresa = NovaEmpresa(db, "001");
        var item = NovoItem(empresa, "1001");
        db.Itens.Add(item);
        db.SaveChanges();

        Assert.True(item.CriadoEm > DateTime.UtcNow.AddMinutes(-1));
    }

    [Fact]
    public void Enums_sao_gravados_como_texto_legivel()
    {
        using var db = _banco.NovoContexto();
        var empresa = NovaEmpresa(db, "001");
        db.Localizacoes.Add(new Localizacao { EmpresaId = empresa.Id, Codigo = "A-01-02", Tipo = TipoLocalizacao.Picking });
        db.SaveChanges();

        Assert.Equal(new[] { "Picking" }, _banco.Consultar("SELECT tipo FROM localizacao"));
    }

    [Fact]
    public void Valores_decimais_sao_preservados()
    {
        using var db = _banco.NovoContexto();
        var empresa = NovaEmpresa(db, "001");
        var item = NovoItem(empresa, "1001");
        item.PrecoVenda = 129.90m;
        item.Custo = 64.4512m;
        db.Itens.Add(item);
        db.SaveChanges();

        using var outro = _banco.NovoContexto();
        var lido = outro.Itens.Single();
        Assert.Equal(129.90m, lido.PrecoVenda);
        Assert.Equal(64.4512m, lido.Custo);
    }

    [Fact]
    public void Chave_estrangeira_impede_item_com_categoria_inexistente()
    {
        using var db = _banco.NovoContexto();
        var empresa = NovaEmpresa(db, "001");
        var item = NovoItem(empresa, "1001");
        item.CategoriaId = 999;
        db.Itens.Add(item);

        Assert.Throws<DbUpdateException>(() => db.SaveChanges());
    }

    [Fact]
    public void RN11_movimentacao_nao_pode_ser_alterada()
    {
        using var db = _banco.NovoContexto();
        var mov = NovaMovimentacao(db);

        mov.Quantidade = 99;

        var erro = Assert.Throws<RegraDeNegocioException>(() => db.SaveChanges());
        Assert.Equal("RN11", erro.CodigoRegra);
    }

    [Fact]
    public void RN11_movimentacao_nao_pode_ser_apagada()
    {
        using var db = _banco.NovoContexto();
        var mov = NovaMovimentacao(db);

        db.Movimentacoes.Remove(mov);

        var erro = Assert.Throws<RegraDeNegocioException>(() => db.SaveChanges());
        Assert.Equal("RN11", erro.CodigoRegra);
    }

    [Fact]
    public void RN11_correcao_e_feita_por_estorno()
    {
        using var db = _banco.NovoContexto();
        var original = NovaMovimentacao(db);

        db.Movimentacoes.Add(new Movimentacao
        {
            EmpresaId = original.EmpresaId,
            Tipo = TipoMovimentacao.Estorno,
            ItemId = original.ItemId,
            OrigemId = original.DestinoId,
            Quantidade = original.Quantidade,
            OperadorId = original.OperadorId,
            DataHora = DateTime.UtcNow,
            EstornoDeId = original.Id
        });
        db.SaveChanges();

        Assert.Equal(2, db.Movimentacoes.Count());
    }

    [Fact]
    public void Migracao_inicial_esta_em_dia_com_o_modelo()
    {
        using var db = _banco.NovoContexto();

        Assert.NotEmpty(db.Database.GetMigrations());
        Assert.False(db.Database.HasPendingModelChanges(),
            "O modelo mudou e falta gerar uma migração (dotnet ef migrations add ...).");
    }

    [Fact]
    public void Migracao_cria_o_banco_em_arquivo_com_as_23_tabelas()
    {
        var arquivo = Path.Combine(Path.GetTempPath(), $"zelis-teste-{Guid.NewGuid():N}.db");
        try
        {
            var opcoes = new DbContextOptionsBuilder<ZelisDbContext>()
                .UseSqlite($"Data Source={arquivo};Pooling=False").Options;
            using (var db = new ZelisDbContext(opcoes))
                ConfiguracaoDados.PrepararBanco(db);

            using var conexao = new SqliteConnection($"Data Source={arquivo};Pooling=False");
            conexao.Open();
            using var cmd = conexao.CreateCommand();
            cmd.CommandText = SqlTabelas;
            using var leitor = cmd.ExecuteReader();
            var tabelas = new List<string>();
            while (leitor.Read()) tabelas.Add(leitor.GetString(0));

            Assert.Equal(TabelasDaEspecificacao.Order(StringComparer.Ordinal), tabelas.Order(StringComparer.Ordinal));
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            foreach (var f in new[] { arquivo, arquivo + "-wal", arquivo + "-shm" })
                if (File.Exists(f)) File.Delete(f);
        }
    }

    // ---------- Auxiliares ----------

    private static Empresa NovaEmpresa(ZelisDbContext db, string codigo)
    {
        var empresa = new Empresa { Codigo = codigo, RazaoSocial = $"Empresa {codigo}" };
        db.Empresas.Add(empresa);
        db.SaveChanges();
        return empresa;
    }

    private static Item NovoItem(Empresa empresa, string codigo) => new()
    {
        EmpresaId = empresa.Id,
        CodigoProduto = codigo,
        Descricao = "Perfume Sunset 100 ml",
        PrecoVenda = 129.90m,
        Custo = 60m
    };

    private static Movimentacao NovaMovimentacao(ZelisDbContext db)
    {
        var empresa = NovaEmpresa(db, "001");
        var item = NovoItem(empresa, "1001");
        var local = new Localizacao { EmpresaId = empresa.Id, Codigo = "A-01-02", Tipo = TipoLocalizacao.Picking };
        var operador = new Operador { EmpresaId = empresa.Id, Login = "admin", Nome = "Administrador", SenhaHash = "x" };
        db.AddRange(item, local, operador);
        db.SaveChanges();

        var mov = new Movimentacao
        {
            EmpresaId = empresa.Id,
            Tipo = TipoMovimentacao.Entrada,
            ItemId = item.Id,
            DestinoId = local.Id,
            Validade = new DateOnly(2027, 8, 31),
            Lote = "L123",
            Quantidade = 14,
            OperadorId = operador.Id,
            DataHora = DateTime.UtcNow
        };
        db.Movimentacoes.Add(mov);
        db.SaveChanges();
        return mov;
    }
}
