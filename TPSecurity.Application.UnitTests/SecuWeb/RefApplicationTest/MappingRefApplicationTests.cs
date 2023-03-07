using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefApplicationTest
{
    public class MappingRefApplicationTests
    {
        public MappingRefApplicationTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleRefApplicationAndString_ShouldMap_RefApplicationResult()
        {
            (RefApplication refApplication, string hashCode) poco = (RefApplication.Init(1, "libelle", true), "hashcode");

            RefApplicationResult refApplicationResult = poco.Adapt<RefApplicationResult>();

            refApplicationResult.Id.Should().Be(poco.refApplication.Id);
            refApplicationResult.Libelle.Should().Be(poco.refApplication.Libelle);
            refApplicationResult.EstActif.Should().Be(poco.refApplication.EstActif);
            refApplicationResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
