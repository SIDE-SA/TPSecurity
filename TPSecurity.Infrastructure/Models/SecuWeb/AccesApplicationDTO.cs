namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesApplicationDTO : BaseClass
{
    public AccesApplicationDTO(int id, bool estActif, int idAccesGroupe, int idRefApplication)
    {
        this.Id = id;
        this.EstActif = estActif;
        this.IdAccesGroupe = idAccesGroupe;
        this.IdRefApplication = idRefApplication;
    }
    public int IdAccesGroupe { get; set; }

    public int IdRefApplication { get; set; }

    public bool EstActif { get; set; }   

    public virtual ICollection<AccesModuleDTO> AccesModules { get; } = new List<AccesModuleDTO>();

    public virtual AccesGroupeDTO IdAccesGroupeNavigation { get; set; } = null!;

    public virtual RefApplicationDTO IdRefApplicationNavigation { get; set; } = null!;
}
