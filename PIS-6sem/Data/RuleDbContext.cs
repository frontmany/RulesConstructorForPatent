using Microsoft.EntityFrameworkCore;
using PIS_6sem.Entities;

namespace PIS_6sem.Data;

public class RuleDbContext : DbContext
{
    public DbSet<Rule> Rules { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ProfileProperty> ProfileProperties { get; set; }
    public DbSet<Guidance> Guidances { get; set; }
    public DbSet<TargetDocument> TargetDocuments { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=rules.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rule>()
            .HasMany(r => r.Profiles)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Rule>()
            .HasMany(r => r.TargetDocuments)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Rule>()
            .HasOne(r => r.Guidance)
            .WithOne(g => g.Rule)           
            .HasForeignKey<Guidance>(g => g.RuleId)
            .IsRequired()                
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Profile>()
            .HasMany(p => p.Properties)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Guidance>()
            .HasMany(g => g.Organizations)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}