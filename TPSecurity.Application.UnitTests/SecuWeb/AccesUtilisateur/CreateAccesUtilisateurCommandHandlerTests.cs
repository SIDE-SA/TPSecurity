using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesUtilisateurTest
{
    public class CreateAccesUtilisateurCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly CreateAccesUtilisateurCommandHandler _handler;

        public CreateAccesUtilisateurCommandHandlerTests()
        {
            _handler = new CreateAccesUtilisateurCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateAccesUtilisateurCommand_ShouldReturnError_WhenAccesGroupeDoesntExists()
        {
            var command = new CreateAccesUtilisateurCommand(true, 1, 1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesUtilisateur.AccesGroupeNotFound.Code);
            _uow.Verify(x => x.AccesUtilisateur.Create(It.IsAny<AccesUtilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesUtilisateurCommand_ShouldReturnError_WhenUtilisateurDoesntExists()
        {
            var command = new CreateAccesUtilisateurCommand(true, 1, 1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(AccesGroupe.Init(1, "lbelle", true, true, new Guid()));
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesUtilisateur.UtilisateurNotFound.Code);
            _uow.Verify(x => x.AccesUtilisateur.Create(It.IsAny<AccesUtilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }


        [Fact]
        public async Task CreateAccesUtilisateurCommand_ShouldReturnError_WhenAccesUtilisateurAlreadyExists()
        {
            var command = new CreateAccesUtilisateurCommand(true, 1, 1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(AccesGroupe.Init(1, "lbelle", true, true, new Guid()));
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(Utilisateur.Init(1, "nom", "prenom", "email", true));

            _uow.Setup(x => x.AccesUtilisateur.GetByUnicite(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(AccesUtilisateur.Init(1, true, 1, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.AccesUtilisateur.AccesUtilisateurAlreadyExist.Code);
            _uow.Verify(x => x.AccesUtilisateur.Create(It.IsAny<AccesUtilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateAccesUtilisateurCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateAccesUtilisateurCommand(true, 1, 1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(AccesGroupe.Init(1, "lbelle", true, true, new Guid()));
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(Utilisateur.Init(1, "nom", "prenom", "email", true));
            _uow.Setup(x => x.AccesUtilisateur.GetByUnicite(It.IsAny<int>(), It.IsAny<int>()));
            _uow.Setup(x => x.AccesUtilisateur.Create(It.IsAny<AccesUtilisateur>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.AccesUtilisateur.GetById(It.IsAny<int>()))
                .Returns(AccesUtilisateur.Init(1, true, 1, 1));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesUtilisateur.Create(It.IsAny<AccesUtilisateur>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
