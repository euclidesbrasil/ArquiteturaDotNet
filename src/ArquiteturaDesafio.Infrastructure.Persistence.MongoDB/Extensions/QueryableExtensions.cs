using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Extensions
{
    public static class MongoExtensions
    {
        public static FilterDefinition<T> ApplyFilters<T>(this FilterDefinitionBuilder<T> builder, Dictionary<string, string> filters)
        {
            filters = filters ?? new Dictionary<string, string>();
            var filterDefinition = builder.Empty;

            foreach (var filter in filters)
            {
                if (!string.IsNullOrEmpty(filter.Value))
                {
                    if (filter.Key.StartsWith("_min"))
                    {
                        var field = filter.Key.Substring(4);
                        filterDefinition &= builder.Gte(field, Convert.ChangeType(filter.Value, typeof(decimal)));
                    }
                    else if (filter.Key.StartsWith("_max"))
                    {
                        var field = filter.Key.Substring(4);
                        filterDefinition &= builder.Lte(field, Convert.ChangeType(filter.Value, typeof(decimal)));
                    }
                    else
                    {
                        filterDefinition &= builder.Eq(filter.Key, filter.Value);
                    }
                }
            }

            return filterDefinition;
        }
    }

}
