using JsonPlaceholderDataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPlaceholderDataAccess.Configurations
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Albums>
    {
        public void Configure(EntityTypeBuilder<Albums> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
