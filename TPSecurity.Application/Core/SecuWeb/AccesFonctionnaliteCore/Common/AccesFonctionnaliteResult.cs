namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common
{
    public record AccesFonctionnaliteResult(int Id, bool EstActif, int IdAccesModule, int IdRefFonctionnalite, string HashCode);
}
