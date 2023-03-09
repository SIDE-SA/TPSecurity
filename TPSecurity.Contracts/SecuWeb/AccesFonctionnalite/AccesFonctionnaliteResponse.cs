namespace TPSecurity.Contracts.SecuWeb.AccesFonctionnalite;

public record AccesFonctionnaliteResponse(int Identifiant, bool EstActif, int IdAccesModule, int IdRefFonctionnalite);