namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesApplicationDTO : BaseClass
{
    public int IdAccesGroupe { get; set; }

    public int IdRefApplication { get; set; }

    public string EstActif { get; set; } = null!;   

    public virtual ICollection<AccesModuleDTO> AccesModules { get; } = new List<AccesModuleDTO>();

    public virtual AccesGroupeDTO IdAccesGroupeNavigation { get; set; } = null!;

    public virtual RefApplicationDTO IdRefApplicationNavigation { get; set; } = null!;
}
