using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Zelis.Dados;

public static class ConfiguracaoDados
{
    /// <summary>Registra o banco SQLite. Só a API chama este método.</summary>
    public static IServiceCollection AddZelisDados(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ZelisDbContext>(o => o.UseSqlite(connectionString));
        return services;
    }

    /// <summary>
    /// Cria a pasta do arquivo, aplica as migrações e liga o modo WAL
    /// (leituras não travam enquanto a API grava).
    /// </summary>
    public static void PrepararBanco(ZelisDbContext db)
    {
        var dataSource = new SqliteConnectionStringBuilder(db.Database.GetConnectionString()).DataSource;
        if (!string.IsNullOrWhiteSpace(dataSource) && dataSource != ":memory:")
        {
            var pasta = Path.GetDirectoryName(Path.GetFullPath(dataSource));
            if (!string.IsNullOrEmpty(pasta)) Directory.CreateDirectory(pasta);
        }

        db.Database.Migrate();
        db.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL;");
    }
}

/// <summary>Usado só pelo comando "dotnet ef" para gerar migrações.</summary>
public class ZelisDbContextFactory : IDesignTimeDbContextFactory<ZelisDbContext>
{
    public ZelisDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ZelisDbContext>()
            .UseSqlite("Data Source=zelis-design.db")
            .Options;
        return new ZelisDbContext(options);
    }
}
