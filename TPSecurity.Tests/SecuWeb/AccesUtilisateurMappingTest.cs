using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesUtilisateur;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class AccesUtilisateurMappingTest
    {
        public AccesUtilisateurMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Api.AssemblyReference).Assembly);
        }

        [Fact]
        public void AccesUtilisateurResult_ShouldMap_AccesUtilisateurResponse()
        {
            AccesUtilisateurResult result = new AccesUtilisateurResult(1, true, 1, 1, "hash");

            AccesUtilisateurResponse response = result.Adapt<AccesUtilisateurResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.IdUtilisateur.Should().Be(result.IdUtilisateur);
            response.EstActif.Should().Be(result.EstActif);
            response.IdAccesGroupe.Should().Be(result.IdAccesGroupe);
        }
    }
}
