using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesFonctionnalite;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class AccesFonctionnaliteMappingTest
    {
        public AccesFonctionnaliteMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Api.AssemblyReference).Assembly);
        }

        [Fact]
        public void AccesFonctionnaliteResult_ShouldMap_AccesFonctionnaliteResponse()
        {
            AccesFonctionnaliteResult result = new AccesFonctionnaliteResult(1, true, 1, 1, "hash");

            AccesFonctionnaliteResponse response = result.Adapt<AccesFonctionnaliteResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.IdRefFonctionnalite.Should().Be(result.IdRefFonctionnalite);
            response.EstActif.Should().Be(result.EstActif);
            response.IdAccesModule.Should().Be(result.IdAccesModule);
        }
    }
}
