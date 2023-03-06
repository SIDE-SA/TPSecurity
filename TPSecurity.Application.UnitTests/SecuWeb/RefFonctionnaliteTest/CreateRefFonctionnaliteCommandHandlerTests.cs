using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefFonctionnaliteTest
{
    public class CreateRefFonctionnaliteCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly CreateRefFonctionnaliteCommandHandler _handler;

        public CreateRefFonctionnaliteCommandHandlerTests()
        {
            _handler = new CreateRefFonctionnaliteCommandHandler(_uow.Object, _mapper.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task CreateRefFonctionnaliteCommand_ShouldReturnError_WhenLibelleNotValid(string libelle)
        {
            var command = new CreateRefFonctionnaliteCommand(libelle, true, true, "Allow", 1);
            _uow.Setup(x => x.RefFonctionnalite.GetByLibelle(It.IsAny<string>()));
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(RefModule.Init(1, "libelle", true, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.RefFonctionnalite.Create(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData("Ecrit")]
        public async Task CreateRefFonctionnaliteCommand_ShouldReturnError_WhenPermissionNotAllowed(string permission)
        {
            var command = new CreateRefFonctionnaliteCommand("libelle", true, true, permission, 1);
            _uow.Setup(x => x.RefFonctionnalite.GetByLibelle(It.IsAny<string>()));
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(RefApplication.Init(1, "libelle", true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.RefFonctionnalite.PermissionNotAllowed.Code);
            _uow.Verify(x => x.RefFonctionnalite.Create(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateRefFonctionnaliteCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {
            var command = new CreateRefFonctionnaliteCommand("libelle", true, true, "Allow", 1) ;
            _uow.Setup(x => x.RefFonctionnalite.GetByLibelle(It.IsAny<string>()))
                .Returns(RefFonctionnalite.Init(0, "libelle", true, true, "Allow", 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.DuplicateLibelle.Code);
            _uow.Verify(x => x.RefFonctionnalite.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.RefFonctionnalite.Create(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateRefFonctionnaliteCommand_ShouldReturnError_WhenRefApplicationDoesntExists()
        {
            var command = new CreateRefFonctionnaliteCommand("libelle", true, true, "Allow", 1);
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()));
            _uow.Setup(x => x.RefFonctionnalite.GetByLibelle(It.IsAny<string>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.RefFonctionnalite.RefModuleNotFound.Code);
            _uow.Verify(x => x.RefFonctionnalite.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.RefFonctionnalite.Create(It.IsAny<RefFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateRefFonctionnaliteCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateRefFonctionnaliteCommand("libelle", true, true, "Allow", 1);
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(RefModule.Init(1, "libelle", true, 1));
            _uow.Setup(x => x.RefFonctionnalite.Create(It.IsAny<RefFonctionnalite>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(RefFonctionnalite.Init(1, "libelle", true, true, "Allow", 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefFonctionnalite.Create(It.IsAny<RefFonctionnalite>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
