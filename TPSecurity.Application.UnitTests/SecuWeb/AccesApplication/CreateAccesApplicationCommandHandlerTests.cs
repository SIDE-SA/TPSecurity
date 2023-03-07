using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesApplicationTest
{
    public class CreateAccesApplicationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly CreateAccesApplicationCommandHandler _handler;

        public CreateAccesApplicationCommandHandlerTests()
        {
            _handler = new CreateAccesApplicationCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateAccesApplicationCommand_ShouldReturnError_WhenAccesGroupeDoesntExists()
        {
            var command = new CreateAccesApplicationCommand(true, 1, 1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesApplication.AccesGroupeNotFound.Code);
            _uow.Verify(x => x.AccesApplication.Create(It.IsAny<AccesApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesApplicationCommand_ShouldReturnError_WhenRefApplicationDoesntExists()
        {
            var command = new CreateAccesApplicationCommand(true, 1, 1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(AccesGroupe.Init(1, "libelle", true, true, new Guid()));
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesApplication.RefApplicationNotFound.Code);
            _uow.Verify(x => x.AccesApplication.Create(It.IsAny<AccesApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }


        [Fact]
        public async Task CreateAccesApplicationCommand_ShouldReturnError_WhenAccesApplicationAlreadyExists()
        {
            var command = new CreateAccesApplicationCommand(true, 1, 1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(AccesGroupe.Init(1, "libelle", true, true, new Guid()));
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(RefApplication.Init(1, "libelle", true));

            _uow.Setup(x => x.AccesApplication.GetByUnicite(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(AccesApplication.Init(1, true, 1, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesApplication.AccesApplicationAlreadyExist.Code);
            _uow.Verify(x => x.AccesApplication.Create(It.IsAny<AccesApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesApplicationCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateAccesApplicationCommand(true, 1, 1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(AccesGroupe.Init(1, "libelle", true, true, new Guid()));
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(RefApplication.Init(1, "libelle", true));
            _uow.Setup(x => x.AccesApplication.GetByUnicite(It.IsAny<int>(), It.IsAny<int>()));
            _uow.Setup(x => x.AccesApplication.Create(It.IsAny<AccesApplication>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()))
                .Returns(AccesApplication.Init(1, true, 1, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesApplication.Create(It.IsAny<AccesApplication>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
