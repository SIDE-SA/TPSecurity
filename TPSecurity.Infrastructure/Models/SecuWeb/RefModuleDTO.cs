namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class RefModuleDTO : BaseClass
{
    public RefModuleDTO(int id, string libelle, bool estActif, int idRefApplication)
    {
        this.Id = id;
        this.Libelle = libelle;
        this.EstActif = estActif;
        this.IdRefApplication = idRefApplication;  
    }

    public int IdRefApplication { get; set; }

    public string Libelle { get; set; } = null!;

    public bool EstActif { get; set; }   

    public virtual ICollection<AccesModuleDTO> AccesModules { get; } = new List<AccesModuleDTO>();

    public virtual RefApplicationDTO IdRefApplicationNavigation { get; set; } = null!;

    public virtual ICollection<RefFonctionnaliteDTO> RefFonctionnalites { get; } = new List<RefFonctionnaliteDTO>();
}
