using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Zelis.Testes;

/// <summary>Fase 0: a API sobe, prepara o banco e responde à verificação de saúde.</summary>
public class ApiTestes : IDisposable
{
    private readonly string _arquivo = Path.Combine(Path.GetTempPath(), $"zelis-api-{Guid.NewGuid():N}.db");
    private readonly WebApplicationFactory<Program> _fabrica;

    public ApiTestes()
    {
        _fabrica = new WebApplicationFactory<Program>().WithWebHostBuilder(b =>
            b.UseSetting("ConnectionStrings:Zelis", $"Data Source={_arquivo};Pooling=False"));
    }

    [Fact]
    public async Task Saude_responde_com_sistema_versao_e_banco()
    {
        var http = _fabrica.CreateClient();

        var resposta = await http.GetAsync("/saude");
        var corpo = await resposta.Content.ReadFromJsonAsync<Saude>();

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Equal("Zelis", corpo!.Sistema);
        Assert.Equal("0.1.0", corpo.Versao);
        Assert.Equal("ok", corpo.Banco);
    }

    [Fact]
    public async Task Api_cria_o_arquivo_do_banco_ao_iniciar()
    {
        var http = _fabrica.CreateClient();
        await http.GetAsync("/saude");

        Assert.True(File.Exists(_arquivo));
    }

    public void Dispose()
    {
        _fabrica.Dispose();
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        foreach (var f in new[] { _arquivo, _arquivo + "-wal", _arquivo + "-shm" })
            if (File.Exists(f)) File.Delete(f);
    }

    private sealed record Saude(string Sistema, string Versao, string Banco);
}
