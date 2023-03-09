using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesGroupe;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class AccesGroupeMappingTest
    {
        public AccesGroupeMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void AccesGroupeResult_ShouldMap_AccesGroupeResponse()
        {
            AccesGroupeResult result = new AccesGroupeResult(1, "libelle", true, true, new Guid(), "hash");

            AccesGroupeResponse response = result.Adapt<AccesGroupeResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.Libelle.Should().Be(result.Libelle);
            response.EstActif.Should().Be(result.EstActif);
            response.EstGroupeSpecial.Should().Be(result.EstGroupeSpecial);
            response.IdSociete.Should().Be(result.IdSociete);
        }
    }
}
