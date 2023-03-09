using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;
using TPSecurity.Contracts.SecuWeb.Utilisateur;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class UtilisateurMappingTest
    {
        public UtilisateurMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void UtilisateurResult_ShouldMap_UtilisateurResponse()
        {
            UtilisateurResult result = new UtilisateurResult(1, "nom", "prenom", "email", true, "hash");

            UtilisateurResponse response = result.Adapt<UtilisateurResponse>();

            response.Identifiant.Should().Be(result.Id);
            response.Nom.Should().Be(result.Nom);
            response.Prenom.Should().Be(result.Prenom);
            response.Email.Should().Be(result.Email);
            response.EstActif.Should().Be(result.EstActif);
        }
    }
}
