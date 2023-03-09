namespace TPSecurity.Contracts.SecuWeb.Utilisateur;

public record CreateUtilisateurRequest(string Libelle, string Nom, string Prenom, string Email, bool EstActif);