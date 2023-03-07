namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class UtilisateurDTO : BaseClass
{
    public UtilisateurDTO(int id,
                       string nom,
                       string prenom,
                       string email,
                       bool estActif)
    {
        Id = id;
        Nom = nom;
        Prenom = prenom;
        Email = email;
        EstActif = estActif;
    }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool EstActif { get; set; }    

    public virtual ICollection<AccesUtilisateurDTO> AccesUtilisateurs { get; } = new List<AccesUtilisateurDTO>();
}
