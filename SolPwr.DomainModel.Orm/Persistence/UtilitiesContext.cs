using Microsoft.EntityFrameworkCore;
using OnionDlx.SolPwr.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Persistence
{
    internal class UtilitiesContext : DbContext
    {
        readonly string _connString;

        public DbSet<PowerPlant> PowerPlants { get; set; }

        public DbSet<PowerGenerationRecord> GenerationHistory { get; set; }

        #region Setup

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("spa");

            // PowerPlant geo data columns
            modelBuilder.ApplyConfiguration(new PowerPlantConfiguration());
        }

        #endregion

        public UtilitiesContext(DbContextOptions<UtilitiesContext> options) : base(options)
        {
        }


        public UtilitiesContext(string connString)
        {
            _connString = connString;
        }
    }
}
