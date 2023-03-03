namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesGroupeDTO : BaseClass
{
    public Guid IdSociete { get; set; }

    public string Libelle { get; set; } = null!;

    public bool EstGroupeSpecial { get; set; }

    public string EstActif { get; set; } = null!;    

    public virtual ICollection<AccesApplicationDTO> AccesApplications { get; } = new List<AccesApplicationDTO>();

    public virtual ICollection<AccesUtilisateurDTO> AccesUtilisateurs { get; } = new List<AccesUtilisateurDTO>();
}
