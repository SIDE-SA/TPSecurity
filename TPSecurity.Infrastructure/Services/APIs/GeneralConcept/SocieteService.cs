using System;
using TPSecurity.Application.Common.Interfaces.Services.GeneralConcept;
using TPSecurity.Infrastructure.Models.GeneralConcept;
using TPSecurity.Infrastructure.Options;

namespace TPSecurity.Infrastructure.Services.APIs.GeneralConcept
{
    public class SocieteService : ISocieteService
    {
        const string kVersion = "v2";

        private readonly GeneralConceptOptions _options;
        private readonly Options.Version _version;

        public SocieteService(GeneralConceptOptions options)
        {
            _options = options;
            _version = _options.Versions.Where(v => v.Name == kVersion).SingleOrDefault();
        }

        public async Task<bool> Exist(Guid id)
        {
            SocieteDTO societeDTO = await GetById(id);
            return societeDTO is not null;
        }

        private async Task<SocieteDTO> GetById(Guid id)
        {
            /* to do, ne pas l'instantier à chaque requête */
            string authenticationString = HttpClientHelper.GetBasicAuth(_options.User,
                                                                        _options.Password);

            string path = HttpClientHelper.GetFullPath(_version!.BaseUrl,
                                                       _version!.Name,
                                                       _version!.Concepts.Societe,
                                                       id.ToString());

            SocieteDTO? societeDTO = await Task.FromResult(HttpClientHelper.HttpGet<SocieteDTO>(authenticationString, path)).Result;

            return societeDTO;
        }
    }
}
