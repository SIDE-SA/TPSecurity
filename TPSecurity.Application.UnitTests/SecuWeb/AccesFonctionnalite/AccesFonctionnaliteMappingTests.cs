using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesFonctionnaliteTest
{
    public class AccesFonctionnaliteMappingTests
    {
        public AccesFonctionnaliteMappingTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleAccesFonctionnaliteAndString_ShouldMap_AccesFonctionnaliteResult()
        {
            (AccesFonctionnalite accesFonctionnalite, string hashCode) poco = (AccesFonctionnalite.Init(1, true, 1, 1), "hashcode");

            AccesFonctionnaliteResult accesFonctionnaliteResult = poco.Adapt<AccesFonctionnaliteResult>();

            accesFonctionnaliteResult.Id.Should().Be(poco.accesFonctionnalite.Id);
            accesFonctionnaliteResult.IdAccesModule.Should().Be(poco.accesFonctionnalite.IdAccesModule);
            accesFonctionnaliteResult.EstActif.Should().Be(poco.accesFonctionnalite.EstActif);
            accesFonctionnaliteResult.IdRefFonctionnalite.Should().Be(poco.accesFonctionnalite.IdRefFonctionnalite);
            accesFonctionnaliteResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
