namespace TPSecurity.Contracts.SecuWeb.AccesApplication;

public record CreateAccesApplicationRequest(bool EstActif, int IdAccesGroupe, int IdRefApplication, bool EstGroupeSpecial);