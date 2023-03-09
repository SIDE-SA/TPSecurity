using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class AccesModule : BaseClass
    {
        private AccesModule(int id,
                       bool estActif,
                       int idAccesApplication,
                       int idRefModule,
                       IEnumerable<RefFonctionnalite>? listRefFonctionnalite = null)
        {
            Id = id;
            EstActif = estActif;
            IdAccesApplication = idAccesApplication;
            IdRefModule = idRefModule;
            ListRefFonctionnalite = listRefFonctionnalite;
        }

        public int Id { get; init; }

        public bool EstActif { get; private set; }

        public int IdAccesApplication { get; private set; }

        public int IdRefModule { get; private set; }

        public IEnumerable<RefFonctionnalite>? ListRefFonctionnalite { get; private set; }

        public static ErrorOr<AccesModule> Create(bool estActif,
                                                       int idAccesApplication,
                                                       int idRefModule)
        {
            AccesModule accesModule = new AccesModule(0, estActif, idAccesApplication, idRefModule);

            var validator = new AccesModuleValidator();
            var result = validator.Validate(accesModule);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return accesModule;
        }

        public static AccesModule Init(int id, bool estActif, int idAccesApplication, int idRefModule, IEnumerable<RefFonctionnalite>? listRefFonctionnalite = null)
        {
            AccesModule accesModule = new AccesModule(id, estActif, idAccesApplication, idRefModule, listRefFonctionnalite);
            return accesModule;
        }

        public ErrorOr<Updated> Update(bool estActif)
        {
            this.EstActif = estActif;

            var validator = new AccesModuleValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
