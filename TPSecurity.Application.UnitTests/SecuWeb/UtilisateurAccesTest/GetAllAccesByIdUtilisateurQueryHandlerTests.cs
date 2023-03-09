using FluentAssertions;
using MapsterMapper;
using Moq;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Queries.GetAllAccesByIdUtilisateur;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetByIdUtilisateur;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using Xunit;

namespace TPSecurity.Application.UnitTests.SecuWeb.UtilisateurAccesTest
{
    public class GetAllAccesByIdUtilisateurQueryHandlerHandlerTests
    {
        private readonly Mock<IUnitOfWorkGTP> _uow = new Mock<IUnitOfWorkGTP>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly GetAllAccesByIdUtilisateurQueryHandler _handler;

        public GetAllAccesByIdUtilisateurQueryHandlerHandlerTests()
        {
            _handler = new GetAllAccesByIdUtilisateurQueryHandler(_uow.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetAllAccesByIdUtilisateurQuery_ShouldReturnNotFound_WhenIdDoesntExists()
        {
            //Arrange        
            var command = new GetAllAccesByIdUtilisateurQuery(1);
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Code.Should().Be(Errors.NotFound.Code);
            _uow.Verify(x => x.Utilisateur.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAccesByIdUtilisateurQuery_ShouldReturnResult_WhenQueryOk()
        {
            //Arrange        
            var command = new GetAllAccesByIdUtilisateurQuery(1);
            _uow.Setup(x => x.Utilisateur.GetById(It.IsAny<int>()))
                .Returns(Utilisateur.Init(1, "nom", "prenom", "email", true));
            _uow.Setup(x => x.AccesGroupe.GetByIdUtilisateur(It.IsAny<int>()))
                .Returns(new List<AccesGroupe>() { AccesGroupe.Init(1, "nom", true, true, new Guid(), new List<AccesApplication>()) });

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            _uow.Verify(x => x.Utilisateur.GetById(It.IsAny<int>()), Times.Once);
            _uow.Verify(x => x.AccesGroupe.GetByIdUtilisateur(It.IsAny<int>()), Times.Once);
        }

    }
}
