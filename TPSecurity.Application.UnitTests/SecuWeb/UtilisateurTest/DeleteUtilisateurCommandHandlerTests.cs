using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.UtilisateurTest
{
    public class DeleteUtilisateurCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly DeleteUtilisateurCommandHandler _handler;

        public DeleteUtilisateurCommandHandlerTests()
        {
            _handler = new DeleteUtilisateurCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteUtilisateurCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteUtilisateurCommand(1);
            _uow.Setup(x => x.Utilisateur.GetByIdWithReferences(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.Utilisateur.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.Utilisateur.Delete(It.IsAny<Utilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteUtilisateurCommand_ShouldReturnInUse_ReferencesExists()
        {
            //Arrange
            Utilisateur utilisateur = Utilisateur.Init(1, "nom", "prenom", "email", true);
            var command = new DeleteUtilisateurCommand(1);
            _uow.Setup(x => x.Utilisateur.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(utilisateur);
            _uow.Setup(x => x.Utilisateur.Delete(utilisateur))
                .Returns(false);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.InUse.Code);
            _uow.Verify(x => x.Utilisateur.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.Utilisateur.Delete(It.IsAny<Utilisateur>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteUtilisateurCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            Utilisateur utilisateur = Utilisateur.Init(1, "nom", "prenom", "email", true);
            var command = new DeleteUtilisateurCommand(1);
            _uow.Setup(x => x.Utilisateur.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(utilisateur);
            _uow.Setup(x => x.Utilisateur.Delete(utilisateur))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.Utilisateur.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.Utilisateur.Delete(It.IsAny<Utilisateur>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
