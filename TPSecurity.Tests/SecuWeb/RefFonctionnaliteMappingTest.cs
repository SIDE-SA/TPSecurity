using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Contracts.SecuWeb.RefFonctionnalite;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class RefFonctionnaliteMappingTest
    {
        public RefFonctionnaliteMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Api.AssemblyReference).Assembly);
        }

        [Fact]
        public void RefFonctionnaliteResult_ShouldMap_RefFonctionnaliteResponse()
        {
            RefFonctionnaliteResult result = new RefFonctionnaliteResult(1, "libelle", true, true, "allow", 1, "hash");

            RefFonctionnaliteResponse response = result.Adapt<RefFonctionnaliteResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.Libelle.Should().Be(result.Libelle);
            response.EstActif.Should().Be(result.EstActif);
            response.IdRefModule.Should().Be(result.IdRefModule);
            response.EstDefaut.Should().Be(result.EstDefaut);
            response.Permission.Should().Be(result.Permission);
        }
    }
}
