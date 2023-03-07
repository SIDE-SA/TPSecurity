using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetByIdAccesModule;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesModuleTest
{
    public class GetByIdAccesModuleQueryHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly GetByIdAccesModuleQueryHandler _handler;

        public GetByIdAccesModuleQueryHandlerTests()
        {
            _handler = new GetByIdAccesModuleQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdAccesModuleQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetByIdAccesModuleQuery(1);
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesModule.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAccesModuleQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetByIdAccesModuleQuery(1);
            _uow.Setup(x => x.AccesModule.GetById(It.IsAny<int>()))
                .Returns(AccesModule.Init(1, true, 1, 1));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesModule.GetById(It.IsAny<int>()), Times.Once);
        }

    }
}
