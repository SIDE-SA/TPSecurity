using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefFonctionnaliteTest
{
    public class DeleteRefFonctionnaliteCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly DeleteRefFonctionnaliteCommandHandler _handler;

        public DeleteRefFonctionnaliteCommandHandlerTests()
        {
            _handler = new DeleteRefFonctionnaliteCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteRefFonctionnaliteCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteRefFonctionnaliteCommand(1);
            _uow.Setup(x => x.RefFonctionnalite.GetByIdWithReferences(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefFonctionnalite.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefFonctionnalite.Delete(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteRefFonctionnaliteCommand_ShouldReturnInUse_ReferencesExists()
        {
            //Arrange
            RefFonctionnalite refFonctionnalite = RefFonctionnalite.Init(1, "libelle", true, true, "permission", 1);
            var command = new DeleteRefFonctionnaliteCommand(1);
            _uow.Setup(x => x.RefFonctionnalite.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(refFonctionnalite);
            _uow.Setup(x => x.RefFonctionnalite.Delete(refFonctionnalite))
                .Returns(false);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.InUse.Code);
            _uow.Verify(x => x.RefFonctionnalite.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefFonctionnalite.Delete(It.IsAny<RefFonctionnalite>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteRefFonctionnaliteCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            RefFonctionnalite refFonctionnalite = RefFonctionnalite.Init(1, "libelle", true, true, "permission", 1);
            var command = new DeleteRefFonctionnaliteCommand(1);
            _uow.Setup(x => x.RefFonctionnalite.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(refFonctionnalite);
            _uow.Setup(x => x.RefFonctionnalite.Delete(refFonctionnalite))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.RefFonctionnalite.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefFonctionnalite.Delete(It.IsAny<RefFonctionnalite>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
