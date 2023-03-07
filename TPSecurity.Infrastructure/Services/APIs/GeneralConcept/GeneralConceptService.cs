using Microsoft.Extensions.Options;
using TPSecurity.Application.Common.Interfaces.Services.GeneralConcept;
using TPSecurity.Infrastructure.Options;

namespace TPSecurity.Infrastructure.Services.APIs.GeneralConcept
{
    public class GeneralConceptService : IGeneralConceptService
    {        
        public ISocieteService Societe { get; private set; }

        public GeneralConceptService(IOptions<GeneralConceptOptions> generalConceptConfig)
        {            
            Societe = new SocieteService(generalConceptConfig.Value);
        }        
    }
}
