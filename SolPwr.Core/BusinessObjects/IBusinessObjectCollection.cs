using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
 

    public interface IBusinessObjectReadOnlyCollection<out T> : IEnumerable<T>, IAsyncEnumerable<T>, IQueryable<T>
        where T : IBusinessObject
    {
    }


    public interface IBusinessObjectCollection<T> : IBusinessObjectReadOnlyCollection<T>
        where T : IBusinessObject
    {
        void Add(T obj);

        void Remove(T obj);

        void RemoveRange(IEnumerable<T> obj);

        Task<T> FirstOrDefaultAsync();

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
