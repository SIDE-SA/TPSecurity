using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetByIdAccesApplication;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesApplicationTest
{
    public class GetByIdAccesApplicationQueryHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly GetByIdAccesApplicationQueryHandler _handler;

        public GetByIdAccesApplicationQueryHandlerTests()
        {
            _handler = new GetByIdAccesApplicationQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdAccesApplicationQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetByIdAccesApplicationQuery(1);
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesApplication.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAccesApplicationQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetByIdAccesApplicationQuery(1);
            _uow.Setup(x => x.AccesApplication.GetById(It.IsAny<int>()))
                .Returns(AccesApplication.Init(1, true, 1, 1));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesApplication.GetById(It.IsAny<int>()), Times.Once);
        }

    }
}
