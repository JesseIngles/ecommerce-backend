namespace Kimbito.Domain.Enums;

/// <summary>
/// Nível de acesso do utilizador: Cliente (compra), Gestor ou Admin (gerem produtos, pagamentos, encomendas).
/// </summary>
public enum NivelUtilizador
{
    Cliente = 0,
    Gestor = 1,
    Admin = 2
}
