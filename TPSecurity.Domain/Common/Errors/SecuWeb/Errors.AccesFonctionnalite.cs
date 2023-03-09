using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class AccesFonctionnalite
        {
            public static Error RefFonctionnaliteNotFound => Error.Conflict(code: "AccesFonctionnalite.RefFonctionnaliteNotfound", description: "La fonctionnalité n'existe pas");
            public static Error AccesModuleNotFound => Error.Conflict(code: "AccesFonctionnalite.AccesModuleNotFound", description: "L'accès module n'existe pas");
            public static Error AccesFonctionnaliteAlreadyExist => Error.Conflict(code: "AccesFonctionnalite.AccesFonctionnaliteAlreadyExist", description: "Il existe déjà une association identique de type module-fonctionnalté");
        }
    }
}
