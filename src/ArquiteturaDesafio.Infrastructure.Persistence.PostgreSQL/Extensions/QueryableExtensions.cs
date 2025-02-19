using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Dictionary<string, string> filters)
        {
            filters = filters ?? new Dictionary<string, string>();
            foreach (var filter in filters)
            {
                if (!string.IsNullOrEmpty(filter.Value))
                {
                    if (filter.Key.StartsWith("_min"))
                    {
                        var field = filter.Key.Substring(4);
                        query = query.Where($"{field} >= @0", Convert.ChangeType(filter.Value, typeof(decimal)));
                    }
                    else if (filter.Key.StartsWith("_max"))
                    {
                        var field = filter.Key.Substring(4);
                        query = query.Where($"{field} <= @0", Convert.ChangeType(filter.Value, typeof(decimal)));
                    }
                    else if (filter.Value.Contains("*"))
                    {
                        var filterValue = filter.Value.Replace("*", "");
                        query = query.Where($"{filter.Key}.Contains(@0)", filterValue);
                    }
                    else
                    {
                        query = query.Where($"{filter.Key} == @0", filter.Value);
                    }
                }
            }
            return query;
        }
    }

}
