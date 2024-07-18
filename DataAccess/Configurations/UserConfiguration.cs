using JsonPlaceholderDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JsonPlaceholderDataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(u => u.Id);

            builder.OwnsOne(u => u.Address, address =>
            {
                address.OwnsOne(a => a.Geo);
            });

            builder.OwnsOne(u => u.Company);
        }
    }
}
