using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetAllAccesModule
{
    public class AccesModuleParameters : QueryParameters
    {
        public AccesModuleParameters()
        {
            orderBy = nameof(EstActif);
            orderOrientation = "asc";
        }

        public bool? EstActif { get; set; }

        public int? IdAccesApplication { get; set; }

        public int? IdRefModule { get; set; }
    }
}
