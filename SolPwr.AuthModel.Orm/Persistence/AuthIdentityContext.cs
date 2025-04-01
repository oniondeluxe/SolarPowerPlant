using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Persistence
{
    internal class AuthIdentityContext : IdentityDbContext
    {
        #region Setup

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("spu");
        }

        #endregion

        public AuthIdentityContext(DbContextOptions<AuthIdentityContext> options) : base(options)
        {
        }
    }
}
