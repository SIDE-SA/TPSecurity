namespace TPSecurity.Contracts.SecuWeb.AccesModule
{
    public record AccesModuleResponse(int Identifiant, bool EstActif, int IdAccesApplication, int IdRefModule);
}
