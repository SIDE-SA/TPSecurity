namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class UtilisateurDTO : BaseClass
{
    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool EstActif { get; set; }    

    public virtual ICollection<AccesUtilisateurDTO> AccesUtilisateurs { get; } = new List<AccesUtilisateurDTO>();
}
