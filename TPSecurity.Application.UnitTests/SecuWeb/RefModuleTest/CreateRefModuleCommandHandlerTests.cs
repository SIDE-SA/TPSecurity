using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefModuleTest
{
    public class CreateRefModuleCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly CreateRefModuleCommandHandler _handler;

        public CreateRefModuleCommandHandlerTests()
        {
            _handler = new CreateRefModuleCommandHandler(_uow.Object, _mapper.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task CreateRefModuleCommand_ShouldReturnError_WhenLibelleNotValid(string libelle)
        {
            var command = new CreateRefModuleCommand(libelle, true, 1);
            _uow.Setup(x => x.RefModule.GetByLibelle(It.IsAny<string>()));
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(RefApplication.Init(1, "libelle", true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.RefModule.Create(It.IsAny<RefModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateRefModuleCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {
            var command = new CreateRefModuleCommand("libelle", true, 1) ;
            _uow.Setup(x => x.RefModule.GetByLibelle(It.IsAny<string>()))
                .Returns(RefModule.Init(0, "libelle", true, null));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.DuplicateLibelle.Code);
            _uow.Verify(x => x.RefModule.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.RefModule.Create(It.IsAny<RefModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateRefModuleCommand_ShouldReturnError_WhenRefApplicationDoesntExists()
        {
            var command = new CreateRefModuleCommand("libelle", true, 1);
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()));
            _uow.Setup(x => x.RefModule.GetByLibelle(It.IsAny<string>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.RefModule.RefApplicationNotFound.Code);
            _uow.Verify(x => x.RefModule.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.RefModule.Create(It.IsAny<RefModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateRefModuleCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateRefModuleCommand("libelle", true, 1);
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(RefApplication.Init(1, "libelle", true));
            _uow.Setup(x => x.RefModule.Create(It.IsAny<RefModule>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(RefModule.Init(1, "libelle", true, RefApplication.Init(1, "libelle", true)));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefModule.Create(It.IsAny<RefModule>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
