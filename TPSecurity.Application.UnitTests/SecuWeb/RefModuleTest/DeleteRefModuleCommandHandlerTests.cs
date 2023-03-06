using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefModuleTest
{
    public class DeleteRefModuleCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly DeleteRefModuleCommandHandler _handler;

        public DeleteRefModuleCommandHandlerTests()
        {
            _handler = new DeleteRefModuleCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteRefModuleCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteRefModuleCommand(1);
            _uow.Setup(x => x.RefModule.GetByIdWithReferences(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefModule.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefModule.Delete(It.IsAny<RefModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteRefModuleCommand_ShouldReturnInUse_ReferencesExists()
        {
            //Arrange
            RefModule refApplication = RefModule.Init(1, "libelle", true, null);
            var command = new DeleteRefModuleCommand(1);
            _uow.Setup(x => x.RefModule.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefModule.Delete(refApplication))
                .Returns(false);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.InUse.Code);
            _uow.Verify(x => x.RefModule.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefModule.Delete(It.IsAny<RefModule>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteRefModuleCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            RefModule refApplication = RefModule.Init(1, "libelle", true, null);
            var command = new DeleteRefModuleCommand(1);
            _uow.Setup(x => x.RefModule.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefModule.Delete(refApplication))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.RefModule.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefModule.Delete(It.IsAny<RefModule>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
