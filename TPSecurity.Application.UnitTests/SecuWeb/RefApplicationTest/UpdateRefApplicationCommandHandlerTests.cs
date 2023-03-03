using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Update;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefApplicationTest
{
    public class UpdateRefApplicationCommandHandlerTests
    {         
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly UpdateRefApplicationCommandHandler _handler;

        public UpdateRefApplicationCommandHandlerTests()
        {
            _handler = new UpdateRefApplicationCommandHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateRefApplicationCommand_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new UpdateRefApplicationCommand(1, "libelle", true, "hashCode");
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefApplication.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefApplication.Update(It.IsAny<RefApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefApplicationCommand_ShouldReturnConcurrency_WhenHashCodeIsNotEqual()
        {
            //Arrange        
            RefApplication refApplication = RefApplication.Init(1, "libelle", true);
            var command = new UpdateRefApplicationCommand(1, "libelle", true, "hashCode");

            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(refApplication);           

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.Concurrency.Code);
            _uow.Verify(x => x.RefApplication.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.RefApplication.Update(It.IsAny<RefApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task UpdateRefApplicationCommand_ShouldReturnError_WhenLibelleNotValid(string libelle)
        {
            RefApplication refApplication = RefApplication.Init(1, libelle, true);
            var hashCode = refApplication.GetHashCodeAsString();
            var command = new UpdateRefApplicationCommand(1, libelle, true, hashCode);
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(refApplication);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            _uow.Verify(x => x.RefApplication.Update(It.IsAny<RefApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefApplicationCommand_ShouldReturnError_WhenLibelleAlreadyExists()
        {
            RefApplication refApplication = RefApplication.Init(1, "libelle", true);            
            var hashCode = refApplication.GetHashCodeAsString();

            var command = new UpdateRefApplicationCommand(1, "libelle", true, hashCode);
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefApplication.GetByLibelle(It.IsAny<string>()))
                .Returns(RefApplication.Init(0, "libelle", true));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.DuplicateLibelle.Code);
            _uow.Verify(x => x.RefApplication.GetByLibelle(It.IsAny<string>()), Times.Once);
            _uow.Verify(x => x.RefApplication.Update(It.IsAny<RefApplication>()), Times.Never);
            _uow.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateRefApplicationCommand_ShouldReturnResult_WhenCommandOk()
        {
            RefApplication refApplication = RefApplication.Init(1, "libelle", true);
            var hashCode = refApplication.GetHashCodeAsString();
            var command = new UpdateRefApplicationCommand(1, "libelle", true, hashCode);
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(refApplication);
            _uow.Setup(x => x.RefApplication.Update(It.IsAny<RefApplication>()))
              .Returns(_baseClass.Object);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefApplication.Update(It.IsAny<RefApplication>()), Times.Once);
            _uow.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
