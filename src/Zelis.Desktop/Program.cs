using Zelis.Compartilhado;

namespace Zelis.Desktop;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new TelaInicial("Zelis", "http://localhost:5080/"));
    }
}
