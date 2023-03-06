using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetAllRefFonctionnalite
{
    public class RefFonctionnaliteParameters : QueryParameters
    {
        public RefFonctionnaliteParameters()
        {
            orderBy = nameof(Libelle);
            orderOrientation = "asc";
        }

        public string? Libelle { get; set; }

        public bool? EstActif { get; set; }

        public int? idRefModule { get; set; }
    }
}
