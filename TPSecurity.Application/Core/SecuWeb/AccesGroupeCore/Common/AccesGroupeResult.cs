namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common
{
    public record AccesGroupeResult(int Id, string Libelle, bool EstActif, Guid IdSociete, bool EstGroupeSpecial, string HashCode);
}
