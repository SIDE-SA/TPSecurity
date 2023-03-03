using ErrorOr;

namespace TPSecurity.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static Error NotFound => Error.NotFound(code: "NotFound", description: "Donnee non trouvée.");
        public static Error Concurrency => Error.Conflict(code: "Concurrency", description: "Données modifiées entre temps");
        public static Error InUse => Error.Conflict(code: "In Use", description: "Données référencées par un autre concept");
        public static Error DBError => Error.Failure(code: "DB error", description: "Une erreur est survenue lors de la sauvegarde en base de données");

    }
}
