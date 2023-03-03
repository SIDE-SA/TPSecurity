namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesModuleDTO : BaseClass
{    
    public int IdAccesApplication { get; set; }

    public int IdRefModule { get; set; }

    public string EstActif { get; set; } = null!;   

    public virtual ICollection<AccesFonctionnaliteDTO> AccesFonctionnalites { get; } = new List<AccesFonctionnaliteDTO>();

    public virtual AccesApplicationDTO IdAccesApplicationNavigation { get; set; } = null!;

    public virtual RefModuleDTO IdRefModuleNavigation { get; set; } = null!;
}
