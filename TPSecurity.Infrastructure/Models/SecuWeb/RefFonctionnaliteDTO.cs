namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class RefFonctionnaliteDTO : BaseClass
{
    public int IdRefModule { get; set; }

    public string Libelle { get; set; } = null!;

    public bool EstDefaut { get; set; }

    public string Permission { get; set; } = null!;

    public bool EstActif { get; set; }    

    public virtual ICollection<AccesFonctionnaliteDTO> AccesFonctionnalites { get; } = new List<AccesFonctionnaliteDTO>();

    public virtual RefModuleDTO IdRefModuleNavigation { get; set; } = null!;
}
