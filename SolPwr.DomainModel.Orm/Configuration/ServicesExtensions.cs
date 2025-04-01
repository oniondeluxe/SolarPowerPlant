using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.BusinessLogic;
using OnionDlx.SolPwr.BusinessObjects;
using OnionDlx.SolPwr.Dto;
using OnionDlx.SolPwr.Persistence;
using OnionDlx.SolPwr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Configuration
{
    public static class ServicesOrmExtensions
    {
        /// <summary>
        /// Will add the needed boilerplate to IoC, without the need to introduce a dependency to EF in the main app
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IServiceCollection AddPersistence(this IServiceCollection coll, string connString)
        {
            // As originating from a static method, this is what we need to do
            Extensions.RequestConvert += Extensions_RequestConvert;

            coll.AddDbContext<UtilitiesContext>(options => options.UseSqlServer(connString));
            coll.AddScoped<IUtilitiesRepositoryFactory>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<IUtilitiesRepositoryFactory>>();
                return new UtilitiesRepositoryFactory(connString, logger);
            });

            return coll;
        }


        private static void Extensions_RequestConvert(object sender, ConverterEventArgs e)
        {
            if (e != null)
            {
                // Trick
                e.Converter = new EntityQueryConverter();
            }
        }


        class EntityQueryConverter : IConverter
        {
            public EntityQueryConverter()
            {
            }


            public Task<List<T>> GetToListAsync<T>(IQueryable<T> coll) where T : IBusinessObject
            {
                return EntityFrameworkQueryableExtensions.ToListAsync(coll);
            }


            public Task<T> GetFirstOrDefaultAsync<T>(IQueryable<T> coll) where T : IBusinessObject
            {
                // EF is not reachable from the pure domain layer
                return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(coll);
            }
        }
    }



}
