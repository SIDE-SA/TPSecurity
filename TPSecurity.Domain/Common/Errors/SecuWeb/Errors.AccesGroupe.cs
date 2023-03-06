using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class AccesGroupe
        {
            public static Error SocietyNotFound => Error.Conflict(code: "AccesGroupe.SocietyNotFound", description: "La société n'existe pas");
        }
    }
}
