using FluentAssertions;
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesGroupeTest
{
    public class AccesGroupeMappingTests
    {
        public AccesGroupeMappingTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Application.AssemblyReference).Assembly);
        }

        [Fact]
        public void TupleLigneCautionAndString_ShouldMap_LigneCautionResult()
        {
            (AccesGroupe accesGroupe, string hashCode) poco = (AccesGroupe.Init(1, "libelle", true, true, new Guid()), "hashcode");

            AccesGroupeResult accesGroupeResult = poco.Adapt<AccesGroupeResult>();

            accesGroupeResult.Id.Should().Be(poco.accesGroupe.Id);
            accesGroupeResult.Libelle.Should().Be(poco.accesGroupe.Libelle);
            accesGroupeResult.EstActif.Should().Be(poco.accesGroupe.EstActif);
            accesGroupeResult.EstGroupeSpecial.Should().Be(poco.accesGroupe.EstGroupeSpecial);
            accesGroupeResult.IdSociete.Should().Be(poco.accesGroupe.IdSociete);
            accesGroupeResult.HashCode.Should().Be(poco.hashCode);
        }
    }
}
