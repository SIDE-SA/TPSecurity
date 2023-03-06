using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetAllRefApplication;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class RefApplicationRepository : IRefApplicationRepository
    {
        private readonly ApplicationContextGTP _context;

        public RefApplicationRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public RefApplication? GetById(int id)
        {
            return _context.RefApplication.Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public RefApplication? GetByIdWithReferences(int id)
        {
            return _context.RefApplication 
                    .AsSplitQuery()
                    .Include(x => x.RefModules)
                    .Include(x => x.AccesApplications)
                    .Where(x => x.Id == id)
                    .Select(x => FromDTO(x))
                    .SingleOrDefault();
        }

        public RefApplication? GetByLibelle(string libelle)
        {
            return _context.RefApplication.Where(x => x.Libelle.ToLower() == libelle.Trim().ToLower()).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public PagedList<RefApplication> GetRefApplications(RefApplicationParameters queryParameters)
        {
            var refApplicationDTOs = _context.RefApplication.AsQueryable();

            SearchByLibelle(ref refApplicationDTOs, queryParameters.Libelle);
            SearchByEstActif(ref refApplicationDTOs, queryParameters.EstActif);
            SortHelper.ApplySort(ref refApplicationDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<RefApplicationDTO>.ApplyPagination(ref refApplicationDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var refApplications = FromDTO(refApplicationDTOs);
            return PagedList<RefApplication>.ToPagedList(refApplications, totalCount);
        }

        private void SearchByLibelle(ref IQueryable<RefApplicationDTO> refApplications, string? libelle)
        {
            if (string.IsNullOrWhiteSpace(libelle)) return;
            refApplications = refApplications.Where(x => x.Libelle.ToLower().Contains(libelle.Trim().ToLower()));
        }

        private void SearchByEstActif(ref IQueryable<RefApplicationDTO> refApplications, bool? estActif)
        {
            if (!estActif.HasValue) return;
            refApplications = refApplications.Where(x => x.EstActif == estActif);
        }

        public IBaseClass Create(RefApplication refApplication)
        {
            RefApplicationDTO refApplicationDTO = ToDTO(refApplication);
            _context.RefApplication.Add(refApplicationDTO);
            return refApplicationDTO;
        }

        public IBaseClass Update(RefApplication refApplication)
        {
            RefApplicationDTO refApplicationDTO = _context.RefApplication.Find(refApplication.Id);
            ApplyChanges(refApplicationDTO, refApplication);
            _context.RefApplication.Update(refApplicationDTO);
            return refApplicationDTO;
        }

        public bool Delete(RefApplication refApplication)
        {
            RefApplicationDTO refApplicationDTO = _context.RefApplication.Find(refApplication.Id);

            if (refApplicationDTO.RefModules.Any())
                return false;

            if (refApplicationDTO.AccesApplications.Any())
                return false;

            _context.RefApplication.Remove(refApplicationDTO);

            return true;
        }

        public RefApplicationDTO ToDTO(RefApplication refApplication)
        {
            RefApplicationDTO dto = new RefApplicationDTO(refApplication.Id, refApplication.Libelle, refApplication.EstActif);
            return dto;
        }

        public static RefApplication FromDTO(RefApplicationDTO dto)
        {
            RefApplication refApplication = RefApplication.Init(dto.Id, dto.Libelle, dto.EstActif);
            return refApplication;
        }

        public static IEnumerable<RefApplication> FromDTO(IQueryable<RefApplicationDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static void ApplyChanges(RefApplicationDTO dest, RefApplication source)
        {
            if (dest is null || source is null) return;
            dest.Libelle = source.Libelle;
            dest.EstActif = source.EstActif;
        }

       
    }
}
