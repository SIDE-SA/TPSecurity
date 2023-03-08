using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesModuleTest
{
    public class DeleteAccesModuleCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly DeleteAccesModuleCommandHandler _handler;

        public DeleteAccesModuleCommandHandlerTests()
        {
            _handler = new DeleteAccesModuleCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteAccesModuleCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteAccesModuleCommand(1);
            _uow.Setup(x => x.AccesModule.GetByIdWithReferences(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesModule.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesModule.Delete(It.IsAny<AccesModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteAccesModuleCommand_ShouldReturnInUse_ReferencesExists()
        {
            //Arrange
            AccesModule accesModule = AccesModule.Init(1, true, 1, 1);
            var command = new DeleteAccesModuleCommand(1);
            _uow.Setup(x => x.AccesModule.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(accesModule);
            _uow.Setup(x => x.AccesModule.Delete(accesModule))
                .Returns(false);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.InUse.Code);
            _uow.Verify(x => x.AccesModule.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesModule.Delete(It.IsAny<AccesModule>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteAccesModuleCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            AccesModule accesModule = AccesModule.Init(1, true, 1, 1);
            var command = new DeleteAccesModuleCommand(1);
            _uow.Setup(x => x.AccesModule.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(accesModule);
            _uow.Setup(x => x.AccesModule.Delete(accesModule))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.AccesModule.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesModule.Delete(It.IsAny<AccesModule>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
