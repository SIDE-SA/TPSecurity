using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetAllAccesModule;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class AccesModuleRepository : IAccesModuleRepository
    {
        private readonly ApplicationContextGTP _context;

        public AccesModuleRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public AccesModule? GetById(int id)
        {
            return _context.AccesModule
                    .Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public AccesModule? GetByUnicite(int idAccesApplication, int idRefModule)
        {
            return _context.AccesModule
                    .Where(x => x.IdAccesApplication == idAccesApplication && x.IdRefModule == idRefModule).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public AccesModule? GetByIdWithReferences(int id)
        {
            return _context.AccesModule 
                    .AsSplitQuery()
                    .Include(x => x.AccesFonctionnalites)
                    .Where(x => x.Id == id)
                    .Select(x => FromDTO(x))
                    .SingleOrDefault();
        }

        public PagedList<AccesModule> GetAccesModules(AccesModuleParameters queryParameters)
        {
            var accesApplicationDTOs = _context.AccesModule
                    .AsQueryable();

            SearchByEstActif(ref accesApplicationDTOs, queryParameters.EstActif);
            SearchByIdAccesApplication(ref accesApplicationDTOs, queryParameters.IdAccesApplication);
            SearchByIdRefModule(ref accesApplicationDTOs, queryParameters.IdRefModule);
            SortHelper.ApplySort(ref accesApplicationDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<AccesModuleDTO>.ApplyPagination(ref accesApplicationDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var accesApplications = FromDTO(accesApplicationDTOs);
            return PagedList<AccesModule>.ToPagedList(accesApplications, totalCount);
        }

        private void SearchByEstActif(ref IQueryable<AccesModuleDTO> accesApplications, bool? estActif)
        {
            if (!estActif.HasValue) return;
            accesApplications = accesApplications.Where(x => x.EstActif == estActif);
        }

        private void SearchByIdAccesApplication(ref IQueryable<AccesModuleDTO> accesApplications, int? idAccesApplication)
        {
            if (idAccesApplication is null) return;
            accesApplications = accesApplications.Where(x => x.IdAccesApplication == idAccesApplication);
        }

        private void SearchByIdRefModule(ref IQueryable<AccesModuleDTO> accesApplications, int? idRefModule)
        {
            if (idRefModule is null) return;
            accesApplications = accesApplications.Where(x => x.IdRefModule == idRefModule);
        }

        public IBaseClass Create(AccesModule accesApplication)
        {
            AccesModuleDTO accesApplicationDTO = ToDTO(accesApplication);
            _context.AccesModule.Add(accesApplicationDTO);
            return accesApplicationDTO;
        }

        public IBaseClass Update(AccesModule accesApplication)
        {
            AccesModuleDTO accesApplicationDTO = _context.AccesModule.Find(accesApplication.Id);
            ApplyChanges(accesApplicationDTO, accesApplication);
            _context.AccesModule.Update(accesApplicationDTO);
            return accesApplicationDTO;
        }

        public bool Delete(AccesModule accesApplication)
        {
            AccesModuleDTO accesApplicationDTO = _context.AccesModule.Find(accesApplication.Id);

            if (accesApplicationDTO.AccesFonctionnalites.Any())
                return false;

            _context.AccesModule.Remove(accesApplicationDTO);

            return true;
        }

        public AccesModuleDTO ToDTO(AccesModule accesApplication)
        {
            AccesModuleDTO dto = new AccesModuleDTO(accesApplication.Id, accesApplication.EstActif, accesApplication.IdAccesApplication, accesApplication.IdRefModule);
            return dto;
        }

        public static AccesModule FromDTO(AccesModuleDTO dto)
        {
            AccesModule accesApplication = AccesModule.Init(dto.Id, dto.EstActif, dto.IdAccesApplication, dto.IdRefModule);
            return accesApplication;
        }

        public static IEnumerable<AccesModule> FromDTO(IQueryable<AccesModuleDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static void ApplyChanges(AccesModuleDTO dest, AccesModule source)
        {
            if (dest is null || source is null) return;
            dest.EstActif = source.EstActif;
        }       
    }
}
