namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesModuleDTO : BaseClass
{
    public AccesModuleDTO(int id,
                       bool estActif,
                       int idAccesApplication,
                       int idRefModule)
    {
        Id = id;
        EstActif = estActif;
        IdAccesApplication = idAccesApplication;
        IdRefModule = idRefModule;
    }

    public int IdAccesApplication { get; set; }

    public int IdRefModule { get; set; }

    public bool EstActif { get; set; }   

    public virtual ICollection<AccesFonctionnaliteDTO> AccesFonctionnalites { get; } = new List<AccesFonctionnaliteDTO>();

    public virtual AccesApplicationDTO IdAccesApplicationNavigation { get; set; } = null!;

    public virtual RefModuleDTO IdRefModuleNavigation { get; set; } = null!;
}
