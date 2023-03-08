namespace TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Common
{
    public record UtilisateurAccesResult(Guid IdSociete, List<UtilisateurApplicationResult> Applications);

    public record UtilisateurApplicationResult(int IdApplication, List<UtilisateurModuleResult> Modules);

    public record UtilisateurModuleResult(int IdModule, List<UtilisateurFonctionnaliteResult> Fonctionnalites);

    public record UtilisateurFonctionnaliteResult(string Libelle, string Permission);
}
