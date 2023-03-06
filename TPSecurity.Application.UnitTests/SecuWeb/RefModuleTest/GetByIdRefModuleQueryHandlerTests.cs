using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetByIdRefModule;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefModuleTest
{
    public class GetByIdRefModuleQueryHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly GetByIdRefModuleQueryHandler _handler;

        public GetByIdRefModuleQueryHandlerTests()
        {
            _handler = new GetByIdRefModuleQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdRefModuleQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetByIdRefModuleQuery(1);
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefModule.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdRefModuleQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetByIdRefModuleQuery(1);
            _uow.Setup(x => x.RefModule.GetById(It.IsAny<int>()))
                .Returns(RefModule.Init(1, "libelle", true, 1));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefModule.GetById(It.IsAny<int>()), Times.Once);
        }

    }
}
