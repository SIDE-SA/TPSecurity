using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetAllAccesApplication
{
    public class AccesApplicationParameters : QueryParameters
    {
        public AccesApplicationParameters()
        {
            orderBy = nameof(EstActif);
            orderOrientation = "asc";
        }


        public bool? EstActif { get; set; }

        public int? IdAccesGroupe { get; set; }

        public int? IdRefApplication { get; set; }
    }
}
