using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetAllRefFonctionnalite;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class RefFonctionnaliteRepository : IRefFonctionnaliteRepository
    {
        private readonly ApplicationContextGTP _context;

        public RefFonctionnaliteRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public RefFonctionnalite? GetById(int id)
        {
            return _context.RefFonctionnalite.Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public RefFonctionnalite? GetByIdWithReferences(int id)
        {
            return _context.RefFonctionnalite 
                    .AsSplitQuery()
                    .Include(x => x.AccesFonctionnalites)
                    .Where(x => x.Id == id)
                    .Select(x => FromDTO(x))
                    .SingleOrDefault();
        }

        public RefFonctionnalite? GetByLibelle(string libelle)
        {
            return _context.RefFonctionnalite.Where(x => x.Libelle.ToLower() == libelle.Trim().ToLower()).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public PagedList<RefFonctionnalite> GetRefFonctionnalites(RefFonctionnaliteParameters queryParameters)
        {
            var refFonctionnaliteDTOs = _context.RefFonctionnalite.AsQueryable();

            SearchByLibelle(ref refFonctionnaliteDTOs, queryParameters.Libelle);
            SearchByEstActif(ref refFonctionnaliteDTOs, queryParameters.EstActif);
            SearchByRefModule(ref refFonctionnaliteDTOs, queryParameters.IdRefModule);
            SortHelper.ApplySort(ref refFonctionnaliteDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<RefFonctionnaliteDTO>.ApplyPagination(ref refFonctionnaliteDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var refFonctionnalites = FromDTO(refFonctionnaliteDTOs);
            return PagedList<RefFonctionnalite>.ToPagedList(refFonctionnalites, totalCount);
        }

        private void SearchByLibelle(ref IQueryable<RefFonctionnaliteDTO> refFonctionnalites, string? libelle)
        {
            if (string.IsNullOrWhiteSpace(libelle)) return;
            refFonctionnalites = refFonctionnalites.Where(x => x.Libelle.ToLower().Contains(libelle.Trim().ToLower()));
        }

        private void SearchByRefModule(ref IQueryable<RefFonctionnaliteDTO> refFonctionnalites, int? idRefModule)
        {
            if (idRefModule is null) return;
            refFonctionnalites = refFonctionnalites.Where(x => x.IdRefModule == idRefModule);
        }

        private void SearchByEstActif(ref IQueryable<RefFonctionnaliteDTO> refFonctionnalites, bool? estActif)
        {
            if (!estActif.HasValue) return;
            refFonctionnalites = refFonctionnalites.Where(x => x.EstActif == estActif);
        }

        public IBaseClass Create(RefFonctionnalite refFonctionnalite)
        {
            RefFonctionnaliteDTO refFonctionnaliteDTO = ToDTO(refFonctionnalite);
            _context.RefFonctionnalite.Add(refFonctionnaliteDTO);
            return refFonctionnaliteDTO;
        }

        public IBaseClass Update(RefFonctionnalite refFonctionnalite)
        {
            RefFonctionnaliteDTO contactBanqueDTO = _context.RefFonctionnalite.Find(refFonctionnalite.Id);
            ApplyChanges(contactBanqueDTO, refFonctionnalite);
            _context.RefFonctionnalite.Update(contactBanqueDTO);
            return contactBanqueDTO;
        }

        public bool Delete(RefFonctionnalite refFonctionnalite)
        {
            RefFonctionnaliteDTO refFonctionnaliteDTO = _context.RefFonctionnalite.Find(refFonctionnalite.Id);

            if (refFonctionnaliteDTO.AccesFonctionnalites.Any())
                return false;

            _context.RefFonctionnalite.Remove(refFonctionnaliteDTO);

            return true;
        }

        public RefFonctionnaliteDTO ToDTO(RefFonctionnalite refFonctionnalite)
        {
            RefFonctionnaliteDTO dto = new RefFonctionnaliteDTO(refFonctionnalite.Id, refFonctionnalite.Libelle, refFonctionnalite.EstActif,
                refFonctionnalite.EstDefaut, refFonctionnalite.Permission, refFonctionnalite.IdRefModule);
            return dto;
        }

        public static RefFonctionnalite FromDTO(RefFonctionnaliteDTO dto)
        {
            RefFonctionnalite refFonctionnalite = RefFonctionnalite.Init(dto.Id, dto.Libelle, dto.EstActif, dto.EstDefaut, dto.Permission, dto.IdRefModule);
            return refFonctionnalite;
        }

        public static IEnumerable<RefFonctionnalite> FromDTO(IQueryable<RefFonctionnaliteDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static void ApplyChanges(RefFonctionnaliteDTO dest, RefFonctionnalite source)
        {
            if (dest is null || source is null) return;
            dest.Libelle = source.Libelle;
            dest.EstActif = source.EstActif;
        }

       
    }
}
