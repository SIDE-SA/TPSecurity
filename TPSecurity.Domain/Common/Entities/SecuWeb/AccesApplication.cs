using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class AccesApplication : BaseClass
    {
        private AccesApplication(int id,
                               bool estActif,
                               int idAccesGroupe,
                               int idRefApplication,
                               IEnumerable<AccesModule>? listAccesModule = null)
        {
            Id = id;
            EstActif = estActif;
            IdAccesGroupe = idAccesGroupe;
            IdRefApplication = idRefApplication;
            ListAccesModule = listAccesModule;
        }

        public int Id { get; init; }

        public bool EstActif { get; private set; }

        public int IdAccesGroupe { get; private set; }

        public int IdRefApplication { get; private set; }

        public IEnumerable<AccesModule>? ListAccesModule { get; private set; }

        public static ErrorOr<AccesApplication> Create(bool estActif,
                                                       int idAccesGroupe,
                                                       int idRefApplication)
        {
            AccesApplication accesApplication = new AccesApplication(0, estActif, idAccesGroupe, idRefApplication);

            var validator = new AccesApplicationValidator();
            var result = validator.Validate(accesApplication);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return accesApplication;
        }

        public static AccesApplication Init(int id, bool estActif, int idAccesGroupe, int idRefApplication, IEnumerable<AccesModule>? listAccesModule = null)
        {
            AccesApplication accesApplication = new AccesApplication(id, estActif, idAccesGroupe, idRefApplication, listAccesModule);
            return accesApplication;
        }

        public ErrorOr<Updated> Update(bool estActif)
        {
            this.EstActif = estActif;

            var validator = new AccesApplicationValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
