namespace Zelis.Dominio;

/// <summary>Violação de uma regra de negócio, com o código da regra (RN01...RN15).</summary>
public class RegraDeNegocioException(string codigoRegra, string mensagem)
    : InvalidOperationException($"{codigoRegra}: {mensagem}")
{
    public string CodigoRegra { get; } = codigoRegra;
}
