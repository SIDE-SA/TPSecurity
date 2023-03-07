using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.UtilisateurTest
{
    public class MappingUtilisateurTests
    {
        public MappingUtilisateurTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleUtilisateurAndString_ShouldMap_UtilisateurResult()
        {
            (Utilisateur utilisateur, string hashCode) poco = (Utilisateur.Init(1, "nom", "prenom", "email", true), "hashcode");

            UtilisateurResult utilisateurResult = poco.Adapt<UtilisateurResult>();

            utilisateurResult.Id.Should().Be(poco.utilisateur.Id);
            utilisateurResult.Nom.Should().Be(poco.utilisateur.Nom);
            utilisateurResult.Prenom.Should().Be(poco.utilisateur.Prenom);
            utilisateurResult.Email.Should().Be(poco.utilisateur.Email);
            utilisateurResult.EstActif.Should().Be(poco.utilisateur.EstActif);
            utilisateurResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
