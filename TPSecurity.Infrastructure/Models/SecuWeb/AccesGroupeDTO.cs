namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesGroupeDTO : BaseClass
{
    public AccesGroupeDTO(int id, string libelle, bool estActif, bool estGroupeSpecial, Guid idSociete)
    {
        this.Id = id;
        this.Libelle = libelle;
        this.EstActif = estActif;
        this.EstGroupeSpecial = estGroupeSpecial;
        this.IdSociete = idSociete;
    }

    public Guid IdSociete { get; set; }

    public string Libelle { get; set; } = null!;

    public bool EstGroupeSpecial { get; set; }

    public bool EstActif { get; set; }   

    public virtual ICollection<AccesApplicationDTO> AccesApplications { get; } = new List<AccesApplicationDTO>();

    public virtual ICollection<AccesUtilisateurDTO> AccesUtilisateurs { get; } = new List<AccesUtilisateurDTO>();
}
