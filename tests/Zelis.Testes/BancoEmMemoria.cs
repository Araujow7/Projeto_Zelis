using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Zelis.Dados;

namespace Zelis.Testes;

/// <summary>Banco SQLite em memória, recriado a cada teste a partir do modelo.</summary>
public sealed class BancoEmMemoria : IDisposable
{
    private readonly SqliteConnection _conexao;

    public BancoEmMemoria()
    {
        _conexao = new SqliteConnection("Data Source=:memory:");
        _conexao.Open();
        using var db = NovoContexto();
        db.Database.EnsureCreated();
    }

    public ZelisDbContext NovoContexto() =>
        new(new DbContextOptionsBuilder<ZelisDbContext>().UseSqlite(_conexao).Options);

    public List<string> Consultar(string sql)
    {
        using var cmd = _conexao.CreateCommand();
        cmd.CommandText = sql;
        using var leitor = cmd.ExecuteReader();
        var linhas = new List<string>();
        while (leitor.Read()) linhas.Add(Convert.ToString(leitor.GetValue(0))!);
        return linhas;
    }

    public void Dispose() => _conexao.Dispose();
}
