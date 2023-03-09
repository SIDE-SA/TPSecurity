using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesUtilisateurTest
{
    public class UpdateAccesUtilisateurCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateAccesUtilisateurCommandHandler _handler;

        public UpdateAccesUtilisateurCommandHandlerTests()
        {
            _handler = new UpdateAccesUtilisateurCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateAccesUtilisateurCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateAccesUtilisateurCommand(1, true, "hashCode");
            _uow.Setup(x => x.AccesUtilisateur.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesUtilisateur.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesUtilisateur.Update(It.IsAny<AccesUtilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesUtilisateurCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            AccesUtilisateur accesUtilisateur = AccesUtilisateur.Init(1, true, 1, 1);
            var command = new UpdateAccesUtilisateurCommand(1, true, "hashCode");

            _uow.Setup(x => x.AccesUtilisateur.GetById(It.IsAny<int>()))
                .Returns(accesUtilisateur);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.AccesUtilisateur.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesUtilisateur.Update(It.IsAny<AccesUtilisateur>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesUtilisateurCommand_ShouldReturnResult_WhenCommandOk()
        {
            AccesUtilisateur accesUtilisateur = AccesUtilisateur.Init(1, true, 1, 1);
            var hashCode = accesUtilisateur.GetHashCodeAsString();
            var command = new UpdateAccesUtilisateurCommand(1, true, hashCode);
            _uow.Setup(x => x.AccesUtilisateur.GetById(It.IsAny<int>()))
                .Returns(accesUtilisateur);
            _uow.Setup(x => x.AccesUtilisateur.Update(It.IsAny<AccesUtilisateur>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesUtilisateur.Update(It.IsAny<AccesUtilisateur>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
