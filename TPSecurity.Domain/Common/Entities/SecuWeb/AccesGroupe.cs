using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;
using static TPSecurity.Domain.Common.Errors.Errors;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class AccesGroupe : BaseClass
    {
        private AccesGroupe(int id,
                               string libelle,
                               bool estActif,
                               bool estGroupeSpecial,
                               Guid idSociete)
        {
            Id = id;
            Libelle = libelle;
            EstActif = estActif;
            EstGroupeSpecial = estGroupeSpecial;
            IdSociete = idSociete;
        }

        public int Id { get; init; }

        public string Libelle { get; private set; }

        public bool EstActif { get; private set; }

        public bool EstGroupeSpecial { get; private set; }

        public Guid IdSociete { get; private set; }

        public static ErrorOr<AccesGroupe> Create(string libelle,
                                                   bool estActif,
                                                   bool estGroupeSpecial,
                                                   Guid idSociete)
        {
            AccesGroupe accesGroupe = new AccesGroupe(0, libelle, estActif, estGroupeSpecial, idSociete);

            var validator = new AccesGroupeValidator();
            var result = validator.Validate(accesGroupe);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return accesGroupe;
        }

        public static AccesGroupe Init(int id, string libelle, bool estActif, bool estGroupeSpecial, Guid idSociete)
        {
            AccesGroupe accesGroupe = new AccesGroupe(id, libelle, estActif, estGroupeSpecial, idSociete);
            return accesGroupe;
        }

        public ErrorOr<Updated> Update(string libelle, bool estActif, bool estGroupeSpecial)
        {
            this.Libelle = libelle;
            this.EstActif = estActif;
            this.EstGroupeSpecial = estGroupeSpecial;

            var validator = new AccesGroupeValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
