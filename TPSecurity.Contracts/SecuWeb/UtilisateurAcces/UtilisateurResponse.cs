namespace TPSecurity.Contracts.SecuWeb.UtilisateurAcces
{
    public record UtilisateurAccesResponse(Guid IdSociete, List<UtilisateurApplicationReponse> Applications);

    public record UtilisateurApplicationReponse(int IdApplication, List<UtilisateurModuleReponse> Modules);

    public record UtilisateurModuleReponse(int IdModule, List<UtilisateurFonctionnaliteReponse> Fonctionnalites);

    public record UtilisateurFonctionnaliteReponse(string Libelle, string Permission);
}
