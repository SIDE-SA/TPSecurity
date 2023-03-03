namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesFonctionnaliteDTO : BaseClass
{

    public int IdAccesModule { get; set; }

    public int IdRefFonctionnalite { get; set; }

    public string EstActif { get; set; } = null!;    

    public virtual AccesModuleDTO IdAccesModuleNavigation { get; set; } = null!;

    public virtual RefFonctionnaliteDTO IdRefFonctionnaliteNavigation { get; set; } = null!;
}
