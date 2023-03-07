using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Create;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.UtilisateurTest
{
    public class CreateUtilisateurCommandHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly CreateUtilisateurCommandHandler _handler;

        public CreateUtilisateurCommandHandlerTests()
        {
            _handler = new CreateUtilisateurCommandHandler(_uow.Object, _mapper.Object);
        }

        [Theory] 
        [InlineData("", "a","a")]
        [InlineData("a", "", "a")]
        [InlineData("a", "a", "")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "a","a")]
        [InlineData("a", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "a")]
        [InlineData("a", "a", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task CreateUtilisateurCommand_ShouldReturnError_WhenLibelleNotValid(string nom, string prenom, string email)
        {
            var command = new CreateUtilisateurCommand(nom, prenom, email, true);
            _uow.Setup(x => x.Utilisateur.GetByEmail(It.IsAny<string>()));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.Utilisateur.Create(It.IsAny<Utilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateUtilisateurCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {
            var command = new CreateUtilisateurCommand("nom", "prenom", "email", true) ;
            _uow.Setup(x => x.Utilisateur.GetByEmail(It.IsAny<string>()))
                .Returns(Utilisateur.Init(0, "nom", "prenom", "email", true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Utilisateur.DuplicateEmail.Code);
            _uow.Verify(x => x.Utilisateur.GetByEmail(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.Utilisateur.Create(It.IsAny<Utilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task CreateUtilisateurCommand_ShouldReturnResult_WhenCommandOk()
        {
            var command = new CreateUtilisateurCommand("nom", "prenom", "email", true);
            _uow.Setup(x => x.Utilisateur.Create(It.IsAny<Utilisateur>()))
                .Returns(_baseClass.Object);
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(Utilisateur.Init(1, "nom", "prenom", "email", true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.Utilisateur.Create(It.IsAny<Utilisateur>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
