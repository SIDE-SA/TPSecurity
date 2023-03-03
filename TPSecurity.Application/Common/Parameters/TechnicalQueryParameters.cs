
namespace TPSecurity.Application.Common.Parameters;

public abstract class TechnicalQueryParameters : QueryParameters
{
    /// <summary>
    /// The name of the property on which we will sort
    /// </summary>
    public string? UserCreation { get; set; } = null!;

    public string? UserModification { get; set; } = null!;

    public DateTime? DateCreation { get; set; } = null;

    public DateTime? DateModification { get; set; } = null;

    public bool? Actif { get; set; }
    
}
