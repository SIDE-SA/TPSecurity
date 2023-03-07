using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetAllAccesApplication;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class AccesApplicationRepository : IAccesApplicationRepository
    {
        private readonly ApplicationContextGTP _context;

        public AccesApplicationRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public AccesApplication? GetById(int id)
        {
            return _context.AccesApplication
                    .Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public AccesApplication? GetByUnicite(int idAccesGroupe, int idRefApplication)
        {
            return _context.AccesApplication
                    .Where(x => x.IdAccesGroupe == idAccesGroupe && x.IdRefApplication == idRefApplication).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public AccesApplication? GetByIdWithReferences(int id)
        {
            return _context.AccesApplication 
                    .AsSplitQuery()
                    .Include(x => x.AccesModules)
                    .Where(x => x.Id == id)
                    .Select(x => FromDTO(x))
                    .SingleOrDefault();
        }

        public PagedList<AccesApplication> GetAccesApplications(AccesApplicationParameters queryParameters)
        {
            var accesApplicationDTOs = _context.AccesApplication
                    .AsQueryable();

            SearchByEstActif(ref accesApplicationDTOs, queryParameters.EstActif);
            SearchByIdAccesGroupe(ref accesApplicationDTOs, queryParameters.IdAccesGroupe);
            SearchByIdRefApplication(ref accesApplicationDTOs, queryParameters.IdRefApplication);
            SortHelper.ApplySort(ref accesApplicationDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<AccesApplicationDTO>.ApplyPagination(ref accesApplicationDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var accesApplications = FromDTO(accesApplicationDTOs);
            return PagedList<AccesApplication>.ToPagedList(accesApplications, totalCount);
        }

        private void SearchByEstActif(ref IQueryable<AccesApplicationDTO> accesApplications, bool? estActif)
        {
            if (!estActif.HasValue) return;
            accesApplications = accesApplications.Where(x => x.EstActif == estActif);
        }

        private void SearchByIdAccesGroupe(ref IQueryable<AccesApplicationDTO> accesApplications, int? idAccesGroupe)
        {
            if (idAccesGroupe is null) return;
            accesApplications = accesApplications.Where(x => x.IdAccesGroupe == idAccesGroupe);
        }

        private void SearchByIdRefApplication(ref IQueryable<AccesApplicationDTO> accesApplications, int? idRefApplication)
        {
            if (idRefApplication is null) return;
            accesApplications = accesApplications.Where(x => x.IdRefApplication == idRefApplication);
        }

        public IBaseClass Create(AccesApplication accesApplication)
        {
            AccesApplicationDTO accesApplicationDTO = ToDTO(accesApplication);
            _context.AccesApplication.Add(accesApplicationDTO);
            return accesApplicationDTO;
        }

        public IBaseClass Update(AccesApplication accesApplication)
        {
            AccesApplicationDTO accesApplicationDTO = _context.AccesApplication.Find(accesApplication.Id);
            ApplyChanges(accesApplicationDTO, accesApplication);
            _context.AccesApplication.Update(accesApplicationDTO);
            return accesApplicationDTO;
        }

        public bool Delete(AccesApplication accesApplication)
        {
            AccesApplicationDTO accesApplicationDTO = _context.AccesApplication.Find(accesApplication.Id);

            if (accesApplicationDTO.AccesModules.Any())
                return false;

            _context.AccesApplication.Remove(accesApplicationDTO);

            return true;
        }

        public AccesApplicationDTO ToDTO(AccesApplication accesApplication)
        {
            AccesApplicationDTO dto = new AccesApplicationDTO(accesApplication.Id, accesApplication.EstActif, accesApplication.IdAccesGroupe, accesApplication.IdRefApplication);
            return dto;
        }

        public static AccesApplication FromDTO(AccesApplicationDTO dto)
        {
            AccesApplication accesApplication = AccesApplication.Init(dto.Id, dto.EstActif, dto.IdAccesGroupe, dto.IdRefApplication);
            return accesApplication;
        }

        public static IEnumerable<AccesApplication> FromDTO(IQueryable<AccesApplicationDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static void ApplyChanges(AccesApplicationDTO dest, AccesApplication source)
        {
            if (dest is null || source is null) return;
            dest.EstActif = source.EstActif;
        }       
    }
}
