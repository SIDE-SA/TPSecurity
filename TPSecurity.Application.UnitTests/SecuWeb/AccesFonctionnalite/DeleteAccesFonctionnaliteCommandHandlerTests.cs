using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesFonctionnaliteTest
{
    public class DeleteAccesFonctionnaliteCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly DeleteAccesFonctionnaliteCommandHandler _handler;

        public DeleteAccesFonctionnaliteCommandHandlerTests()
        {
            _handler = new DeleteAccesFonctionnaliteCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteAccesFonctionnaliteCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteAccesFonctionnaliteCommand(1);
            _uow.Setup(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesFonctionnalite.Delete(It.IsAny<AccesFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }
      
        [Fact]
        public async Task DeleteAccesFonctionnaliteCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            AccesFonctionnalite accesFonctionnalite = AccesFonctionnalite.Init(1, true, 1, 1);
            var command = new DeleteAccesFonctionnaliteCommand(1);
            _uow.Setup(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(accesFonctionnalite);
            _uow.Setup(x => x.AccesFonctionnalite.Delete(accesFonctionnalite))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesFonctionnalite.Delete(It.IsAny<AccesFonctionnalite>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
