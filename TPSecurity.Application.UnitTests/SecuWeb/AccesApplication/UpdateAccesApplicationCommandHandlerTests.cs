using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesApplicationTest
{
    public class UpdateAccesApplicationCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateAccesApplicationCommandHandler _handler;

        public UpdateAccesApplicationCommandHandlerTests()
        {
            _handler = new UpdateAccesApplicationCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateAccesApplicationCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateAccesApplicationCommand(1, true, "hashCode");
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesApplication.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesApplication.Update(It.IsAny<AccesApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesApplicationCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            AccesApplication accesApplication = AccesApplication.Init(1, true, 1, 1);
            var command = new UpdateAccesApplicationCommand(1, true, "hashCode");

            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()))
                .Returns(accesApplication);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.AccesApplication.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesApplication.Update(It.IsAny<AccesApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesApplicationCommand_ShouldReturnResult_WhenCommandOk()
        {
            AccesApplication accesApplication = AccesApplication.Init(1, true, 1, 1);
            var hashCode = accesApplication.GetHashCodeAsString();
            var command = new UpdateAccesApplicationCommand(1, true, hashCode);
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()))
                .Returns(accesApplication);
            _uow.Setup(x => x.AccesApplication.Update(It.IsAny<AccesApplication>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesApplication.Update(It.IsAny<AccesApplication>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
