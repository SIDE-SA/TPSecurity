using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefModuleTest
{
    public class RefModuleMappingTests
    {
        public RefModuleMappingTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Application.AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleLigneCautionAndString_ShouldMap_LigneCautionResult()
        {
            (RefModule refModule, string hashCode) poco = (RefModule.Init(1, "libelle", true, 1), "hashcode");

            RefModuleResult refModuleResult = poco.Adapt<RefModuleResult>();

            refModuleResult.Id.Should().Be(poco.refModule.Id);
            refModuleResult.Libelle.Should().Be(poco.refModule.Libelle);
            refModuleResult.EstActif.Should().Be(poco.refModule.EstActif);
            refModuleResult.IdRefApplication.Should().Be(poco.refModule.IdRefApplication);
            refModuleResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
