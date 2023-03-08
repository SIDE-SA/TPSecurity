using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesUtilisateurTest
{
    public class AccesUtilisateurMappingTests
    {
        public AccesUtilisateurMappingTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleAccesUtilisateurAndString_ShouldMap_AccesUtilisateurResult()
        {
            (AccesUtilisateur accesUtilisateur, string hashCode) poco = (AccesUtilisateur.Init(1, true, 1, 1), "hashcode");

            AccesUtilisateurResult accesUtilisateurResult = poco.Adapt<AccesUtilisateurResult>();

            accesUtilisateurResult.Id.Should().Be(poco.accesUtilisateur.Id);
            accesUtilisateurResult.IdAccesGroupe.Should().Be(poco.accesUtilisateur.IdAccesGroupe);
            accesUtilisateurResult.EstActif.Should().Be(poco.accesUtilisateur.EstActif);
            accesUtilisateurResult.IdUtilisateur.Should().Be(poco.accesUtilisateur.IdUtilisateur);
            accesUtilisateurResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
