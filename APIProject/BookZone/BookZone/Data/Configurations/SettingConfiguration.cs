using BookZone.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Data.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(x => x.Image).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Location).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Contact).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Facebook).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Instagram).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Pinterest).HasMaxLength(100).IsRequired();

        }
    }
}
