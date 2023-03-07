using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.UtilisateurTest
{
    public class UpdateUtilisateurCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateUtilisateurCommandHandler _handler;

        public UpdateUtilisateurCommandHandlerTests()
        {
            _handler = new UpdateUtilisateurCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateUtilisateurCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateUtilisateurCommand(1, "nom", "prenom", "email", true, "hashCode");
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.Utilisateur.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.Utilisateur.Update(It.IsAny<Utilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateUtilisateurCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            Utilisateur utilisateur = Utilisateur.Init(1, "nom", "prenom", "email", true);
            var command = new UpdateUtilisateurCommand(1, "nom", "prenom", "email", true, "hashCode");

            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(utilisateur);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.Utilisateur.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.Utilisateur.Update(It.IsAny<Utilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData("", "a", "a")]
        [InlineData("a", "", "a")]
        [InlineData("a", "a", "")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "a", "a")]
        [InlineData("a", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "a")]
        [InlineData("a", "a", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task UpdateUtilisateurCommand_ShouldReturnError_WhenLibelleNotValid(string nom, string prenom, string email)
        {
            Utilisateur utilisateur = Utilisateur.Init(1, nom, prenom, email, true);
            var hashCode = utilisateur.GetHashCodeAsString();
            var command = new UpdateUtilisateurCommand(1, nom, prenom, email, true, hashCode);
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(utilisateur);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.Utilisateur.Update(It.IsAny<Utilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateUtilisateurCommand_ShouldReturnError_WhenEmailAlreadyExists()
        {
            Utilisateur utilisateur = Utilisateur.Init(1, "nom", "prenom", "email", true);            
            var hashCode = utilisateur.GetHashCodeAsString();

            var command = new UpdateUtilisateurCommand(1, "nom", "prenom", "email", true, hashCode);
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(utilisateur);
            _uow.Setup(x => x.Utilisateur.GetByEmail(It.IsAny<string>()))
                .Returns(Utilisateur.Init(0, "nom", "prenom", "email", true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Utilisateur.DuplicateEmail.Code);
            _uow.Verify(x => x.Utilisateur.GetByEmail(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.Utilisateur.Update(It.IsAny<Utilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateUtilisateurCommand_ShouldReturnResult_WhenCommandOk()
        {
            Utilisateur utilisateur = Utilisateur.Init(1, "nom", "prenom", "email", true);
            var hashCode = utilisateur.GetHashCodeAsString();
            var command = new UpdateUtilisateurCommand(1, "nom", "prenom", "email", true, hashCode);
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(utilisateur);
            _uow.Setup(x => x.Utilisateur.Update(It.IsAny<Utilisateur>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.Utilisateur.Update(It.IsAny<Utilisateur>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
