namespace Zelis.Dominio;

/// <summary>Empresa em que o operador está cadastrado (001, 002...).</summary>
public class Empresa : Entidade
{
    public string Codigo { get; set; } = "";
    public string RazaoSocial { get; set; } = "";
    public string? Cnpj { get; set; }
    public bool Ativa { get; set; } = true;
}

public class Operador : EntidadeDaEmpresa
{
    public string Login { get; set; } = "";
    public string Nome { get; set; } = "";

    /// <summary>Informativo (Operador, Operador 1...). Não define acessos: não há perfis.</summary>
    public string? Funcao { get; set; }

    /// <summary>Hash forte (bcrypt ou Argon2), nunca a senha em texto.</summary>
    public string SenhaHash { get; set; } = "";

    public int Tentativas { get; set; }
    public bool Bloqueado { get; set; }
    public bool TrocarSenha { get; set; } = true;
    public bool Ativo { get; set; } = true;

    public List<OperadorPermissao> Permissoes { get; set; } = [];
}

/// <summary>Lista única de permissões do sistema.</summary>
public class Permissao
{
    public string Codigo { get; set; } = "";
    public string Descricao { get; set; } = "";
    public string Modulo { get; set; } = "";
}

/// <summary>RN13: permissão ativada pelo administrador para um operador.</summary>
public class OperadorPermissao
{
    public int OperadorId { get; set; }
    public Operador? Operador { get; set; }

    public string PermissaoCodigo { get; set; } = "";
    public Permissao? Permissao { get; set; }
}

/// <summary>
/// Auditoria de login. Por LGPD não guarda o login digitado nem a senha:
/// tentativas com operador inexistente ficam com OperadorId nulo.
/// </summary>
public class LogAcesso : Entidade
{
    public int? EmpresaId { get; set; }
    public int? OperadorId { get; set; }
    public DateTime DataHora { get; set; }
    public bool Sucesso { get; set; }
    public string? Dispositivo { get; set; }
}
