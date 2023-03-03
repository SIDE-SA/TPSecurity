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
                               RefApplication refApplication)
        {
            Id = id;
            Libelle = libelle;
            EstActif = estActif;
            IdRefApplication = refApplication;
        }

        public int Id { get; init; }

        public string Libelle { get; private set; }

        public bool EstActif { get; private set; }
        public RefApplication IdRefApplication { get; private set; } = null!;

        public static ErrorOr<RefModule> Create(string libelle,
                                                bool estActif,
                                                RefApplication refApplication)
        {
            RefModule refModule = new RefModule(0, libelle, estActif, refApplication);

            var validator = new RefModuleValidator();
            var result = validator.Validate(refModule);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return refModule;
        }

        public static RefModule Init(int id, string libelle, bool estActif, RefApplication refApplication)
        {
            RefModule refModule = new RefModule(id, libelle, estActif, refApplication);
            return refModule;
        }

        public ErrorOr<Updated> Update(string Libelle, bool estActif)
        {
            this.Libelle = Libelle;
            this.EstActif = estActif;

            var validator = new RefModuleValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
