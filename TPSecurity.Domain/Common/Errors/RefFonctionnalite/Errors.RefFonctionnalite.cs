using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class RefFonctionnalite
        {
            public static Error RefModuleNotFound => Error.Conflict(code: "RefFonctionnalite.RefModuleNotFound", description: "Le module n'existe pas");
            public static Error PermissionNotAllowed => Error.Conflict(code: "RefFonctionnalite.PermissionNotAllowed", description: "La valeur de la permission n'est pas autorisée");
        }
    }
}
