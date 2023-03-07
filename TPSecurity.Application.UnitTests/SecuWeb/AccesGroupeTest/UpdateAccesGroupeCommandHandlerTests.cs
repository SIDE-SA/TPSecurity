using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesGroupeTest
{
    public class UpdateAccesGroupeCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateAccesGroupeCommandHandler _handler;

        public UpdateAccesGroupeCommandHandlerTests()
        {
            _handler = new UpdateAccesGroupeCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateAccesGroupeCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateAccesGroupeCommand(1, "libelle", true, true, "hashCode");
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesGroupe.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.Update(It.IsAny<AccesGroupe>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesGroupeCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            AccesGroupe accesGroupe = AccesGroupe.Init(1, "libelle", true, true, new Guid());
            var command = new UpdateAccesGroupeCommand(1, "libelle", true, true, "hashCode");

            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(accesGroupe);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.AccesGroupe.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.Update(It.IsAny<AccesGroupe>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task UpdateAccesGroupeCommand_ShouldReturnError_WhenLibelleNotValid(string libelle)
        {
            AccesGroupe accesGroupe = AccesGroupe.Init(1, libelle, true, true, new Guid());
            var hashCode = accesGroupe.GetHashCodeAsString();
            var command = new UpdateAccesGroupeCommand(1, libelle, true, true, hashCode);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(accesGroupe);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.AccesGroupe.Update(It.IsAny<AccesGroupe>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesGroupeCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {
            AccesGroupe accesGroupe = AccesGroupe.Init(1, "libelle", true, true, new Guid());            
            var hashCode = accesGroupe.GetHashCodeAsString();

            var command = new UpdateAccesGroupeCommand(1, "libelle", true, true, hashCode);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(accesGroupe);
            _uow.Setup(x => x.AccesGroupe.GetByLibelle(It.IsAny<string>()))
                .Returns(AccesGroupe.Init(0, "libelle", true, true, new Guid()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.DuplicateLibelle.Code);
            _uow.Verify(x => x.AccesGroupe.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.Update(It.IsAny<AccesGroupe>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesGroupeCommand_ShouldReturnResult_WhenCommandOk()
        {
            AccesGroupe accesGroupe = AccesGroupe.Init(1, "libelle", true, true, new Guid());
            var hashCode = accesGroupe.GetHashCodeAsString();
            var command = new UpdateAccesGroupeCommand(1, "libelle", true, true, hashCode);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(accesGroupe);
            _uow.Setup(x => x.AccesGroupe.Update(It.IsAny<AccesGroupe>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesGroupe.Update(It.IsAny<AccesGroupe>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
