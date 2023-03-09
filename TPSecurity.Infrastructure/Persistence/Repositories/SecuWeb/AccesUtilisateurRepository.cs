using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetAllAccesUtilisateur;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class AccesUtilisateurRepository : IAccesUtilisateurRepository
    {
        private readonly ApplicationContextGTP _context;

        public AccesUtilisateurRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public AccesUtilisateur? GetById(int id)
        {
            return _context.AccesUtilisateur
                    .Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public AccesUtilisateur? GetByUnicite(int idAccesGroupe, int idUtilisateur)
        {
            return _context.AccesUtilisateur
                    .Where(x => x.IdAccesGroupe == idAccesGroupe && x.IdUtilisateur == idUtilisateur).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public PagedList<AccesUtilisateur> GetAccesUtilisateurs(AccesUtilisateurParameters queryParameters)
        {
            var accesUtilisateurDTOs = _context.AccesUtilisateur
                    .AsQueryable();

            SearchByEstActif(ref accesUtilisateurDTOs, queryParameters.EstActif);
            SearchByIdAccesGroupe(ref accesUtilisateurDTOs, queryParameters.IdAccesGroupe);
            SearchByIdUtilisateur(ref accesUtilisateurDTOs, queryParameters.IdUtilisateur);
            SortHelper.ApplySort(ref accesUtilisateurDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<AccesUtilisateurDTO>.ApplyPagination(ref accesUtilisateurDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var accesUtilisateurs = FromDTO(accesUtilisateurDTOs);
            return PagedList<AccesUtilisateur>.ToPagedList(accesUtilisateurs, totalCount);
        }

        private void SearchByEstActif(ref IQueryable<AccesUtilisateurDTO> accesUtilisateurs, bool? estActif)
        {
            if (!estActif.HasValue) return;
            accesUtilisateurs = accesUtilisateurs.Where(x => x.EstActif == estActif);
        }

        private void SearchByIdAccesGroupe(ref IQueryable<AccesUtilisateurDTO> accesUtilisateurs, int? idAccesGroupe)
        {
            if (idAccesGroupe is null) return;
            accesUtilisateurs = accesUtilisateurs.Where(x => x.IdAccesGroupe == idAccesGroupe);
        }

        private void SearchByIdUtilisateur(ref IQueryable<AccesUtilisateurDTO> accesUtilisateurs, int? idUtilisateur)
        {
            if (idUtilisateur is null) return;
            accesUtilisateurs = accesUtilisateurs.Where(x => x.IdUtilisateur == idUtilisateur);
        }

        public IBaseClass Create(AccesUtilisateur accesUtilisateur)
        {
            AccesUtilisateurDTO accesUtilisateurDTO = ToDTO(accesUtilisateur);
            _context.AccesUtilisateur.Add(accesUtilisateurDTO);
            return accesUtilisateurDTO;
        }

        public IBaseClass Update(AccesUtilisateur accesUtilisateur)
        {
            AccesUtilisateurDTO accesUtilisateurDTO = _context.AccesUtilisateur.Find(accesUtilisateur.Id);
            ApplyChanges(accesUtilisateurDTO, accesUtilisateur);
            _context.AccesUtilisateur.Update(accesUtilisateurDTO);
            return accesUtilisateurDTO;
        }

        public bool Delete(AccesUtilisateur accesUtilisateur)
        {
            AccesUtilisateurDTO accesUtilisateurDTO = _context.AccesUtilisateur.Find(accesUtilisateur.Id);

            _context.AccesUtilisateur.Remove(accesUtilisateurDTO);

            return true;
        }

        public AccesUtilisateurDTO ToDTO(AccesUtilisateur accesUtilisateur)
        {
            AccesUtilisateurDTO dto = new AccesUtilisateurDTO(accesUtilisateur.Id, accesUtilisateur.EstActif, accesUtilisateur.IdAccesGroupe, accesUtilisateur.IdUtilisateur);
            return dto;
        }

        public static AccesUtilisateur FromDTO(AccesUtilisateurDTO dto)
        {
            AccesUtilisateur accesUtilisateur = AccesUtilisateur.Init(dto.Id, dto.EstActif, dto.IdAccesGroupe, dto.IdUtilisateur);
            return accesUtilisateur;
        }

        public static IEnumerable<AccesUtilisateur> FromDTO(IQueryable<AccesUtilisateurDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static void ApplyChanges(AccesUtilisateurDTO dest, AccesUtilisateur source)
        {
            if (dest is null || source is null) return;
            dest.EstActif = source.EstActif;
        }       
    }
}
