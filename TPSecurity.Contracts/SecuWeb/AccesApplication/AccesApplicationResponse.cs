namespace TPSecurity.Contracts.SecuWeb.AccesApplication;

public record AccesApplicationResponse(int Identifiant, bool EstActif, int IdAccesGroupe, int IdRefApplication);