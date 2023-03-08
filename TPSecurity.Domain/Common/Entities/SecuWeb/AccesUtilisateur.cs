using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class AccesUtilisateur : BaseClass
    {
        private AccesUtilisateur(int id,
                               bool estActif,
                               int idAccesGroupe,
                               int idUtilisateur)
        {
            Id = id;
            EstActif = estActif;
            IdAccesGroupe = idAccesGroupe;
            IdUtilisateur = idUtilisateur;
        }

        public int Id { get; init; }

        public bool EstActif { get; private set; }

        public int IdAccesGroupe { get; private set; }

        public int IdUtilisateur { get; private set; }

        public static ErrorOr<AccesUtilisateur> Create(bool estActif,
                                                       int idAccesGroupe,
                                                       int idUtilisateur)
        {
            AccesUtilisateur accesUtilisateur = new AccesUtilisateur(0, estActif, idAccesGroupe, idUtilisateur);

            var validator = new AccesUtilisateurValidator();
            var result = validator.Validate(accesUtilisateur);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return accesUtilisateur;
        }

        public static AccesUtilisateur Init(int id, bool estActif, int idAccesGroupe, int idUtilisateur)
        {
            AccesUtilisateur accesUtilisateur = new AccesUtilisateur(id, estActif, idAccesGroupe, idUtilisateur);
            return accesUtilisateur;
        }

        public ErrorOr<Updated> Update(bool estActif)
        {
            this.EstActif = estActif;

            var validator = new AccesUtilisateurValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
