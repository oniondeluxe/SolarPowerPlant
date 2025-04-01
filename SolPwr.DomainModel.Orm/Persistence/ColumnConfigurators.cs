using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnionDlx.SolPwr.BusinessObjects;
using OnionDlx.SolPwr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Persistence
{
    public class PowerPlantConfiguration : IEntityTypeConfiguration<PowerPlant>
    {
        public void Configure(EntityTypeBuilder<PowerPlant> builder)
        {
            builder.OwnsOne(p => p.Location, a =>
            {
                // TODO: Check why the migration doesn't set these cols to NOT NULL
                a.Property(p => p.Latitude).HasColumnName("Latitude").IsRequired();
                a.Property(p => p.Longitude).HasColumnName("Longitude").IsRequired();
            });

            builder.Property(p => p.UtcInstallDate).IsRequired();
            builder.Property(p => p.PowerCapacity).IsRequired();
        }

        public PowerPlantConfiguration()
        {            
        }
    }
}
