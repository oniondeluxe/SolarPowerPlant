using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
    public interface IBusinessObjectRepository : IDisposable
    {
        Guid? SaveChanges();

        Task<Guid?> SaveChangesAsync();
    }


    public interface IRepositoryFactory<out T> where T : IBusinessObjectRepository
    {
        T NewQuery();

        T NewCommand();

        string ProviderInfo { get; }
    }
}
