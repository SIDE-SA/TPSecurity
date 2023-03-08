using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesModule;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class AccesModuleMappingTest
    {
        public AccesModuleMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Api.AssemblyReference).Assembly);
        }

        [Fact]
        public void AccesModuleResult_ShouldMap_AccesModuleResponse()
        {
            AccesModuleResult result = new AccesModuleResult(1, true, 1, 1, "hash");

            AccesModuleResponse response = result.Adapt<AccesModuleResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.IdRefModule.Should().Be(result.IdRefModule);
            response.EstActif.Should().Be(result.EstActif);
            response.IdAccesApplication.Should().Be(result.IdAccesApplication);
        }
    }
}
