using ErrorOr;
using TPSecurity.Domain.Common.Validation;
using TPSecurity.Domain.Common.Validation.SecuWeb;

namespace TPSecurity.Domain.Common.Entities.SecuWeb
{
    public sealed class RefFonctionnalite : BaseClass
    {
        private RefFonctionnalite(int id,
                               string libelle,
                               bool estActif,
                               bool estDefaut,
                               string permission,
                               int idRefModule)
        {
            Id = id;
            Libelle = libelle;
            EstActif = estActif;
            EstDefaut = estDefaut;
            Permission = permission;
            IdRefModule = idRefModule;
        }

        public int Id { get; init; }

        public string Libelle { get; private set; }

        public bool EstActif { get; private set; }

        public int IdRefModule { get; init; }

        public string Permission { get; private set; }

        public bool EstDefaut { get; private set; }

        public static ErrorOr<RefFonctionnalite> Create(string libelle,
                                                       bool estActif,
                                                       bool estDefaut,
                                                       string permission,
                                                       int idRefModule)
        {
            RefFonctionnalite refFonctionnalite = new RefFonctionnalite(0, libelle, estActif, estDefaut, permission, idRefModule);

            var validator = new RefFonctionnaliteValidator();
            var result = validator.Validate(refFonctionnalite);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return refFonctionnalite;
        }

        public static RefFonctionnalite Init(int id, string libelle, bool estActif, bool estDefaut, string permission, int idRefModule)
        {
            RefFonctionnalite refFonctionnalite = new RefFonctionnalite(id, libelle, estActif, estDefaut, permission, idRefModule);
            return refFonctionnalite;
        }

        public ErrorOr<Updated> Update(string Libelle, bool estActif, bool estDefaut)
        {
            this.Libelle = Libelle;
            this.EstActif = estActif;
            this.EstDefaut = estDefaut;

            var validator = new RefFonctionnaliteValidator();
            var result = validator.Validate(this);
            if (!result.IsValid)
                return ValidationHelper.GenerateErrorList(result);

            return Result.Updated;
        }
    }
}
