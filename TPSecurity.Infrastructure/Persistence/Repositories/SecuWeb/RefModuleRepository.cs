using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetAllRefModule;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class RefModuleRepository : IRefModuleRepository
    {
        private readonly ApplicationContextGTP _context;

        public RefModuleRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public RefModule? GetById(int id)
        {
            return _context.RefModule
                    .Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public RefModule? GetByIdWithReferences(int id)
        {
            return _context.RefModule 
                    .AsSplitQuery()
                    .Include(x => x.RefFonctionnalites)
                    .Include(x => x.AccesModules)
                    .Where(x => x.Id == id)
                    .Select(x => FromDTO(x))
                    .SingleOrDefault();
        }

        public RefModule? GetByLibelle(string libelle)
        {
            return _context.RefModule
                    .Where(x => x.Libelle.ToLower() == libelle.Trim().ToLower()).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public PagedList<RefModule> GetRefModules(RefModuleParameters queryParameters)
        {
            var refModuleDTOs = _context.RefModule
                    .AsQueryable();

            SearchByLibelle(ref refModuleDTOs, queryParameters.Libelle);
            SearchByEstActif(ref refModuleDTOs, queryParameters.EstActif);
            SortHelper.ApplySort(ref refModuleDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<RefModuleDTO>.ApplyPagination(ref refModuleDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var refModules = FromDTO(refModuleDTOs);
            return PagedList<RefModule>.ToPagedList(refModules, totalCount);
        }

        private void SearchByLibelle(ref IQueryable<RefModuleDTO> refModules, string? libelle)
        {
            if (string.IsNullOrWhiteSpace(libelle)) return;
            refModules = refModules.Where(x => x.Libelle.ToLower().Contains(libelle.Trim().ToLower()));
        }

        private void SearchByEstActif(ref IQueryable<RefModuleDTO> refModules, bool? estActif)
        {
            if (!estActif.HasValue) return;
            refModules = refModules.Where(x => x.EstActif == estActif);
        }

        public IBaseClass Create(RefModule refModule)
        {
            RefModuleDTO refModuleDTO = ToDTO(refModule);
            _context.RefModule.Add(refModuleDTO);
            return refModuleDTO;
        }

        public IBaseClass Update(RefModule refModule)
        {
            RefModuleDTO refModuleDTO = _context.RefModule.Find(refModule.Id);
            ApplyChanges(refModuleDTO, refModule);
            _context.RefModule.Update(refModuleDTO);
            return refModuleDTO;
        }

        public bool Delete(RefModule refModule)
        {
            RefModuleDTO refModuleDTO = _context.RefModule.Find(refModule.Id);

            if (refModuleDTO.RefFonctionnalites.Any())
                return false;

            if (refModuleDTO.AccesModules.Any())
                return false;

            _context.RefModule.Remove(refModuleDTO);

            return true;
        }

        public RefModuleDTO ToDTO(RefModule refModule)
        {
            RefModuleDTO dto = new RefModuleDTO(refModule.Id, refModule.Libelle, refModule.EstActif, refModule.IdRefApplication);
            return dto;
        }

        public static RefModule FromDTO(RefModuleDTO dto)
        {
            RefModule refModule = RefModule.Init(dto.Id, dto.Libelle, dto.EstActif, dto.IdRefApplication);
            return refModule;
        }

        public static IEnumerable<RefModule> FromDTO(IQueryable<RefModuleDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static void ApplyChanges(RefModuleDTO dest, RefModule source)
        {
            if (dest is null || source is null) return;
            dest.Libelle = source.Libelle;
            dest.EstActif = source.EstActif;
        }

       
    }
}
