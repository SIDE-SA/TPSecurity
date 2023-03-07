using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetByIdAccesGroupe;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesGroupeTest
{
    public class GetByIdAccesGroupeQueryHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly GetByIdAccesGroupeQueryHandler _handler;

        public GetByIdAccesGroupeQueryHandlerTests()
        {
            _handler = new GetByIdAccesGroupeQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdAccesGroupeQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetByIdAccesGroupeQuery(1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesGroupe.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAccesGroupeQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetByIdAccesGroupeQuery(1);
            _uow.Setup(x => x.AccesGroupe.GetById(It.IsAny<int>()))
                .Returns(AccesGroupe.Init(1, "libelle", true, true, new Guid()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesGroupe.GetById(It.IsAny<int>()), Times.Once);
        }

    }
}
