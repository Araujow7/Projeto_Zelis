using Microsoft.EntityFrameworkCore;
using Zelis.Dominio;

namespace Zelis.Dados;

public class ZelisDbContext(DbContextOptions<ZelisDbContext> options) : DbContext(options)
{
    // Acesso
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Operador> Operadores => Set<Operador>();
    public DbSet<Permissao> Permissoes => Set<Permissao>();
    public DbSet<OperadorPermissao> OperadorPermissoes => Set<OperadorPermissao>();
    public DbSet<LogAcesso> LogsAcesso => Set<LogAcesso>();

    // Cadastros
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Colecao> Colecoes => Set<Colecao>();
    public DbSet<Item> Itens => Set<Item>();
    public DbSet<ItemEmbalagem> ItemEmbalagens => Set<ItemEmbalagem>();
    public DbSet<ItemColecao> ItemColecoes => Set<ItemColecao>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<Localizacao> Localizacoes => Set<Localizacao>();
    public DbSet<ItemLocalizacao> ItemLocalizacoes => Set<ItemLocalizacao>();

    // Estoque
    public DbSet<Recebimento> Recebimentos => Set<Recebimento>();
    public DbSet<RecebimentoItem> RecebimentoItens => Set<RecebimentoItem>();
    public DbSet<Saldo> Saldos => Set<Saldo>();
    public DbSet<Movimentacao> Movimentacoes => Set<Movimentacao>();
    public DbSet<Inventario> Inventarios => Set<Inventario>();
    public DbSet<InventarioItem> InventarioItens => Set<InventarioItem>();
    public DbSet<Pendencia> Pendencias => Set<Pendencia>();

    // Vendas e promoções
    public DbSet<Promocao> Promocoes => Set<Promocao>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<VendaItem> VendaItens => Set<VendaItem>();

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        // Valores e quantidades com 4 casas (pesáveis e preços); útil ao migrar para PostgreSQL.
        builder.Properties<decimal>().HavePrecision(18, 4);
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ---------- Chaves compostas e chaves de texto ----------
        mb.Entity<Permissao>().HasKey(p => p.Codigo);
        mb.Entity<OperadorPermissao>().HasKey(op => new { op.OperadorId, op.PermissaoCodigo });
        mb.Entity<ItemColecao>().HasKey(ic => new { ic.ItemId, ic.ColecaoId });

        // ---------- Unicidade (sempre dentro da empresa, RN12) ----------
        mb.Entity<Empresa>().HasIndex(e => e.Codigo).IsUnique();
        mb.Entity<Operador>().HasIndex(o => new { o.EmpresaId, o.Login }).IsUnique();
        mb.Entity<Categoria>().HasIndex(c => new { c.EmpresaId, c.Nome }).IsUnique();
        mb.Entity<Colecao>().HasIndex(c => new { c.EmpresaId, c.Nome }).IsUnique();
        mb.Entity<Item>().HasIndex(i => new { i.EmpresaId, i.CodigoProduto }).IsUnique(); // RN01
        mb.Entity<Item>().HasIndex(i => new { i.EmpresaId, i.EanUnidade });
        mb.Entity<ItemEmbalagem>().HasIndex(e => new { e.EmpresaId, e.CodigoEmbalagem }).IsUnique();
        mb.Entity<ItemEmbalagem>().HasIndex(e => new { e.EmpresaId, e.EanEmbalagem });
        mb.Entity<Localizacao>().HasIndex(l => new { l.EmpresaId, l.Codigo }).IsUnique();
        mb.Entity<ItemLocalizacao>().HasIndex(il => new { il.ItemId, il.LocalizacaoId }).IsUnique();
        mb.Entity<Venda>().HasIndex(v => new { v.EmpresaId, v.Caixa, v.NumeroPdv }).IsUnique();

        // RN03. Atenção: no SQLite, NULL não conflita em índice único, então saldos sem
        // validade são consolidados pelo código da fase 4 (não pelo índice).
        mb.Entity<Saldo>()
            .HasIndex(s => new { s.EmpresaId, s.ItemId, s.LocalizacaoId, s.Validade, s.Situacao })
            .IsUnique();

        // ---------- Índices de consulta ----------
        mb.Entity<Movimentacao>().HasIndex(m => new { m.EmpresaId, m.ItemId, m.DataHora });
        mb.Entity<Promocao>().HasIndex(p => new { p.EmpresaId, p.AlvoTipo, p.AlvoId });
        mb.Entity<Pendencia>().HasIndex(p => new { p.EmpresaId, p.Status, p.Tipo });
        mb.Entity<LogAcesso>().HasIndex(l => l.DataHora);

        // ---------- Tamanhos de texto (respeitados no PostgreSQL/SQL Server) ----------
        mb.Entity<Empresa>().Property(e => e.Codigo).HasMaxLength(10);
        mb.Entity<Empresa>().Property(e => e.Cnpj).HasMaxLength(14);
        mb.Entity<Operador>().Property(o => o.Login).HasMaxLength(50);
        mb.Entity<Permissao>().Property(p => p.Codigo).HasMaxLength(50);
        mb.Entity<OperadorPermissao>().Property(p => p.PermissaoCodigo).HasMaxLength(50);
        mb.Entity<Item>().Property(i => i.CodigoProduto).HasMaxLength(30);
        mb.Entity<Item>().Property(i => i.EanUnidade).HasMaxLength(14);
        mb.Entity<ItemEmbalagem>().Property(e => e.CodigoEmbalagem).HasMaxLength(30);
        mb.Entity<ItemEmbalagem>().Property(e => e.EanEmbalagem).HasMaxLength(14);
        mb.Entity<Fornecedor>().Property(f => f.Cnpj).HasMaxLength(14);
        mb.Entity<Localizacao>().Property(l => l.Codigo).HasMaxLength(20);

        // ---------- Dados iniciais ----------
        mb.Entity<Permissao>().HasData(CatalogoPermissoes.Todas.Select(p =>
            new Permissao { Codigo = p.Codigo, Descricao = p.Descricao, Modulo = p.Modulo }));

        AplicarConvencoes(mb);
    }

    /// <summary>
    /// Nomes em snake_case (como na especificação), enums gravados como texto e
    /// exclusão em cascata desligada: nada é apagado por tabela (RN11).
    /// </summary>
    private static void AplicarConvencoes(ModelBuilder mb)
    {
        foreach (var entidade in mb.Model.GetEntityTypes())
        {
            entidade.SetTableName(NomeBanco.SnakeCase(entidade.ClrType.Name));

            foreach (var propriedade in entidade.GetProperties())
            {
                propriedade.SetColumnName(NomeBanco.SnakeCase(propriedade.Name));

                var tipo = Nullable.GetUnderlyingType(propriedade.ClrType) ?? propriedade.ClrType;
                if (tipo.IsEnum)
                {
                    propriedade.SetProviderClrType(typeof(string));
                    propriedade.SetMaxLength(30);
                }
            }

            foreach (var fk in entidade.GetForeignKeys())
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        PrepararAlteracoes();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        PrepararAlteracoes();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void PrepararAlteracoes()
    {
        var agora = DateTime.UtcNow;

        foreach (var entrada in ChangeTracker.Entries<EntidadeDaEmpresa>())
        {
            if (entrada.State == EntityState.Added && entrada.Entity.CriadoEm == default)
                entrada.Entity.CriadoEm = agora;
        }

        // RN11: a ficha de movimentações não é alterada nem apagada, só estornada.
        foreach (var entrada in ChangeTracker.Entries<Movimentacao>())
        {
            if (entrada.State is EntityState.Modified or EntityState.Deleted)
                throw new RegraDeNegocioException("RN11",
                    "Movimentações não podem ser alteradas nem apagadas. Lance um estorno.");
        }
    }
}
