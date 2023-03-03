using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace TPSecurity.Infrastructure.Services;

public static class SortHelper
{
    public static void ApplySort<T>(ref IQueryable<T> entities, string orderBy, string orderOrientation) where T : class
    {
        if (string.IsNullOrWhiteSpace(orderBy)) return;

        var orderParams = orderBy.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param)) continue;

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null) continue;

            var sortingOrder = orderOrientation.ToLower() == "desc" ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');            
        entities = string.IsNullOrWhiteSpace(orderQuery) ? entities : entities.OrderBy(orderQuery);  
    }       
}
