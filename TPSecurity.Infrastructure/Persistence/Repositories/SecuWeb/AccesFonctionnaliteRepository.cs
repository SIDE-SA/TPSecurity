using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetAllAccesFonctionnalite;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class AccesFonctionnaliteRepository : IAccesFonctionnaliteRepository
    {
        private readonly ApplicationContextGTP _context;

        public AccesFonctionnaliteRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public AccesFonctionnalite? GetById(int id)
        {
            return _context.AccesFonctionnalite
                    .Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public AccesFonctionnalite? GetByUnicite(int idAccesModule, int idRefFonctionnalite)
        {
            return _context.AccesFonctionnalite
                    .Where(x => x.IdAccesModule == idAccesModule && x.IdRefFonctionnalite == idRefFonctionnalite).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public PagedList<AccesFonctionnalite> GetAccesFonctionnalites(AccesFonctionnaliteParameters queryParameters)
        {
            var accesApplicationDTOs = _context.AccesFonctionnalite
                    .AsQueryable();

            SearchByEstActif(ref accesApplicationDTOs, queryParameters.EstActif);
            SearchByIdAccesModule(ref accesApplicationDTOs, queryParameters.IdAccesModule);
            SearchByIdRefFonctionnalite(ref accesApplicationDTOs, queryParameters.IdRefFonctionnalite);
            SortHelper.ApplySort(ref accesApplicationDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<AccesFonctionnaliteDTO>.ApplyPagination(ref accesApplicationDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var accesApplications = FromDTO(accesApplicationDTOs);
            return PagedList<AccesFonctionnalite>.ToPagedList(accesApplications, totalCount);
        }

        private void SearchByEstActif(ref IQueryable<AccesFonctionnaliteDTO> accesApplications, bool? estActif)
        {
            if (!estActif.HasValue) return;
            accesApplications = accesApplications.Where(x => x.EstActif == estActif);
        }

        private void SearchByIdAccesModule(ref IQueryable<AccesFonctionnaliteDTO> accesApplications, int? idAccesModule)
        {
            if (idAccesModule is null) return;
            accesApplications = accesApplications.Where(x => x.IdAccesModule == idAccesModule);
        }

        private void SearchByIdRefFonctionnalite(ref IQueryable<AccesFonctionnaliteDTO> accesApplications, int? idRefFonctionnalite)
        {
            if (idRefFonctionnalite is null) return;
            accesApplications = accesApplications.Where(x => x.IdRefFonctionnalite == idRefFonctionnalite);
        }

        public IBaseClass Create(AccesFonctionnalite accesApplication)
        {
            AccesFonctionnaliteDTO accesApplicationDTO = ToDTO(accesApplication);
            _context.AccesFonctionnalite.Add(accesApplicationDTO);
            return accesApplicationDTO;
        }

        public IBaseClass Update(AccesFonctionnalite accesApplication)
        {
            AccesFonctionnaliteDTO accesApplicationDTO = _context.AccesFonctionnalite.Find(accesApplication.Id);
            ApplyChanges(accesApplicationDTO, accesApplication);
            _context.AccesFonctionnalite.Update(accesApplicationDTO);
            return accesApplicationDTO;
        }

        public bool Delete(AccesFonctionnalite accesApplication)
        {
            AccesFonctionnaliteDTO accesApplicationDTO = _context.AccesFonctionnalite.Find(accesApplication.Id);

            _context.AccesFonctionnalite.Remove(accesApplicationDTO);

            return true;
        }

        public AccesFonctionnaliteDTO ToDTO(AccesFonctionnalite accesApplication)
        {
            AccesFonctionnaliteDTO dto = new AccesFonctionnaliteDTO(accesApplication.Id, accesApplication.EstActif, accesApplication.IdAccesModule, accesApplication.IdRefFonctionnalite);
            return dto;
        }

        public static AccesFonctionnalite FromDTO(AccesFonctionnaliteDTO dto)
        {
            AccesFonctionnalite accesApplication = AccesFonctionnalite.Init(dto.Id, dto.EstActif, dto.IdAccesModule, dto.IdRefFonctionnalite);
            return accesApplication;
        }

        public static IEnumerable<AccesFonctionnalite> FromDTO(IQueryable<AccesFonctionnaliteDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static IEnumerable<RefFonctionnalite> GetRefFonctionnaliteFromAccesModule(IQueryable<AccesFonctionnaliteDTO> dto)
        {
            foreach (AccesFonctionnaliteDTO item in dto)
            {
                yield return RefFonctionnaliteRepository.FromDTO(item.IdRefFonctionnaliteNavigation);
            }
        }

        public static void ApplyChanges(AccesFonctionnaliteDTO dest, AccesFonctionnalite source)
        {
            if (dest is null || source is null) return;
            dest.EstActif = source.EstActif;
        }       
    }
}
