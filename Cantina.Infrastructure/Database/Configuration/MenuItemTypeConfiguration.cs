using Cantina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Cantina.Infrastructure.Database.Configuration
{
    internal class MenuItemTypeConfiguration : IEntityTypeConfiguration<MenuItemType>
    {
        public void Configure(EntityTypeBuilder<MenuItemType> builder)
        {
            builder.ToTable("MenuItemTypes");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);

            builder.HasData(
                 Enum.GetValues<Domain.Contants.MenuItemType>()
                    .Cast<Domain.Contants.MenuItemType>()
                    .Select(menuItemType => new MenuItemType
                    {
                        Id = (int)menuItemType,
                        Name = menuItemType.ToString()
                    })
                 );
        }
    }
}
