namespace TPSecurity.Contracts.SecuWeb.AccesModule;

public record CreateAccesModuleRequest(bool EstActif, int IdAccesApplication, int IdRefModule);