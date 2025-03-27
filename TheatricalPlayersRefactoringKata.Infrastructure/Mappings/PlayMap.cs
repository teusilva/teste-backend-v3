using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheatricalPlayersRefactoringKata.Domain.Entities;

namespace TheatricalPlayersRefactoringKata.Infrastructure.Mappings
{
    internal class PlayMap : IEntityTypeConfiguration<Play>
    {
        public void Configure(EntityTypeBuilder<Play> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Lines).IsRequired();
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Type).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedBy);
            builder.Property(x => x.UpdatedAt);
        }
    }
}