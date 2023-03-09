namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;

public record UtilisateurResult(int Id, string Nom, string Prenom, string Email, bool EstActif, string HashCode);