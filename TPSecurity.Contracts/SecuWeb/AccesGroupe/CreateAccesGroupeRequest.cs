namespace TPSecurity.Contracts.SecuWeb.AccesGroupe;

public record CreateAccesGroupeRequest(string Libelle, bool EstActif, Guid IdSociete, bool EstGroupeSpecial);