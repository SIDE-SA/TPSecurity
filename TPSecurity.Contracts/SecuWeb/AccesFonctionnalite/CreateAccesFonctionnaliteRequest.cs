namespace TPSecurity.Contracts.SecuWeb.AccesFonctionnalite;

public record CreateAccesFonctionnaliteRequest(bool EstActif, int IdAccesModule, int IdRefFonctionnalite);