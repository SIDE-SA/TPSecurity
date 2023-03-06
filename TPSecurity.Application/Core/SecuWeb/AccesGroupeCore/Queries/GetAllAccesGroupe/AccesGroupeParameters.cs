using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetAllAccesGroupe
{
    public class AccesGroupeParameters : QueryParameters
    {
        public AccesGroupeParameters()
        {
            orderBy = nameof(Libelle);
            orderOrientation = "asc";
        }

        public string? Libelle { get; set; }

        public bool? EstActif { get; set; }

        public bool? EstGroupeSpecial { get; set; }

        public Guid? IdSociete { get; set; }
    }
}
