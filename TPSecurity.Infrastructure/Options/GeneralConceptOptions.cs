namespace TPSecurity.Infrastructure.Options
{
    public class GeneralConceptOptions
    {
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Version[] Versions { get; set; } = null!;

    }

    public class Version
    {
        public string Name { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public Concepts Concepts { get; set; } = null!;
    }

    public class Concepts
    {
        public string Banque { get; set; } = null!;
        public string Civilite { get; set; } = null!;
        public string Societe { get; set; } = null!;
    }
}
