namespace TPSecurity.Contracts.SecuWeb.AccesUtilisateur;

public record CreateAccesUtilisateurRequest(bool EstActif, int IdAccesGroupe, int IdUtilisateur);