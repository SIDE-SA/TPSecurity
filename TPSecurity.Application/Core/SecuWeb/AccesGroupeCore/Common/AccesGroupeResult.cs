namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;

public record AccesGroupeResult(int Id, string Libelle, bool EstActif, bool EstGroupeSpecial, Guid IdSociete, string HashCode);