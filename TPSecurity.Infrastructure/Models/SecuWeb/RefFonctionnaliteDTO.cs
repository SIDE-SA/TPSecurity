namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class RefFonctionnaliteDTO : BaseClass
{
    public RefFonctionnaliteDTO(int id, string libelle, bool estActif, bool estDefaut, string permission, int idRefModule)
    {
        this.Id = id;
        this.Libelle = libelle;
        this.EstActif = estActif;
        this.IdRefModule = idRefModule;
        this.Permission = permission;
        this.EstDefaut = estDefaut;
    }

    public int IdRefModule { get; set; }

    public string Libelle { get; set; } = null!;

    public bool EstDefaut { get; set; }

    public string Permission { get; set; } = null!;

    public bool EstActif { get; set; }    

    public virtual ICollection<AccesFonctionnaliteDTO> AccesFonctionnalites { get; } = new List<AccesFonctionnaliteDTO>();

    public virtual RefModuleDTO IdRefModuleNavigation { get; set; } = null!;
}
