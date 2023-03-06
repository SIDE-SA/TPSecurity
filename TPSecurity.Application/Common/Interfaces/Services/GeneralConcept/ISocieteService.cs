namespace TPSecurity.Application.Common.Interfaces.Services.GeneralConcept
{
    public interface ISocieteService
    {
        public Task<bool> Exist(Guid id);
    }
}
