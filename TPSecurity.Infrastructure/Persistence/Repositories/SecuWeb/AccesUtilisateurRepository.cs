using Microsoft.EntityFrameworkCore;
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
            var accesApplicationDTOs = _context.AccesUtilisateur
                    .AsQueryable();

            SearchByEstActif(ref accesApplicationDTOs, queryParameters.EstActif);
            SearchByIdAccesGroupe(ref accesApplicationDTOs, queryParameters.IdAccesGroupe);
            SearchByIdUtilisateur(ref accesApplicationDTOs, queryParameters.IdUtilisateur);
            SortHelper.ApplySort(ref accesApplicationDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<AccesUtilisateurDTO>.ApplyPagination(ref accesApplicationDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var accesApplications = FromDTO(accesApplicationDTOs);
            return PagedList<AccesUtilisateur>.ToPagedList(accesApplications, totalCount);
        }

        private void SearchByEstActif(ref IQueryable<AccesUtilisateurDTO> accesApplications, bool? estActif)
        {
            if (!estActif.HasValue) return;
            accesApplications = accesApplications.Where(x => x.EstActif == estActif);
        }

        private void SearchByIdAccesGroupe(ref IQueryable<AccesUtilisateurDTO> accesApplications, int? idAccesGroupe)
        {
            if (idAccesGroupe is null) return;
            accesApplications = accesApplications.Where(x => x.IdAccesGroupe == idAccesGroupe);
        }

        private void SearchByIdUtilisateur(ref IQueryable<AccesUtilisateurDTO> accesApplications, int? idUtilisateur)
        {
            if (idUtilisateur is null) return;
            accesApplications = accesApplications.Where(x => x.IdUtilisateur == idUtilisateur);
        }

        public IBaseClass Create(AccesUtilisateur accesApplication)
        {
            AccesUtilisateurDTO accesApplicationDTO = ToDTO(accesApplication);
            _context.AccesUtilisateur.Add(accesApplicationDTO);
            return accesApplicationDTO;
        }

        public IBaseClass Update(AccesUtilisateur accesApplication)
        {
            AccesUtilisateurDTO accesApplicationDTO = _context.AccesUtilisateur.Find(accesApplication.Id);
            ApplyChanges(accesApplicationDTO, accesApplication);
            _context.AccesUtilisateur.Update(accesApplicationDTO);
            return accesApplicationDTO;
        }

        public bool Delete(AccesUtilisateur accesApplication)
        {
            AccesUtilisateurDTO accesApplicationDTO = _context.AccesUtilisateur.Find(accesApplication.Id);

            _context.AccesUtilisateur.Remove(accesApplicationDTO);

            return true;
        }

        public AccesUtilisateurDTO ToDTO(AccesUtilisateur accesApplication)
        {
            AccesUtilisateurDTO dto = new AccesUtilisateurDTO(accesApplication.Id, accesApplication.EstActif, accesApplication.IdAccesGroupe, accesApplication.IdUtilisateur);
            return dto;
        }

        public static AccesUtilisateur FromDTO(AccesUtilisateurDTO dto)
        {
            AccesUtilisateur accesApplication = AccesUtilisateur.Init(dto.Id, dto.EstActif, dto.IdAccesGroupe, dto.IdUtilisateur);
            return accesApplication;
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
