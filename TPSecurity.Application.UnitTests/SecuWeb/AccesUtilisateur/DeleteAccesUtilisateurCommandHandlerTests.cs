using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesUtilisateurTest
{
    public class DeleteAccesUtilisateurCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly DeleteAccesUtilisateurCommandHandler _handler;

        public DeleteAccesUtilisateurCommandHandlerTests()
        {
            _handler = new DeleteAccesUtilisateurCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteAccesUtilisateurCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteAccesUtilisateurCommand(1);
            _uow.Setup(x => x.AccesUtilisateur.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesUtilisateur.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesUtilisateur.Delete(It.IsAny<AccesUtilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }
      
        [Fact]
        public async Task DeleteAccesUtilisateurCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            AccesUtilisateur accesUtilisateur = AccesUtilisateur.Init(1, true, 1, 1);
            var command = new DeleteAccesUtilisateurCommand(1);
            _uow.Setup(x => x.AccesUtilisateur.GetById(It.IsAny<int>()))
                .Returns(accesUtilisateur);
            _uow.Setup(x => x.AccesUtilisateur.Delete(accesUtilisateur))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.AccesUtilisateur.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesUtilisateur.Delete(It.IsAny<AccesUtilisateur>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
