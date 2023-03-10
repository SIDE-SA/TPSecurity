namespace TPSecurity.Contracts.SecuWeb.Utilisateur;

public record CreateUtilisateurRequest(string Nom, string Prenom, string Email, bool EstActif);