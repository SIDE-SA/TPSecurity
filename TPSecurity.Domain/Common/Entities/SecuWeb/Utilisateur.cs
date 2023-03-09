using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class Utilisateur : BaseClass
    {
        private Utilisateur(int id,
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

        public int Id { get; init; }

        public string Nom { get; private set; }

        public string Prenom { get; private set; }

        public string Email { get; private set; }

        public bool EstActif { get; private set; }

        public static ErrorOr<Utilisateur> Create(string nom,
                                                   string prenom,
                                                   string email,
                                                   bool estActif)
        {
            Utilisateur utilisateur = new Utilisateur(0, nom, prenom, email, estActif);

            var validator = new UtilisateurValidator();
            var result = validator.Validate(utilisateur);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return utilisateur;
        }

        public static Utilisateur Init(int id, string nom, string prenom, string email, bool estActif)
        {
            Utilisateur utilisateur = new Utilisateur(id, nom, prenom, email, estActif);
            return utilisateur;
        }

        public ErrorOr<Updated> Update(string nom, string prenom, string email, bool estActif)
        {
            this.Nom = nom;
            this.Prenom = prenom;
            this.Email = email;
            this.EstActif = estActif;

            var validator = new UtilisateurValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
