using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.UtilisateurAccesTest
{
    public class MappingUtilisateurAccesTests
    {
        public MappingUtilisateurAccesTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void AccesGroupe_ShouldMap_UtilisateurAccesResult()
        {
            AccesGroupe pocoGroupe = AccesGroupe.Init(1, "nom", true, true , new Guid(), new List<AccesApplication>());
            UtilisateurAccesResult utilisateurResult = pocoGroupe.Adapt<UtilisateurAccesResult>();
            utilisateurResult.IdSociete.Should().Be(pocoGroupe.IdSociete);

            AccesApplication pocoApplication = AccesApplication.Init(1, true, 1, 1, new List<AccesModule>());
            UtilisateurApplicationResult applicationResult = pocoApplication.Adapt<UtilisateurApplicationResult>();
            applicationResult.IdApplication.Should().Be(pocoApplication.Id);

            AccesModule pocoModule = AccesModule.Init(1, true, 1, 1, new List<RefFonctionnalite>());
            UtilisateurModuleResult moduleResult = pocoModule.Adapt<UtilisateurModuleResult>();
            moduleResult.IdModule.Should().Be(pocoModule.Id);

            RefFonctionnalite pocoFonctionnalite = RefFonctionnalite.Init(1, "nom", true, true, "Allow", 1);
            UtilisateurFonctionnaliteResult fonctionnaliteResult = pocoFonctionnalite.Adapt<UtilisateurFonctionnaliteResult>();
            fonctionnaliteResult.Libelle.Should().Be(pocoFonctionnalite.Libelle);
            fonctionnaliteResult.Permission.Should().Be(pocoFonctionnalite.Permission);
        }
    }
}
