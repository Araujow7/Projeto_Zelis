using System.Text;

namespace Zelis.Dados;

internal static class NomeBanco
{
    /// <summary>"ItemEmbalagem" → "item_embalagem"; "CodigoProduto" → "codigo_produto".</summary>
    public static string SnakeCase(string nome)
    {
        var sb = new StringBuilder(nome.Length + 8);
        for (var i = 0; i < nome.Length; i++)
        {
            var c = nome[i];
            if (char.IsUpper(c))
            {
                if (i > 0) sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}
