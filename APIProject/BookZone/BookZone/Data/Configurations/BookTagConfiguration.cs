using BookZone.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Data.Configurations
{
    public class BookTagConfiguration : IEntityTypeConfiguration<BookTag>
    {
        public void Configure(EntityTypeBuilder<BookTag> builder)
        {
            builder.HasOne(x => x.Book).WithMany(x => x.BookTags).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Tag).WithMany(x => x.BookTags).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
