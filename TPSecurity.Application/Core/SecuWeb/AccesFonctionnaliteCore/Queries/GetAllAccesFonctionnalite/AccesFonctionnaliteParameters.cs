using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetAllAccesFonctionnalite
{
    public class AccesFonctionnaliteParameters : QueryParameters
    {
        public AccesFonctionnaliteParameters()
        {
            orderBy = nameof(EstActif);
            orderOrientation = "asc";
        }

        public bool? EstActif { get; set; }

        public int? IdAccesModule { get; set; }

        public int? IdRefFonctionnalite { get; set; }
    }
}
