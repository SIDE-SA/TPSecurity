using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetAllUtilisateur;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Services;

namespace TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly ApplicationContextGTP _context;

        public UtilisateurRepository(ApplicationContextGTP context)
        {
            _context = context;
        }

        public Utilisateur? GetById(int id)
        {
            return _context.Utilisateur
                    .Where(x => x.Id == id).Select(x => FromDTO(x)).SingleOrDefault();            
        }

        public Utilisateur? GetByIdWithReferences(int id)
        {
            return _context.Utilisateur 
                    .AsSplitQuery()
                    .Include(x => x.AccesUtilisateurs)
                    .Where(x => x.Id == id)
                    .Select(x => FromDTO(x))
                    .SingleOrDefault();
        }

        public Utilisateur? GetByEmail(string email)
        {
            return _context.Utilisateur
                    .Where(x => x.Email.ToLower() == email.Trim().ToLower()).Select(x => FromDTO(x)).SingleOrDefault();
        }

        public PagedList<Utilisateur> GetUtilisateurs(UtilisateurParameters queryParameters)
        {
            var utilisateurDTOs = _context.Utilisateur
                    .AsQueryable();

            SearchByNom(ref utilisateurDTOs, queryParameters.Nom);
            SearchByPrenom(ref utilisateurDTOs, queryParameters.Prenom);
            SearchByEmail(ref utilisateurDTOs, queryParameters.Email);
            SearchByEstActif(ref utilisateurDTOs, queryParameters.EstActif);
            SortHelper.ApplySort(ref utilisateurDTOs, queryParameters.orderBy, queryParameters.orderOrientation);
            PagedList<UtilisateurDTO>.ApplyPagination(ref utilisateurDTOs, queryParameters.offSet, queryParameters.limit, out int totalCount);
            var utilisateurs = FromDTO(utilisateurDTOs);
            return PagedList<Utilisateur>.ToPagedList(utilisateurs, totalCount);
        }

        private void SearchByNom(ref IQueryable<UtilisateurDTO> utilisateurs, string? nom)
        {
            if (string.IsNullOrWhiteSpace(nom)) return;
            utilisateurs = utilisateurs.Where(x => x.Nom.ToLower().Contains(nom.Trim().ToLower()));
        }

        private void SearchByPrenom(ref IQueryable<UtilisateurDTO> utilisateurs, string? prenom)
        {
            if (string.IsNullOrWhiteSpace(prenom)) return;
            utilisateurs = utilisateurs.Where(x => x.Prenom.ToLower().Contains(prenom.Trim().ToLower()));
        }

        private void SearchByEmail(ref IQueryable<UtilisateurDTO> utilisateurs, string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;
            utilisateurs = utilisateurs.Where(x => x.Email.ToLower().Contains(email.Trim().ToLower()));
        }

        private void SearchByEstActif(ref IQueryable<UtilisateurDTO> utilisateurs, bool? estActif)
        {
            if (!estActif.HasValue) return;
            utilisateurs = utilisateurs.Where(x => x.EstActif == estActif);
        }

        public IBaseClass Create(Utilisateur utilisateur)
        {
            UtilisateurDTO utilisateurDTO = ToDTO(utilisateur);
            _context.Utilisateur.Add(utilisateurDTO);
            return utilisateurDTO;
        }

        public IBaseClass Update(Utilisateur utilisateur)
        {
            UtilisateurDTO utilisateurDTO = _context.Utilisateur.Find(utilisateur.Id);
            ApplyChanges(utilisateurDTO, utilisateur);
            _context.Utilisateur.Update(utilisateurDTO);
            return utilisateurDTO;
        }

        public bool Delete(Utilisateur utilisateur)
        {
            UtilisateurDTO utilisateurDTO = _context.Utilisateur.Find(utilisateur.Id);

            if (utilisateurDTO.AccesUtilisateurs.Any())
                return false;

            _context.Utilisateur.Remove(utilisateurDTO);

            return true;
        }

        public UtilisateurDTO ToDTO(Utilisateur utilisateur)
        {
            UtilisateurDTO dto = new UtilisateurDTO(utilisateur.Id, utilisateur.Nom, utilisateur.Prenom, utilisateur.Email, utilisateur.EstActif);
            return dto;
        }

        public static Utilisateur FromDTO(UtilisateurDTO dto)
        {
            Utilisateur utilisateur = Utilisateur.Init(dto.Id, dto.Nom, dto.Prenom, dto.Email, dto.EstActif);
            return utilisateur;
        }

        public static IEnumerable<Utilisateur> FromDTO(IQueryable<UtilisateurDTO> dto)
        {
            foreach (var item in dto)
            {
                yield return FromDTO(item);
            }
        }

        public static void ApplyChanges(UtilisateurDTO dest, Utilisateur source)
        {
            if (dest is null || source is null) return;
            dest.Nom = source.Nom;
            dest.Prenom  = source.Prenom;
            dest.Email = source.Email;
            dest.EstActif = source.EstActif;
        }       
    }
}
