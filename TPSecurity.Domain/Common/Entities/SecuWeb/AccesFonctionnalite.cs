using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class AccesFonctionnalite : BaseClass
    {
        private AccesFonctionnalite(int id,
                               bool estActif,
                               int idAccesModule,
                               int idRefFonctionnalite)
        {
            Id = id;
            EstActif = estActif;
            IdAccesModule = idAccesModule;
            IdRefFonctionnalite = idRefFonctionnalite;
        }

        public int Id { get; init; }

        public bool EstActif { get; private set; }

        public int IdAccesModule { get; private set; }

        public int IdRefFonctionnalite { get; private set; }

        public static ErrorOr<AccesFonctionnalite> Create(bool estActif,
                                                       int idAccesModule,
                                                       int idRefFonctionnalite)
        {
            AccesFonctionnalite accesFonctionnalite = new AccesFonctionnalite(0, estActif, idAccesModule, idRefFonctionnalite);

            var validator = new AccesFonctionnaliteValidator();
            var result = validator.Validate(accesFonctionnalite);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return accesFonctionnalite;
        }

        public static AccesFonctionnalite Init(int id, bool estActif, int idAccesModule, int idRefFonctionnalite)
        {
            AccesFonctionnalite accesFonctionnalite = new AccesFonctionnalite(id, estActif, idAccesModule, idRefFonctionnalite);
            return accesFonctionnalite;
        }

        public ErrorOr<Updated> Update(bool estActif)
        {
            this.EstActif = estActif;

            var validator = new AccesFonctionnaliteValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
