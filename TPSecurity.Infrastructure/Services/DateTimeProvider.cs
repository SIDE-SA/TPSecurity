using TPSecurity.Application.Common.Interfaces.Services;

namespace TPSecurity.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => WithoutMillisecond(DateTime.Now);

    /// <summary>
    /// On ne stock pas les millisecondes
    /// Le soucis est que la db SqlServeur arrondit les millisecondes lors du stockage
    /// https://stackoverflow.com/questions/67549782/rounding-problems-with-datetime
    /// Ce qui pose problème lors de la vérification de la concurrence lors des updates
    /// -- TODO -> arriver à la même conversion que SQL, dangereux si on change de DB
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public DateTime WithoutMillisecond(DateTime d)
    {
        return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute,
                            d.Second, 0, d.Kind);
    }
}