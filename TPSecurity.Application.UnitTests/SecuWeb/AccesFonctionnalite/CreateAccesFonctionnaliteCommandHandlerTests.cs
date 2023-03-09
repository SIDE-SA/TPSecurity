using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesFonctionnaliteTest
{
    public class CreateAccesFonctionnaliteCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly CreateAccesFonctionnaliteCommandHandler _handler;

        public CreateAccesFonctionnaliteCommandHandlerTests()
        {
            _handler = new CreateAccesFonctionnaliteCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateAccesFonctionnaliteCommand_ShouldReturnError_WhenAccesModuleDoesntExists()
        {
            var command = new CreateAccesFonctionnaliteCommand(true, 1, 1);
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesFonctionnalite.AccesModuleNotFound.Code);
            _uow.Verify(x => x.AccesFonctionnalite.Create(It.IsAny<AccesFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesFonctionnaliteCommand_ShouldReturnError_WhenRefFonctionnaliteDoesntExists()
        {
            var command = new CreateAccesFonctionnaliteCommand(true, 1, 1);
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()))
                .Returns(AccesModule.Init(1, true, 1, 1));
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesFonctionnalite.RefFonctionnaliteNotFound.Code);
            _uow.Verify(x => x.AccesFonctionnalite.Create(It.IsAny<AccesFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }


        [Fact]
        public async Task CreateAccesFonctionnaliteCommand_ShouldReturnError_WhenAccesFonctionnaliteAlreadyExists()
        {
            var command = new CreateAccesFonctionnaliteCommand(true, 1, 1);
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()))
                .Returns(AccesModule.Init(1, true, 1, 1));
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(RefFonctionnalite.Init(1, "libelle", true, true, "Allow", 1));

            _uow.Setup(x => x.AccesFonctionnalite.GetByUnicite(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(AccesFonctionnalite.Init(1, true, 1, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesFonctionnalite.AccesFonctionnaliteAlreadyExist.Code);
            _uow.Verify(x => x.AccesFonctionnalite.Create(It.IsAny<AccesFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesFonctionnaliteCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateAccesFonctionnaliteCommand(true, 1, 1);
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()))
                .Returns(AccesModule.Init(1, true, 1, 1));
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(RefFonctionnalite.Init(1, "libelle", true, true, "Allow", 1));
            _uow.Setup(x => x.AccesFonctionnalite.GetByUnicite(It.IsAny<int>(), It.IsAny<int>()));
            _uow.Setup(x => x.AccesFonctionnalite.Create(It.IsAny<AccesFonctionnalite>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(AccesFonctionnalite.Init(1, true, 1, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesFonctionnalite.Create(It.IsAny<AccesFonctionnalite>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
