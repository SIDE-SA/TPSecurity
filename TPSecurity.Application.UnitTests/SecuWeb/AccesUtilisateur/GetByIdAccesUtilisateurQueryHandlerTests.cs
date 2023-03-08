using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetByIdAccesUtilisateur;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.AccesUtilisateurTest
{
    public class GetByIdAccesUtilisateurQueryHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly GetByIdAccesUtilisateurQueryHandler _handler;

        public GetByIdAccesUtilisateurQueryHandlerTests()
        {
            _handler = new GetByIdAccesUtilisateurQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdAccesUtilisateurQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetByIdAccesUtilisateurQuery(1);
            _uow.Setup(x => x.AccesUtilisateur.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.AccesUtilisateur.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAccesUtilisateurQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetByIdAccesUtilisateurQuery(1);
            _uow.Setup(x => x.AccesUtilisateur.GetById(It.IsAny<int>()))
                .Returns(AccesUtilisateur.Init(1, true, 1, 1));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.AccesUtilisateur.GetById(It.IsAny<int>()), Times.Once);
        }

    }
}
