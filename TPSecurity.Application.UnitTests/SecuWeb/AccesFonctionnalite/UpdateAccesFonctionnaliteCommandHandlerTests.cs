using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesFonctionnaliteTest
{
    public class UpdateAccesFonctionnaliteCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateAccesFonctionnaliteCommandHandler _handler;

        public UpdateAccesFonctionnaliteCommandHandlerTests()
        {
            _handler = new UpdateAccesFonctionnaliteCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateAccesFonctionnaliteCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateAccesFonctionnaliteCommand(1, true, "hashCode");
            _uow.Setup(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesFonctionnalite.Update(It.IsAny<AccesFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesFonctionnaliteCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            AccesFonctionnalite accesFonctionnalite = AccesFonctionnalite.Init(1, true, 1, 1);
            var command = new UpdateAccesFonctionnaliteCommand(1, true, "hashCode");

            _uow.Setup(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(accesFonctionnalite);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesFonctionnalite.Update(It.IsAny<AccesFonctionnalite>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesFonctionnaliteCommand_ShouldReturnResult_WhenCommandOk()
        {
            AccesFonctionnalite accesFonctionnalite = AccesFonctionnalite.Init(1, true, 1, 1);
            var hashCode = accesFonctionnalite.GetHashCodeAsString();
            var command = new UpdateAccesFonctionnaliteCommand(1, true, hashCode);
            _uow.Setup(x => x.AccesFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(accesFonctionnalite);
            _uow.Setup(x => x.AccesFonctionnalite.Update(It.IsAny<AccesFonctionnalite>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesFonctionnalite.Update(It.IsAny<AccesFonctionnalite>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
