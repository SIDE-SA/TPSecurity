namespace TPSecurity.Infrastructure.Models.SecuWeb;

public partial class AccesUtilisateurDTO : BaseClass
{
    public AccesUtilisateurDTO(int id,
                       bool estActif,
                       int idAccesGroupe,
                       int idUtilisateur)
    {
        Id = id;
        EstActif = estActif;
        IdAccesGroupe = idAccesGroupe;
        IdUtilisateur = idUtilisateur;
    }

    public int IdUtilisateur { get; set; }

    public int IdAccesGroupe { get; set; }

    public bool EstActif { get; set; }  

    public virtual AccesGroupeDTO IdAccesGroupeNavigation { get; set; } = null!;

    public virtual UtilisateurDTO IdUtilisateurNavigation { get; set; } = null!;
}
