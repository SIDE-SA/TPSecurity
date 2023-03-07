using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Utilisateur
        {
            public static Error DuplicateEmail => Error.Conflict(code: "Utilisateur.DuplicateEmail", description: "Un utilisateur avec cet email existe déjà");
        }
    }
}
