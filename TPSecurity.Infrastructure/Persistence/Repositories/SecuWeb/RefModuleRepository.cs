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
            return _context.RefModule.Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
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
            return _context.RefModule.Where(x => x.Libelle.ToLower() == libelle.Trim().ToLower()).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public PagedList<RefModule> GetRefModules(RefModuleParameters queryParameters)
        {
            var refApplicationDTOs = _context.RefModule.AsQueryable();

            SearchByLibelle(ref refApplicationDTOs, queryParameters.Libelle);
            SearchByEstActif(ref refApplicationDTOs, queryParameters.EstActif);
            SortHelper.ApplySort(ref refApplicationDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<RefModuleDTO>.ApplyPagination(ref refApplicationDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var refApplications = FromDTO(refApplicationDTOs);
            return PagedList<RefModule>.ToPagedList(refApplications, totalCount);
        }

        private void SearchByLibelle(ref IQueryable<RefModuleDTO> refApplications, string? libelle)
        {
            if (string.IsNullOrWhiteSpace(libelle)) return;
            refApplications = refApplications.Where(x => x.Libelle.ToLower().Contains(libelle.Trim().ToLower()));
        }

        private void SearchByEstActif(ref IQueryable<RefModuleDTO> refApplications, bool? estActif)
        {
            if (!estActif.HasValue) return;
            refApplications = refApplications.Where(x => x.EstActif == estActif);
        }

        public IBaseClass Create(RefModule refApplication)
        {
            RefModuleDTO refApplicationDTO = ToDTO(refApplication);
            _context.RefModule.Add(refApplicationDTO);
            return refApplicationDTO;
        }

        public IBaseClass Update(RefModule refApplication)
        {
            RefModuleDTO contactBanqueDTO = _context.RefModule.Find(refApplication.Id);
            ApplyChanges(contactBanqueDTO, refApplication);
            _context.RefModule.Update(contactBanqueDTO);
            return contactBanqueDTO;
        }

        public bool Delete(RefModule refApplication)
        {
            RefModuleDTO refApplicationDTO = _context.RefModule.Find(refApplication.Id);

            if (refApplicationDTO.RefFonctionnalites.Any())
                return false;

            if (refApplicationDTO.AccesModules.Any())
                return false;

            _context.RefModule.Remove(refApplicationDTO);

            return true;
        }

        public RefModuleDTO ToDTO(RefModule refApplication)
        {
            RefModuleDTO dto = new RefModuleDTO(refApplication.Id, refApplication.Libelle, refApplication.EstActif, refApplication.IdRefApplication.Id);
            return dto;
        }

        public static RefModule FromDTO(RefModuleDTO dto)
        {
            RefModule refApplication = RefModule.Init(dto.Id, dto.Libelle, dto.EstActif, dto.IdRefApplication != null ?
                                                                        RefApplicationRepository.FromDTO(dto.IdRefApplicationNavigation)
                                                                        : null);
            return refApplication;
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
