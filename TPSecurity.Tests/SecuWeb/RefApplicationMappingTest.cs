using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;
using TPSecurity.Contracts.SecuWeb.RefApplication;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class RefApplicationMappingTest
    {
        public RefApplicationMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void RefApplicationResult_ShouldMap_RefApplicationResponse()
        {
            RefApplicationResult result = new RefApplicationResult(1, "libelle", true, "hash");

            RefApplicationResponse response = result.Adapt<RefApplicationResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.Libelle.Should().Be(result.Libelle);
            response.EstActif.Should().Be(result.EstActif);
        }
    }
}
