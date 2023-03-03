namespace TPSecurity.Infrastructure.Interfaces;

public interface ITenantService
{
    /// <summary>
    /// Retourne l id (uuid) de la société passé en paramètre de la requête (header)"
    /// </summary>
    /// <returns></returns>
    public Guid GetIdSociete();
}
