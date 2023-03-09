using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Common;
using TPSecurity.Contracts.SecuWeb.UtilisateurAcces;
using Xunit;

namespace TPSecurity.Api.Tests.SecuWeb
{
    public class UtilisateurAccesMappingTest
    {
        public UtilisateurAccesMappingTest()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void UtilisateurAccesResult_ShouldMap_UtilisateurAccesResponse()
        {
            List<UtilisateurFonctionnaliteResult> utilisateurFonctionnaliteResults = 
                new List<UtilisateurFonctionnaliteResult>() { new UtilisateurFonctionnaliteResult("libelle", "Allow"), new UtilisateurFonctionnaliteResult("libelle2", "Read") };

            List<UtilisateurModuleResult> utilisateurModuleResults =
                new List<UtilisateurModuleResult>() { new UtilisateurModuleResult(1, utilisateurFonctionnaliteResults), new UtilisateurModuleResult(2, utilisateurFonctionnaliteResults) };

            List<UtilisateurApplicationResult> utilisateurApplicationResults =
                new List<UtilisateurApplicationResult>() { new UtilisateurApplicationResult(1, utilisateurModuleResults), new UtilisateurApplicationResult(2, utilisateurModuleResults) };

            List<UtilisateurAccesResult> result = new List<UtilisateurAccesResult>() { new UtilisateurAccesResult(new Guid(), utilisateurApplicationResults), new UtilisateurAccesResult(new Guid(), utilisateurApplicationResults) };

            List<UtilisateurAccesResponse> response = result.Adapt<List<UtilisateurAccesResponse>>();

            for (int i = 0; i < response.Count; i++)
            {
                response[i].IdSociete.Should().Be(result[i].IdSociete); 
                for (int j = 0; j < response[i].Applications.Count; j++)
                {
                    response[i].Applications[j].IdApplication.Should().Be(result[i].Applications[j].IdApplication);
                    for (int k = 0; k < response[i].Applications[j].Modules.Count; k++)
                    {
                        response[i].Applications[j].Modules[k].IdModule.Should().Be(result[i].Applications[j].Modules[k].IdModule);
                        for (int l = 0; l < response[i].Applications[j].Modules[k].Fonctionnalites.Count; l++)
                        {
                            response[i].Applications[j].Modules[k].Fonctionnalites[l].Libelle.Should().Be(result[i].Applications[j].Modules[k].Fonctionnalites[l].Libelle);
                            response[i].Applications[j].Modules[k].Fonctionnalites[l].Permission.Should().Be(result[i].Applications[j].Modules[k].Fonctionnalites[l].Permission);
                        }
                    }
                }
            }
        }
    }
}
