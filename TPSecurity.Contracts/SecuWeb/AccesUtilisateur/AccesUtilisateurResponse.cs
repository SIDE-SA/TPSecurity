namespace TPSecurity.Contracts.SecuWeb.AccesUtilisateur;

public record AccesUtilisateurResponse(int Identifiant, bool EstActif, int IdAccesGroupe, int IdUtilisateur);