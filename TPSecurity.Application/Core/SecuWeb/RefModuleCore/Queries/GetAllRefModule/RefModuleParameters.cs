using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetAllRefModule
{
    public class RefModuleParameters : QueryParameters
    {
        public RefModuleParameters()
        {
            orderBy = nameof(Libelle);
            orderOrientation = "asc";
        }

        public string? Libelle { get; set; }

        public bool? EstActif { get; set; }

        public int? IdRefApplication { get; set; }
    }
}
