using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class AccesUtilisateur
        {
            public static Error UtilisateurNotFound => Error.Conflict(code: "AccesUtilisateur.UtilisateurNotfound", description: "L'utilisateur n'existe pas");
            public static Error AccesGroupeNotFound => Error.Conflict(code: "AccesUtilisateur.AccesGroupeNotFound", description: "L'accès groupe n'existe pas");
            public static Error AccesUtilisateurAlreadyExist => Error.Conflict(code: "AccesUtilisateur.AccesUtilisateurAlreadyExist", description: "Il existe déjà une association identique de type utilisateur-groupe");
        }
    }
}
