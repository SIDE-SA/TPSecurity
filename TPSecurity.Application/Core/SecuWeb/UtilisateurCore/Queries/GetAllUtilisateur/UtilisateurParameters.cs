using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetAllUtilisateur
{
    public class UtilisateurParameters : QueryParameters
    {
        public UtilisateurParameters()
        {
            orderBy = nameof(Nom);
            orderOrientation = "asc";
        }

        public string? Nom { get; set; }

        public string? Prenom { get; set; }

        public string? Email { get; set; }

        public bool? EstActif { get; set; }
    }
}
