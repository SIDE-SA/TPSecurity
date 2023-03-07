using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesModuleTest
{
    public class AccesModuleMappingTests
    {
        public AccesModuleMappingTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleAccesModuleAndString_ShouldMap_AccesModuleResult()
        {
            (AccesModule accesModule, string hashCode) poco = (AccesModule.Init(1, true, 1, 1), "hashcode");

            AccesModuleResult accesModuleResult = poco.Adapt<AccesModuleResult>();

            accesModuleResult.Id.Should().Be(poco.accesModule.Id);
            accesModuleResult.IdAccesApplication.Should().Be(poco.accesModule.IdAccesApplication);
            accesModuleResult.EstActif.Should().Be(poco.accesModule.EstActif);
            accesModuleResult.IdRefModule.Should().Be(poco.accesModule.IdRefModule);
            accesModuleResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
