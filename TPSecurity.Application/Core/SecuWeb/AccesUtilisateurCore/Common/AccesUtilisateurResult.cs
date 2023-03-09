namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;

public record AccesUtilisateurResult(int Id, bool EstActif, int IdAccesGroupe, int IdUtilisateur, string HashCode);