using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesApplication;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class AccesApplicationMappingTest
    {
        public AccesApplicationMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void AccesApplicationResult_ShouldMap_AccesApplicationResponse()
        {
            AccesApplicationResult result = new AccesApplicationResult(1, true, 1, 1, "hash");

            AccesApplicationResponse response = result.Adapt<AccesApplicationResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.IdRefApplication.Should().Be(result.IdRefApplication);
            response.EstActif.Should().Be(result.EstActif);
            response.IdAccesGroupe.Should().Be(result.IdAccesGroupe);
        }
    }
}
