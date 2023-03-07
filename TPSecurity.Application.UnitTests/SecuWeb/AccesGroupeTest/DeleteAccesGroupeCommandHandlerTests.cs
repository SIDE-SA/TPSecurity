using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Delete;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesGroupeTest
{
    public class DeleteAccesGroupeCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly DeleteAccesGroupeCommandHandler _handler;

        public DeleteAccesGroupeCommandHandlerTests()
        {
            _handler = new DeleteAccesGroupeCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteAccesGroupeCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new DeleteAccesGroupeCommand(1);
            _uow.Setup(x => x.AccesGroupe.GetByIdWithReferences(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesGroupe.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.Delete(It.IsAny<AccesGroupe>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteAccesGroupeCommand_ShouldReturnInUse_ReferencesExists()
        {
            //Arrange
            AccesGroupe accesGroupe = AccesGroupe.Init(1, "libelle", true, true, new Guid());
            var command = new DeleteAccesGroupeCommand(1);
            _uow.Setup(x => x.AccesGroupe.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(accesGroupe);
            _uow.Setup(x => x.AccesGroupe.Delete(accesGroupe))
                .Returns(false);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.InUse.Code);
            _uow.Verify(x => x.AccesGroupe.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.Delete(It.IsAny<AccesGroupe>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteAccesGroupeCommand_ShouldReturnDeleted_WhenCommandOk()
        {
            //Arrange
            AccesGroupe accesGroupe = AccesGroupe.Init(1, "libelle", true, true, new Guid());
            var command = new DeleteAccesGroupeCommand(1);
            _uow.Setup(x => x.AccesGroupe.GetByIdWithReferences(It.IsAny<int>()))
                .Returns(accesGroupe);
            _uow.Setup(x => x.AccesGroupe.Delete(accesGroupe))
                .Returns(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();            
            _uow.Verify(x => x.AccesGroupe.GetByIdWithReferences(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.Delete(It.IsAny<AccesGroupe>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
