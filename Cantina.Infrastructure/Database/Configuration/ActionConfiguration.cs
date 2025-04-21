using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.Database.Configuration
{
    internal class ActionConfiguration: IEntityTypeConfiguration<Domain.Entities.Action>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Action> builder)
        {
            builder.ToTable("Actions");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.HasData
                (
                    Enum.GetValues<Domain.Contants.Actions>()
                            .Cast<Domain.Contants.Actions>()
                            .Select(action => new Domain.Entities.Action
                            {
                                Id = (int)action, 
                                Name = action.ToString()
                            })
                );
        }
    }
}
