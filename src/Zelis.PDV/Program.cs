using Zelis.Compartilhado;

namespace Zelis.PDV;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new TelaInicial("Zelis PDV", "http://localhost:5080/"));
    }
}
