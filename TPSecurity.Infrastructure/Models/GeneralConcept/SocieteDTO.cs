
namespace TPSecurity.Infrastructure.Models.GeneralConcept
{
    public partial class SocieteDTO
    {
        public Guid Identifiant { get; set; }
        public string? Nom { get; set; }
        public bool GereLocation { get; set; }
        public bool GereMateriel { get; set; }
        public string? CodeSociete { get; set; }
        public bool EstActif { get; set; }

        public SocieteDTO(Guid identifiant, string? nom, bool gereLocation, bool gereMateriel, string? codeSociete, bool estActif)
        {
            Identifiant = identifiant;
            Nom = nom;
            GereLocation = gereLocation;
            GereMateriel = gereMateriel;
            CodeSociete = codeSociete;
            EstActif = estActif;
        }
    }
}
