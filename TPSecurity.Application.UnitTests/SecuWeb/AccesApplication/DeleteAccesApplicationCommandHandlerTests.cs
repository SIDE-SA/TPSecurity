using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesApplicationTest
{
    public class DeleteAccesApplicationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly DeleteAccesApplicationCommandHandler _handler;

        public DeleteAccesApplicationCommandHandlerTests()
        {
            _handler = new DeleteAccesApplicationCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteAccesApplicationCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteAccesApplicationCommand(1);
            _uow.Setup(x => x.AccesApplication.GetByIdWithReferences(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesApplication.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesApplication.Delete(It.IsAny<AccesApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteAccesApplicationCommand_ShouldReturnInUse_ReferencesExists()
        {
            //Arrange
            AccesApplication refApplication = AccesApplication.Init(1, true, 1, 1);
            var command = new DeleteAccesApplicationCommand(1);
            _uow.Setup(x => x.AccesApplication.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.AccesApplication.Delete(refApplication))
                .Returns(false);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.InUse.Code);
            _uow.Verify(x => x.AccesApplication.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesApplication.Delete(It.IsAny<AccesApplication>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteAccesApplicationCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            AccesApplication refApplication = AccesApplication.Init(1, true, 1, 1);
            var command = new DeleteAccesApplicationCommand(1);
            _uow.Setup(x => x.AccesApplication.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.AccesApplication.Delete(refApplication))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.AccesApplication.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesApplication.Delete(It.IsAny<AccesApplication>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
