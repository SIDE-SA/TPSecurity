using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetByIdRefFonctionnalite;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.RefFonctionnaliteTest
{
    public class GetByIdRefFonctionnaliteQueryHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IBaseClass> _baseClass = new Mock<IBaseClass>();

        private readonly GetByIdRefFonctionnaliteQueryHandler _handler;

        public GetByIdRefFonctionnaliteQueryHandlerTests()
        {
            _handler = new GetByIdRefFonctionnaliteQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdRefFonctionnaliteQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetByIdRefFonctionnaliteQuery(1);
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.RefFonctionnalite.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdRefFonctionnaliteQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetByIdRefFonctionnaliteQuery(1);
            _uow.Setup(x => x.RefFonctionnalite.GetById(It.IsAny<int>()))
                .Returns(RefFonctionnalite.Init(1, "libelle", true, true, "permission", 1));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.RefFonctionnalite.GetById(It.IsAny<int>()), Times.Once);
        }

    }
}
