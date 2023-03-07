using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefFonctionnaliteTest
{
    public class UpdateRefFonctionnaliteCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateRefFonctionnaliteCommandHandler _handler;

        public UpdateRefFonctionnaliteCommandHandlerTests()
        {
            _handler = new UpdateRefFonctionnaliteCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateRefFonctionnaliteCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateRefFonctionnaliteCommand(1, "libelle", true, true, "hashCode");
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefFonctionnalite.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefFonctionnalite.Update(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefFonctionnaliteCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            RefFonctionnalite refApplication = RefFonctionnalite.Init(1, "libelle", true, true, "Allow", 1);
            var command = new UpdateRefFonctionnaliteCommand(1, "libelle", true, true, "hashCode");

            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(refApplication);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.RefFonctionnalite.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefFonctionnalite.Update(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task UpdateRefFonctionnaliteCommand_ShouldReturnError_WhenLibelleNotValid(string libelle)
        {
            RefFonctionnalite refApplication = RefFonctionnalite.Init(1, libelle, true, true, "Allow", 1);
            var hashCode = refApplication.GetHashCodeAsString();
            var command = new UpdateRefFonctionnaliteCommand(1, libelle, true, true, hashCode);
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(refApplication);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.RefFonctionnalite.Update(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefFonctionnaliteCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {
            RefFonctionnalite refApplication = RefFonctionnalite.Init(1, "libelle", true, true, "Allow", 1);            
            var hashCode = refApplication.GetHashCodeAsString();

            var command = new UpdateRefFonctionnaliteCommand(1, "libelle", true, true, hashCode);
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefFonctionnalite.GetByLibelle(It.IsAny<string>()))
                .Returns(RefFonctionnalite.Init(0, "libelle", true, true, "Allow", 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.DuplicateLibelle.Code);
            _uow.Verify(x => x.RefFonctionnalite.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.RefFonctionnalite.Update(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefFonctionnaliteCommand_ShouldReturnResult_WhenCommandOk()
        {
            RefFonctionnalite refApplication = RefFonctionnalite.Init(1, "libelle", true, true, "Allow", 1);
            var hashCode = refApplication.GetHashCodeAsString();
            var command = new UpdateRefFonctionnaliteCommand(1, "libelle", true, true, hashCode);
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefFonctionnalite.Update(It.IsAny<RefFonctionnalite>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefFonctionnalite.Update(It.IsAny<RefFonctionnalite>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
