namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;

public record RefModuleResult(int Id, string Libelle, bool EstActif, int IdRefApplication, string HashCode);