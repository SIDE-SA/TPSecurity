using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesModuleTest
{
    public class CreateAccesModuleCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly CreateAccesModuleCommandHandler _handler;

        public CreateAccesModuleCommandHandlerTests()
        {
            _handler = new CreateAccesModuleCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateAccesModuleCommand_ShouldReturnError_WhenAccesApplicationDoesntExists()
        {
            var command = new CreateAccesModuleCommand(true, 1, 1);
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesModule.AccesApplicationNotFound.Code);
            _uow.Verify(x => x.AccesModule.Create(It.IsAny<AccesModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesModuleCommand_ShouldReturnError_WhenRefModuleDoesntExists()
        {
            var command = new CreateAccesModuleCommand(true, 1, 1);
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()))
                .Returns(AccesApplication.Init(1, true, 1, 1));
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesModule.RefModuleNotFound.Code);
            _uow.Verify(x => x.AccesModule.Create(It.IsAny<AccesModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }


        [Fact]
        public async Task CreateAccesModuleCommand_ShouldReturnError_WhenAccesModuleAlreadyExists()
        {
            var command = new CreateAccesModuleCommand(true, 1, 1);
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()))
                .Returns(AccesApplication.Init(1, true, 1, 1));
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(RefModule.Init(1, "libelle", true, 1));

            _uow.Setup(x => x.AccesModule.GetByUnicite(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(AccesModule.Init(1, true, 1, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesModule.AccesModuleAlreadyExist.Code);
            _uow.Verify(x => x.AccesModule.Create(It.IsAny<AccesModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesModuleCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateAccesModuleCommand(true, 1, 1);
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()))
                .Returns(AccesApplication.Init(1, true, 1, 1));
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(RefModule.Init(1, "libelle", true, 1));
            _uow.Setup(x => x.AccesModule.GetByUnicite(It.IsAny<int>(), It.IsAny<int>()));
            _uow.Setup(x => x.AccesModule.Create(It.IsAny<AccesModule>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()))
                .Returns(AccesModule.Init(1, true, 1, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesModule.Create(It.IsAny<AccesModule>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
