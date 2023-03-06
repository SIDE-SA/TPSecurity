using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Services.GeneralConcept;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesGroupeTest
{
    public class CreateAccesGroupeCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();
        private readonly Mock<IGeneralConceptService> _generalConceptService = new Mock<IGeneralConceptService>();

        private readonly CreateAccesGroupeCommandHandler _handler;

        public CreateAccesGroupeCommandHandlerTests()
        {
            _handler = new CreateAccesGroupeCommandHandler(_uow.Object, _mapper.Object, _generalConceptService.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task CreateAccesGroupeCommand_ShouldReturnError_WhenLibelleNotValid(string libelle)
        {
            var command = new CreateAccesGroupeCommand(libelle, true, true, new Guid());
            _uow.Setup(x => x.AccesGroupe.GetByLibelle(It.IsAny<string>()));
            _generalConceptService.Setup(x => x.Societe.Exist(It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.AccesGroupe.Create(It.IsAny<AccesGroupe>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesGroupeCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {
            var command = new CreateAccesGroupeCommand("libelle", true, true, new Guid());
            _uow.Setup(x => x.AccesGroupe.GetByLibelle(It.IsAny<string>()))
                .Returns(AccesGroupe.Init(0, "libelle", true, true, new Guid()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.DuplicateLibelle.Code);
            _uow.Verify(x => x.AccesGroupe.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.Create(It.IsAny<AccesGroupe>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesGroupeCommand_ShouldReturnError_WhenSocieteDoesntExists()
        {
            var command = new CreateAccesGroupeCommand("libelle", true, true, new Guid());
            _uow.Setup(x => x.AccesGroupe.GetByLibelle(It.IsAny<string>())); 
            _generalConceptService.Setup(x => x.Societe.Exist(It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesGroupe.SocietyNotFound.Code);
            _uow.Verify(x => x.AccesGroupe.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.Create(It.IsAny<AccesGroupe>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesGroupeCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateAccesGroupeCommand("libelle", true, true, new Guid());
            _uow.Setup(x => x.AccesGroupe.Create(It.IsAny<AccesGroupe>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(AccesGroupe.Init(1, "libelle", true, true, new Guid())); 
            _generalConceptService.Setup(x => x.Societe.Exist(It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesGroupe.Create(It.IsAny<AccesGroupe>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
