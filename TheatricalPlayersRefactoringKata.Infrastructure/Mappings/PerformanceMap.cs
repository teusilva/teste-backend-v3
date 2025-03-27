using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TheatricalPlayersRefactoringKata.Domain.Entities;

namespace TheatricalPlayersRefactoringKata.Infrastructure.Mappings
{
    internal class PerformanceMap : IEntityTypeConfiguration<Performance>
    {
        public void Configure(EntityTypeBuilder<Performance> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Audience).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedBy);
            builder.Property(x => x.UpdatedAt);

            builder
            .HasOne(p => p.Play)
            .WithMany()
            .HasForeignKey(p => p.PlayId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}