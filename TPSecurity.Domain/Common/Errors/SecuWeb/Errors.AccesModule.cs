using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class AccesModule
        {
            public static Error RefModuleNotFound => Error.Conflict(code: "AccesModule.RefModuleNotfound", description: "Le module n'existe pas");
            public static Error AccesApplicationNotFound => Error.Conflict(code: "AccesModule.AccesApplicationNotFound", description: "L'accès application n'existe pas");
            public static Error AccesModuleAlreadyExist => Error.Conflict(code: "AccesModule.AccesModuleAlreadyExist", description: "Il existe déjà une association identique de type application-module");
        }
    }
}
