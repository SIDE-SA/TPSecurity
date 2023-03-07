using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class RefModule : BaseClass
    {
        private RefModule(int id,
                               string libelle,
                               bool estActif,
                               int idRefApplication)
        {
            Id = id;
            Libelle = libelle;
            EstActif = estActif;
            IdRefApplication = idRefApplication;
        }

        public int Id { get; init; }

        public string Libelle { get; private set; }

        public bool EstActif { get; private set; }

        public int IdRefApplication { get; private set; }

        public static ErrorOr<RefModule> Create(string libelle,
                                                bool estActif,
                                                int idRefApplication)
        {
            RefModule refModule = new RefModule(0, libelle, estActif, idRefApplication);

            var validator = new RefModuleValidator();
            var result = validator.Validate(refModule);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return refModule;
        }

        public static RefModule Init(int id, string libelle, bool estActif, int idRefApplication)
        {
            RefModule refModule = new RefModule(id, libelle, estActif, idRefApplication);
            return refModule;
        }

        public ErrorOr<Updated> Update(string libelle, bool estActif)
        {
            this.Libelle = libelle;
            this.EstActif = estActif;

            var validator = new RefModuleValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
