using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefApplicationTest
{
    public class CreateRefApplicationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly CreateRefApplicationCommandHandler _handler;

        public CreateRefApplicationCommandHandlerTests()
        {
            _handler = new CreateRefApplicationCommandHandler(_uow.Object, _mapper.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task CreateRefApplicationCommand_ShouldReturnError_WhenLibelleNotValid(string libelle)
        {
            var command = new CreateRefApplicationCommand(libelle, true);
            _uow.Setup(x => x.RefApplication.GetByLibelle(It.IsAny<string>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.RefApplication.Create(It.IsAny<RefApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateRefApplicationCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {    
            var command = new CreateRefApplicationCommand("libelle", true);
            _uow.Setup(x => x.RefApplication.GetByLibelle(It.IsAny<string>()))
                .Returns(RefApplication.Init(0, "libelle", true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.DuplicateLibelle.Code);
            _uow.Verify(x => x.RefApplication.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.RefApplication.Create(It.IsAny<RefApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateRefApplicationCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateRefApplicationCommand("libelle", true);
            _uow.Setup(x => x.RefApplication.Create(It.IsAny<RefApplication>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(RefApplication.Init(1, "libelle", true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefApplication.Create(It.IsAny<RefApplication>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
