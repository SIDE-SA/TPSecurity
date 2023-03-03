using TPSecurity.Application.Common.Interfaces.Persistence;

namespace TPSecurity.Infrastructure.Models;

public class BaseClass : IBaseClass
{
    public int Id { get; set; }
    public string UserCreation { get; set; } = null!;
    public DateTime DateCreation { get; set; }
    public string UserModification { get; set; } = null!;
    public DateTime DateModification { get; set; }

}
