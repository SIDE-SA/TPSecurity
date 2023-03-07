using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class RefApplication : BaseClass
    {
        private RefApplication(int id,
                               string libelle,
                               bool estActif)
        {
            Id = id;
            Libelle = libelle;
            EstActif = estActif;
        }

        public int Id { get; init; }

        public string Libelle { get; private set; }

        public bool EstActif { get; private set; }        

        public static ErrorOr<RefApplication> Create(string libelle,
                                                     bool estActif)
        {
            RefApplication refApplication = new RefApplication(0, libelle, estActif);

            var validator = new RefApplicationValidator();
            var result = validator.Validate(refApplication);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return refApplication;
        }

        public static RefApplication Init(int id, string libelle, bool estActif)
        {
            RefApplication refApplication = new RefApplication(id, libelle, estActif);
            return refApplication;
        }

        public ErrorOr<Updated> Update(string libelle, bool estActif)
        {
            this.Libelle = libelle;
            this.EstActif = estActif;

            var validator = new RefApplicationValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
