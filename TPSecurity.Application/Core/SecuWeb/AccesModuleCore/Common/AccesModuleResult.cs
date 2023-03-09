namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;

public record AccesModuleResult(int Id, bool EstActif, int IdAccesApplication, int IdRefModule, string HashCode);