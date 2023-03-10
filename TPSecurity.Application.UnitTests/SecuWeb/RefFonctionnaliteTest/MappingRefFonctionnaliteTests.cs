using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefFonctionnaliteTest
{
    public class MappingRefFonctionnaliteTests
    {
        public MappingRefFonctionnaliteTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleRefFonctionnaliteAndString_ShouldMap_RefFonctionnaliteResult()
        {
            (RefFonctionnalite refFonctionnalite, string hashCode) poco = (RefFonctionnalite.Init(1, "libelle", true, true, "permission", 1), "hashcode");

            RefFonctionnaliteResult refFonctionnaliteResult = poco.Adapt<RefFonctionnaliteResult>();

            refFonctionnaliteResult.Id.Should().Be(poco.refFonctionnalite.Id);
            refFonctionnaliteResult.Libelle.Should().Be(poco.refFonctionnalite.Libelle);
            refFonctionnaliteResult.EstActif.Should().Be(poco.refFonctionnalite.EstActif);
            refFonctionnaliteResult.Permission.Should().Be(poco.refFonctionnalite.Permission);
            refFonctionnaliteResult.EstDefaut.Should().Be(poco.refFonctionnalite.EstDefaut);
            refFonctionnaliteResult.IdRefModule.Should().Be(poco.refFonctionnalite.IdRefModule);
            refFonctionnaliteResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
