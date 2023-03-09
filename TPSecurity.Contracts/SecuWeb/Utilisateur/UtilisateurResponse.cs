namespace TPSecurity.Contracts.SecuWeb.Utilisateur;

public record UtilisateurResponse(int Identifiant, string Nom, string Prenom, string Email, bool EstActif);