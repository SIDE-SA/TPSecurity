using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class AccesApplication
        {
            public static Error RefApplicationNotFound => Error.Conflict(code: "AccesApplication.RefApplicationNotfound", description: "L'application n'existe pas");
            public static Error AccesGroupeNotFound => Error.Conflict(code: "AccesApplication.AccesGroupeNotFound", description: "L'accès groupe n'existe pas");
            public static Error AccesApplicationAlreadyExist => Error.Conflict(code: "AccesApplication.AccesApplicationAlreadyExist", description: "Il existe déjà une association identique de type groupe-application");
        }
    }
}
