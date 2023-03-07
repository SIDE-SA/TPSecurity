namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class RefApplicationDTO : BaseClass
{
    public RefApplicationDTO(int id, string libelle, bool estActif)
    {
        this.Id = id;
        this.Libelle = libelle;
        this.EstActif = estActif;
    }

    public string Libelle { get; set; } = null!;

    public bool EstActif { get; set; }

    public virtual ICollection<AccesApplicationDTO> AccesApplications { get; } = new List<AccesApplicationDTO>();

    public virtual ICollection<RefModuleDTO> RefModules { get; } = new List<RefModuleDTO>();
}
