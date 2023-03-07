using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Contracts.SecuWeb.RefModule;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class RefModuleMappingTest
    {
        public RefModuleMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Api.AssemblyReference).Assembly);
        }

        [Fact]
        public void RefModuleResult_ShouldMap_RefModuleResponse()
        {
            RefModuleResult result = new RefModuleResult(1, "libelle", true, 1, "hash");

            RefModuleResponse response = result.Adapt<RefModuleResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.Libelle.Should().Be(result.Libelle);
            response.EstActif.Should().Be(result.EstActif);
            response.IdRefApplication.Should().Be(result.IdRefApplication);
        }
    }
}
