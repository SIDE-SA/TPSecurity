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
            var accesModuleDTOs = _context.AccesModule
                    .AsQueryable();

            SearchByEstActif(ref accesModuleDTOs, queryParameters.EstActif);
            SearchByIdAccesApplication(ref accesModuleDTOs, queryParameters.IdAccesApplication);
            SearchByIdRefModule(ref accesModuleDTOs, queryParameters.IdRefModule);
            SortHelper.ApplySort(ref accesModuleDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<AccesModuleDTO>.ApplyPagination(ref accesModuleDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var accesModules = FromDTO(accesModuleDTOs);
            return PagedList<AccesModule>.ToPagedList(accesModules, totalCount);
        }

        private void SearchByEstActif(ref IQueryable<AccesModuleDTO> accesModules, bool? estActif)
        {
            if (!estActif.HasValue) return;
            accesModules = accesModules.Where(x => x.EstActif == estActif);
        }

        private void SearchByIdAccesApplication(ref IQueryable<AccesModuleDTO> accesModules, int? idAccesApplication)
        {
            if (idAccesApplication is null) return;
            accesModules = accesModules.Where(x => x.IdAccesApplication == idAccesApplication);
        }

        private void SearchByIdRefModule(ref IQueryable<AccesModuleDTO> accesModules, int? idRefModule)
        {
            if (idRefModule is null) return;
            accesModules = accesModules.Where(x => x.IdRefModule == idRefModule);
        }

        public IBaseClass Create(AccesModule accesModule)
        {
            AccesModuleDTO accesModuleDTO = ToDTO(accesModule);
            _context.AccesModule.Add(accesModuleDTO);
            return accesModuleDTO;
        }

        public IBaseClass Update(AccesModule accesModule)
        {
            AccesModuleDTO accesModuleDTO = _context.AccesModule.Find(accesModule.Id);
            ApplyChanges(accesModuleDTO, accesModule);
            _context.AccesModule.Update(accesModuleDTO);
            return accesModuleDTO;
        }

        public bool Delete(AccesModule accesModule)
        {
            AccesModuleDTO accesModuleDTO = _context.AccesModule.Find(accesModule.Id);

            if (accesModuleDTO.AccesFonctionnalites.Any())
                return false;

            _context.AccesModule.Remove(accesModuleDTO);

            return true;
        }

        public AccesModuleDTO ToDTO(AccesModule accesModule)
        {
            AccesModuleDTO dto = new AccesModuleDTO(accesModule.Id, accesModule.EstActif, accesModule.IdAccesApplication, accesModule.IdRefModule);
            return dto;
        }

        public static AccesModule FromDTO(AccesModuleDTO dto)
        {
            AccesModule accesModule = AccesModule.Init(dto.Id, dto.EstActif, dto.IdAccesApplication, dto.IdRefModule,
                                                          dto.AccesFonctionnalites != null ?
                                                          AccesFonctionnaliteRepository.GetRefFonctionnaliteFromAccesModule(dto.AccesFonctionnalites.AsQueryable())
                                                          : null);
            return accesModule;
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
