using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetAllAccesGroupe;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class AccesGroupeRepository : IAccesGroupeRepository
    {
        private readonly ApplicationContextGTP _context;

        public AccesGroupeRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public AccesGroupe? GetById(int id)
        {
            return _context.AccesGroupe
                    .Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public AccesGroupe? GetByIdWithReferences(int id)
        {
            return _context.AccesGroupe 
                    .AsSplitQuery()
                    .Include(x => x.AccesApplications)
                    .Include(x => x.AccesUtilisateurs)
                    .Where(x => x.Id == id)
                    .Select(x => FromDTO(x))
                    .SingleOrDefault();
        }

        public AccesGroupe? GetByLibelle(string libelle)
        {
            return _context.AccesGroupe
                    .Where(x => x.Libelle.ToLower() == libelle.Trim().ToLower()).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public PagedList<AccesGroupe> GetAccesGroupes(AccesGroupeParameters queryParameters)
        {
            var accesGroupeDTOs = _context.AccesGroupe
                    .AsQueryable();

            SearchByLibelle(ref accesGroupeDTOs, queryParameters.Libelle);
            SearchByEstActif(ref accesGroupeDTOs, queryParameters.EstActif);
            SortHelper.ApplySort(ref accesGroupeDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<AccesGroupeDTO>.ApplyPagination(ref accesGroupeDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var accesGroupes = FromDTO(accesGroupeDTOs);
            return PagedList<AccesGroupe>.ToPagedList(accesGroupes, totalCount);
        }

        private void SearchByLibelle(ref IQueryable<AccesGroupeDTO> accesGroupes, string? libelle)
        {
            if (string.IsNullOrWhiteSpace(libelle)) return;
            accesGroupes = accesGroupes.Where(x => x.Libelle.ToLower().Contains(libelle.Trim().ToLower()));
        }

        private void SearchByEstActif(ref IQueryable<AccesGroupeDTO> accesGroupes, bool? estActif)
        {
            if (!estActif.HasValue) return;
            accesGroupes = accesGroupes.Where(x => x.EstActif == estActif);
        }

        public IBaseClass Create(AccesGroupe accesGroupe)
        {
            AccesGroupeDTO accesGroupeDTO = ToDTO(accesGroupe);
            _context.AccesGroupe.Add(accesGroupeDTO);
            return accesGroupeDTO;
        }

        public IBaseClass Update(AccesGroupe accesGroupe)
        {
            AccesGroupeDTO accesGroupeDTO = _context.AccesGroupe.Find(accesGroupe.Id);
            ApplyChanges(accesGroupeDTO, accesGroupe);
            _context.AccesGroupe.Update(accesGroupeDTO);
            return accesGroupeDTO;
        }

        public bool Delete(AccesGroupe accesGroupe)
        {
            AccesGroupeDTO accesGroupeDTO = _context.AccesGroupe.Find(accesGroupe.Id);

            if (accesGroupeDTO.AccesUtilisateurs.Any())
                return false;

            if (accesGroupeDTO.AccesApplications.Any())
                return false;

            _context.AccesGroupe.Remove(accesGroupeDTO);

            return true;
        }

        public AccesGroupeDTO ToDTO(AccesGroupe accesGroupe)
        {
            AccesGroupeDTO dto = new AccesGroupeDTO(accesGroupe.Id, accesGroupe.Libelle, accesGroupe.EstActif, accesGroupe.EstGroupeSpecial, accesGroupe.IdSociete);
            return dto;
        }

        public static AccesGroupe FromDTO(AccesGroupeDTO dto)
        {
            AccesGroupe accesGroupe = AccesGroupe.Init(dto.Id, dto.Libelle, dto.EstActif, dto.EstGroupeSpecial, dto.IdSociete);
            return accesGroupe;
        }

        public static IEnumerable<AccesGroupe> FromDTO(IQueryable<AccesGroupeDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static void ApplyChanges(AccesGroupeDTO dest, AccesGroupe source)
        {
            if (dest is null || source is null) return;
            dest.Libelle = source.Libelle;
            dest.EstActif = source.EstActif;
        }

       
    }
}
