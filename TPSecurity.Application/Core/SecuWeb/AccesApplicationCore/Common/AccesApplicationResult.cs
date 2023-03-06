namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common
{
    public record AccesApplicationResult(int Id, bool EstActif, int IdAccesGroupe, int IdRefApplication, string HashCode);
}
