namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common
{
    public record RefFonctionnaliteResult (int Id, string Libelle, bool EstActif, bool EstDefaut, string Permission, int IdRefModule, string HashCode);
}
