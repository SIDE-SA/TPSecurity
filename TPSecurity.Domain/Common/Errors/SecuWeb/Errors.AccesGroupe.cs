using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class AccesGroupe
        {
            public static Error SocieteNotFound => Error.Conflict(code: "AccesGroupe.SocieteNotFound", description: "La société n'existe pas");
        }
    }
}
