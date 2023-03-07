using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesApplicationTest
{
    public class AccesApplicationMappingTests
    {
        public AccesApplicationMappingTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleAccesApplicationAndString_ShouldMap_AccesApplicationResult()
        {
            (AccesApplication accesApplication, string hashCode) poco = (AccesApplication.Init(1, true, 1, 1), "hashcode");

            AccesApplicationResult accesApplicationResult = poco.Adapt<AccesApplicationResult>();

            accesApplicationResult.Id.Should().Be(poco.accesApplication.Id);
            accesApplicationResult.IdAccesGroupe.Should().Be(poco.accesApplication.IdAccesGroupe);
            accesApplicationResult.EstActif.Should().Be(poco.accesApplication.EstActif);
            accesApplicationResult.IdRefApplication.Should().Be(poco.accesApplication.IdRefApplication);
            accesApplicationResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
