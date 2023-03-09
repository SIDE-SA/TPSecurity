using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetAllAccesUtilisateur
{
    public class AccesUtilisateurParameters : QueryParameters
    {
        public AccesUtilisateurParameters()
        {
            orderBy = nameof(EstActif);
            orderOrientation = "asc";
        }

        public bool? EstActif { get; set; }

        public int? IdAccesGroupe { get; set; }

        public int? IdUtilisateur { get; set; }
    }
}
