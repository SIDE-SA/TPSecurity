using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetByIdUtilisateur;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.UtilisateurTest
{
    public class GetByIdUtilisateurQueryHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly GetByIdUtilisateurQueryHandler _handler;

        public GetByIdUtilisateurQueryHandlerTests()
        {
            _handler = new GetByIdUtilisateurQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdUtilisateurQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetByIdUtilisateurQuery(1);
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.Utilisateur.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdUtilisateurQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetByIdUtilisateurQuery(1);
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(Utilisateur.Init(1, "nom", "prenom", "email", true));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.Utilisateur.GetById(It.IsAny<int>()), Times.Once);
        }

    }
}
