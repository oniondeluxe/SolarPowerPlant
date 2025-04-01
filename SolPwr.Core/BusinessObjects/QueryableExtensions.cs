using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
    public interface IConverter
    {
        Task<T> GetFirstOrDefaultAsync<T>(IQueryable<T> coll) where T : IBusinessObject;

        Task<List<T>> GetToListAsync<T>(IQueryable<T> coll) where T : IBusinessObject;
    }


    public class ConverterEventArgs : EventArgs
    {
        public IConverter Converter { get; set; }

        internal ConverterEventArgs()
        {
        }
    }


    public static class Extensions
    {
        public static event EventHandler<ConverterEventArgs> RequestConvert;

        /// <summary>
        /// On this level (where we don't have a dependency to EF), there is no built-in extension method for async 
        /// behavior, so we need to mimic that ourselves
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> coll) where T : IBusinessObject
        {
            // Static method, so we need to apply a trick to find the layer where there is EF knowledge
            // The one event subscription is handled when the dependency injection is configured
            if (RequestConvert != null)
            {
                var retVal = new ConverterEventArgs();
                RequestConvert(null, retVal);
                if (retVal.Converter != null)
                {
                    return retVal.Converter.GetFirstOrDefaultAsync(coll);
                }
            }

            return Task.FromResult<T>(default);
        }


        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> coll) where T : IBusinessObject
        {
            if (RequestConvert != null)
            {
                var retVal = new ConverterEventArgs();
                RequestConvert(null, retVal);
                if (retVal.Converter != null)
                {
                    return retVal.Converter.GetToListAsync(coll);
                }
            }

            return Task.FromResult<List<T>>(default);
        }
    }
}
