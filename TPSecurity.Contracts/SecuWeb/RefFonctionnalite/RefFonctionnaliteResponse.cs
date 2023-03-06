namespace TPSecurity.Contracts.SecuWeb.RefFonctionnalite
{
    public record RefFonctionnaliteResponse(int Identifiant, string Libelle, bool EstActif, bool EstDefaut, string Permission, int IdRefModule);
}
