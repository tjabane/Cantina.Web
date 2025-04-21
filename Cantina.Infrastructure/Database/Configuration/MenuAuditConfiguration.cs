using Cantina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.Database.Configuration
{
    internal class MenuAuditConfiguration: IEntityTypeConfiguration<MenuAudit>
    {
        public void Configure(EntityTypeBuilder<MenuAudit> builder)
        {
            builder.ToTable("MenuAudit");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.ActionId).IsRequired();
            builder.Property(x => x.Timestamp).IsRequired().ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.MenuItemId).IsRequired();

            builder.HasOne(ma => ma.Action).WithMany()
                .HasForeignKey(ma => ma.ActionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
