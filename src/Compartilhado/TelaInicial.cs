using System.Drawing;
using System.Net.Http.Json;
using System.Reflection;
using System.Windows.Forms;

namespace Zelis.Compartilhado;

/// <summary>
/// Tela inicial da fase 0, comum ao WMS e ao PDV: cabeçalho com as cores da marca,
/// botão para testar a API e a versão no rodapé.
/// </summary>
public class TelaInicial : Form
{
    // Paleta da especificação (fundo claro)
    private static readonly Color AzulMarinho = ColorTranslator.FromHtml("#2E3E58");
    private static readonly Color Ambar = ColorTranslator.FromHtml("#E0A030");
    private static readonly Color Grafite = ColorTranslator.FromHtml("#1E1E2A");

    private readonly string _enderecoApi;
    private readonly Label _status;
    private readonly Panel _ponto;

    public TelaInicial(string nomePrograma, string enderecoApi)
    {
        _enderecoApi = enderecoApi;

        Text = nomePrograma;
        Size = new Size(640, 400);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 10f);

        var cabecalho = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = AzulMarinho };
        cabecalho.Controls.Add(new Label
        {
            Text = nomePrograma.ToLowerInvariant(),
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 20f),
            AutoSize = true,
            Location = new Point(20, 12)
        });

        var botao = new Button
        {
            Text = "Testar conexão com o servidor",
            BackColor = AzulMarinho,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            AutoSize = true,
            Padding = new Padding(12, 6, 12, 6),
            Location = new Point(24, 100)
        };
        botao.Click += async (_, _) => await TestarApiAsync();

        // O ponto âmbar só acende quando algo pede atenção (servidor fora do ar).
        _ponto = new Panel { Size = new Size(12, 12), Location = new Point(26, 166), BackColor = Color.LightGray };
        _status = new Label
        {
            Text = $"Servidor: {_enderecoApi}",
            ForeColor = Grafite,
            AutoSize = true,
            Location = new Point(46, 160)
        };

        var versao = Assembly.GetEntryAssembly()?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
            .Split('+')[0] ?? "0.0.0";
        var rodape = new Label
        {
            Text = $"{nomePrograma} v{versao}",
            Dock = DockStyle.Bottom,
            Height = 28,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.Gray,
            Padding = new Padding(0, 0, 12, 0)
        };

        Controls.AddRange([cabecalho, botao, _ponto, _status, rodape]);
    }

    private async Task TestarApiAsync()
    {
        _status.Text = "Testando...";
        try
        {
            using var http = new HttpClient { BaseAddress = new Uri(_enderecoApi), Timeout = TimeSpan.FromSeconds(5) };
            var saude = await http.GetFromJsonAsync<RespostaSaude>("saude");
            _ponto.BackColor = Color.SeaGreen;
            _status.Text = $"Conectado: {saude?.Sistema} v{saude?.Versao}, banco {saude?.Banco}";
        }
        catch (Exception ex)
        {
            _ponto.BackColor = Ambar;
            _status.Text = $"Servidor fora do ar ({ex.GetType().Name})";
        }
    }

    private sealed record RespostaSaude(string Sistema, string Versao, string Banco);
}
