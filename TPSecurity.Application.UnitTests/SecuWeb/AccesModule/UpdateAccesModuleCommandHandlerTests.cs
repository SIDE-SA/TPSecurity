using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesModuleTest
{
    public class UpdateAccesModuleCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateAccesModuleCommandHandler _handler;

        public UpdateAccesModuleCommandHandlerTests()
        {
            _handler = new UpdateAccesModuleCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateAccesModuleCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateAccesModuleCommand(1, true, "hashCode");
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesModule.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesModule.Update(It.IsAny<AccesModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesModuleCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            AccesModule accesModule = AccesModule.Init(1, true, 1, 1);
            var command = new UpdateAccesModuleCommand(1, true, "hashCode");

            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()))
                .Returns(accesModule);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.AccesModule.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesModule.Update(It.IsAny<AccesModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateAccesModuleCommand_ShouldReturnResult_WhenCommandOk()
        {
            AccesModule accesModule = AccesModule.Init(1, true, 1, 1);
            var hashCode = accesModule.GetHashCodeAsString();
            var command = new UpdateAccesModuleCommand(1, true, hashCode);
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()))
                .Returns(accesModule);
            _uow.Setup(x => x.AccesModule.Update(It.IsAny<AccesModule>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesModule.Update(It.IsAny<AccesModule>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
