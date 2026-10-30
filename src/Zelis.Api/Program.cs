using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Zelis.Dados;

var builder = WebApplication.CreateBuilder(args);

// O banco fica ao lado do executável da API, em dados/zelis.db (configurável).
var conexao = new SqliteConnectionStringBuilder(
    builder.Configuration.GetConnectionString("Zelis") ?? "Data Source=dados/zelis.db");
if (conexao.DataSource != ":memory:" && !Path.IsPathRooted(conexao.DataSource))
    conexao.DataSource = Path.Combine(AppContext.BaseDirectory, conexao.DataSource);

builder.Services.AddZelisDados(conexao.ToString());

var app = builder.Build();

if (app.Configuration.GetValue("Banco:PrepararAoIniciar", true))
{
    using var escopo = app.Services.CreateScope();
    ConfiguracaoDados.PrepararBanco(escopo.ServiceProvider.GetRequiredService<ZelisDbContext>());
}

var versao = Assembly.GetExecutingAssembly()
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
    .Split('+')[0] ?? "0.0.0";

// Verificação de saúde: usada pelo Desktop e pelo PDV para saber se o servidor está no ar.
// Não exige login e não devolve nenhum dado pessoal.
app.MapGet("/saude", async (ZelisDbContext db) =>
{
    var bancoOk = await db.Database.CanConnectAsync();
    return Results.Ok(new
    {
        sistema = "Zelis",
        versao,
        banco = bancoOk ? "ok" : "indisponivel",
        horaServidor = DateTime.UtcNow
    });
});

app.Run();

// Permite que os testes automatizados subam a API em memória.
public partial class Program;
