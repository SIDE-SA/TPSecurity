namespace TPSecurity.Contracts.SecuWeb.Utilisateur;

public record UpdateUtilisateurRequest(string Nom, string Prenom, string Email, bool EstActif);
