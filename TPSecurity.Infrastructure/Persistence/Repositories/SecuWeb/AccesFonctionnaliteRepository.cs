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
            var accesFonctionnaliteDTOs = _context.AccesFonctionnalite
                    .AsQueryable();

            SearchByEstActif(ref accesFonctionnaliteDTOs, queryParameters.EstActif);
            SearchByIdAccesModule(ref accesFonctionnaliteDTOs, queryParameters.IdAccesModule);
            SearchByIdRefFonctionnalite(ref accesFonctionnaliteDTOs, queryParameters.IdRefFonctionnalite);
            SortHelper.ApplySort(ref accesFonctionnaliteDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<AccesFonctionnaliteDTO>.ApplyPagination(ref accesFonctionnaliteDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var accesFonctionnalites = FromDTO(accesFonctionnaliteDTOs);
            return PagedList<AccesFonctionnalite>.ToPagedList(accesFonctionnalites, totalCount);
        }

        private void SearchByEstActif(ref IQueryable<AccesFonctionnaliteDTO> accesFonctionnalites, bool? estActif)
        {
            if (!estActif.HasValue) return;
            accesFonctionnalites = accesFonctionnalites.Where(x => x.EstActif == estActif);
        }

        private void SearchByIdAccesModule(ref IQueryable<AccesFonctionnaliteDTO> accesFonctionnalites, int? idAccesModule)
        {
            if (idAccesModule is null) return;
            accesFonctionnalites = accesFonctionnalites.Where(x => x.IdAccesModule == idAccesModule);
        }

        private void SearchByIdRefFonctionnalite(ref IQueryable<AccesFonctionnaliteDTO> accesFonctionnalites, int? idRefFonctionnalite)
        {
            if (idRefFonctionnalite is null) return;
            accesFonctionnalites = accesFonctionnalites.Where(x => x.IdRefFonctionnalite == idRefFonctionnalite);
        }

        public IBaseClass Create(AccesFonctionnalite accesFonctionnalite)
        {
            AccesFonctionnaliteDTO accesFonctionnaliteDTO = ToDTO(accesFonctionnalite);
            _context.AccesFonctionnalite.Add(accesFonctionnaliteDTO);
            return accesFonctionnaliteDTO;
        }

        public IBaseClass Update(AccesFonctionnalite accesFonctionnalite)
        {
            AccesFonctionnaliteDTO accesFonctionnaliteDTO = _context.AccesFonctionnalite.Find(accesFonctionnalite.Id);
            ApplyChanges(accesFonctionnaliteDTO, accesFonctionnalite);
            _context.AccesFonctionnalite.Update(accesFonctionnaliteDTO);
            return accesFonctionnaliteDTO;
        }

        public bool Delete(AccesFonctionnalite accesFonctionnalite)
        {
            AccesFonctionnaliteDTO accesFonctionnaliteDTO = _context.AccesFonctionnalite.Find(accesFonctionnalite.Id);

            _context.AccesFonctionnalite.Remove(accesFonctionnaliteDTO);

            return true;
        }

        public AccesFonctionnaliteDTO ToDTO(AccesFonctionnalite accesFonctionnalite)
        {
            AccesFonctionnaliteDTO dto = new AccesFonctionnaliteDTO(accesFonctionnalite.Id, accesFonctionnalite.EstActif, accesFonctionnalite.IdAccesModule, accesFonctionnalite.IdRefFonctionnalite);
            return dto;
        }

        public static AccesFonctionnalite FromDTO(AccesFonctionnaliteDTO dto)
        {
            AccesFonctionnalite accesFonctionnalite = AccesFonctionnalite.Init(dto.Id, dto.EstActif, dto.IdAccesModule, dto.IdRefFonctionnalite);
            return accesFonctionnalite;
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
