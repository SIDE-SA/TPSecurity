using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefModuleTest
{
    public class UpdateRefModuleCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateRefModuleCommandHandler _handler;

        public UpdateRefModuleCommandHandlerTests()
        {
            _handler = new UpdateRefModuleCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateRefModuleCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateRefModuleCommand(1, "libelle", true, "hashCode");
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefModule.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefModule.Update(It.IsAny<RefModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefModuleCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            RefModule refApplication = RefModule.Init(1, "libelle", true, null);
            var command = new UpdateRefModuleCommand(1, "libelle", true, "hashCode");

            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(refApplication);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.RefModule.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefModule.Update(It.IsAny<RefModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task UpdateRefModuleCommand_ShouldReturnError_WhenLibelleNotValid(string libelle)
        {
            RefModule refApplication = RefModule.Init(1, libelle, true, null);
            var hashCode = refApplication.GetHashCodeAsString();
            var command = new UpdateRefModuleCommand(1, libelle, true, hashCode);
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(refApplication);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorOr.ErrorType.Validation);
            _uow.Verify(x => x.RefModule.Update(It.IsAny<RefModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefModuleCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {
            RefModule refApplication = RefModule.Init(1, "libelle", true, null);            
            var hashCode = refApplication.GetHashCodeAsString();

            var command = new UpdateRefModuleCommand(1, "libelle", true, hashCode);
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefModule.GetByLibelle(It.IsAny<string>()))
                .Returns(RefModule.Init(0, "libelle", true, null));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.DuplicateLibelle.Code);
            _uow.Verify(x => x.RefModule.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.RefModule.Update(It.IsAny<RefModule>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefModuleCommand_ShouldReturnResult_WhenCommandOk()
        {
            RefModule refApplication = RefModule.Init(1, "libelle", true, RefApplication.Init(1, "libelle", true));
            var hashCode = refApplication.GetHashCodeAsString();
            var command = new UpdateRefModuleCommand(1, "libelle", true, hashCode);
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefModule.Update(It.IsAny<RefModule>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefModule.Update(It.IsAny<RefModule>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
