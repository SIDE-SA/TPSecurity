using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefApplicationTest
{
    public class DeleteRefApplicationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly DeleteRefApplicationCommandHandler _handler;

        public DeleteRefApplicationCommandHandlerTests()
        {
            _handler = new DeleteRefApplicationCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteRefApplicationCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteRefApplicationCommand(1);
            _uow.Setup(x => x.RefApplication.GetByIdWithReferences(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefApplication.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefApplication.Delete(It.IsAny<RefApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteRefApplicationCommand_ShouldReturnInUse_ReferencesExists()
        {
            //Arrange
            RefApplication refApplication = RefApplication.Init(1, "libelle", true);
            var command = new DeleteRefApplicationCommand(1);
            _uow.Setup(x => x.RefApplication.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefApplication.Delete(refApplication))
                .Returns(false);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.InUse.Code);
            _uow.Verify(x => x.RefApplication.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefApplication.Delete(It.IsAny<RefApplication>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteRefApplicationCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            RefApplication refApplication = RefApplication.Init(1, "libelle", true);
            var command = new DeleteRefApplicationCommand(1);
            _uow.Setup(x => x.RefApplication.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefApplication.Delete(refApplication))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.RefApplication.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefApplication.Delete(It.IsAny<RefApplication>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
