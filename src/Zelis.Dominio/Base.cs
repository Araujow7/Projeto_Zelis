namespace Zelis.Dominio;

/// <summary>Toda entidade tem um identificador numérico.</summary>
public abstract class Entidade
{
    public int Id { get; set; }
}

/// <summary>
/// Entidade de operação: pertence a uma empresa (RN12) e registra quem criou e quando.
/// </summary>
public abstract class EntidadeDaEmpresa : Entidade
{
    public int EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }

    public DateTime CriadoEm { get; set; }
    public int? CriadoPor { get; set; }
}
