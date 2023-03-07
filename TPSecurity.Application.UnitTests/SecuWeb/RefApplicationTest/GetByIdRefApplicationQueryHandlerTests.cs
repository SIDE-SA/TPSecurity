using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetByIdRefApplication;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefApplicationTest
{
    public class GetByIdRefApplicationQueryHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly GetByIdRefApplicationQueryHandler _handler;

        public GetByIdRefApplicationQueryHandlerTests()
        {
            _handler = new GetByIdRefApplicationQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdRefApplicationQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetByIdRefApplicationQuery(1);
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefApplication.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdRefApplicationQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetByIdRefApplicationQuery(1);
            _uow.Setup(x => x.RefApplication.GetById(It.IsAny<int>()))
                .Returns(RefApplication.Init(1, "libelle", true));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefApplication.GetById(It.IsAny<int>()), Times.Once);
        }

    }
}
