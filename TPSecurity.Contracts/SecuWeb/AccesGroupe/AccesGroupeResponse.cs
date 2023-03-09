namespace TPSecurity.Contracts.SecuWeb.AccesGroupe;

public record AccesGroupeResponse(int Identifiant, string Libelle, bool EstActif, Guid IdSociete, bool EstGroupeSpecial);