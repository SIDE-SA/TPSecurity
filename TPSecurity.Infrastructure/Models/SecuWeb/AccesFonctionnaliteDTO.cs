namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesFonctionnaliteDTO : BaseClass
{
    public AccesFonctionnaliteDTO(int id,
                   bool estActif,
                   int idAccesModule,
                   int idRefFonctionnalite)
    {
        Id = id;
        EstActif = estActif;
        IdAccesModule = idAccesModule;
        IdRefFonctionnalite = idRefFonctionnalite;
    }

    public int IdAccesModule { get; set; }

    public int IdRefFonctionnalite { get; set; }

    public bool EstActif { get; set; } 

    public virtual AccesModuleDTO IdAccesModuleNavigation { get; set; } = null!;

    public virtual RefFonctionnaliteDTO IdRefFonctionnaliteNavigation { get; set; } = null!;
}
