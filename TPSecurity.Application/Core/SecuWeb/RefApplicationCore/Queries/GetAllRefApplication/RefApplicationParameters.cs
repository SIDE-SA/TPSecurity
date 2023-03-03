using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetAllRefApplication
{
    public class RefApplicationParameters : QueryParameters
    {
        public RefApplicationParameters()
        {
            orderBy = nameof(Libelle);
            orderOrientation = "asc";
        }

        public string? Libelle { get; set; };

        public bool? EstActif { get; set; };
    }
}
