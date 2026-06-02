using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Store.Common
{
    public static class Pagination
    {
        public static IQueryable<T> ToPaged<T>(this IQueryable<T> source, int page, int pageSize,out int rowsCount)
        {
            rowsCount = source.Count();
            return source.Skip((page-1) * pageSize).Take(pageSize);
        }
    }
}