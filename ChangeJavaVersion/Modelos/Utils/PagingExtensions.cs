using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Utils {
    public static class PagingExtensions {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize) {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int page, int pageSize) {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }



    }
}
