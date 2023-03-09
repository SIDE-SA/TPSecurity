namespace TPSecurity.Contracts.SecuWeb.RefFonctionnalite;

public record CreateRefFonctionnaliteRequest(string Libelle, bool EstActif, bool EstDefaut, string Permission, int IdRefModule);