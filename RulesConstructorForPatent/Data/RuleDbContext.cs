using Microsoft.EntityFrameworkCore;
using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Data
{
    public class RuleDbContext : DbContext
    {
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Guidance> Guidances { get; set; }
        public DbSet<TargetDocument> TargetDocuments { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<ProfilePropertyKind> ProfilePropertyKinds { get; set; }
        public DbSet<ProfilePropertyOption> ProfilePropertyOptions { get; set; }

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

            // Зависимости правил — связь «многие ко многим» правила с самим собой.
            // Таблица RequiredAccomplishedRules хранит пары (RuleId, RequiredRuleId).
            // Удаление зависимого правила удаляет и его связи (Cascade), а правило, от которого
            // зависят другие, удалить нельзя (Restrict): иначе их условия молча исчезли бы.
            modelBuilder.Entity<Rule>()
                .HasMany(r => r.RequiredAccomplishedRules)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "RequiredAccomplishedRules",
                    requiredRule => requiredRule
                        .HasOne<Rule>()
                        .WithMany()
                        .HasForeignKey("RequiredRuleId")
                        .OnDelete(DeleteBehavior.Restrict),
                    dependentRule => dependentRule
                        .HasOne<Rule>()
                        .WithMany()
                        .HasForeignKey("RuleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    link => link.HasKey("RuleId", "RequiredRuleId"));

            // Профиль ссылается на выбранные варианты справочника. Вариант, который используют
            // правила, удалить нельзя (Restrict): иначе условия профилей молча исчезли бы.
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Options)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ProfileOptions",
                    option => option
                        .HasOne<ProfilePropertyOption>()
                        .WithMany()
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Restrict),
                    profile => profile
                        .HasOne<Profile>()
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade),
                    link => link.HasKey("ProfileId", "OptionId"));

            modelBuilder.Entity<Guidance>()
                .HasMany(g => g.Organizations)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProfilePropertyKind>()
                .HasMany(k => k.Options)
                .WithOne(o => o.Kind)
                .OnDelete(DeleteBehavior.Cascade);

            // Одинаковые виды склеились бы при выводе, а одинаковые варианты задвоились бы в опросе.
            modelBuilder.Entity<ProfilePropertyKind>()
                .HasIndex(k => k.Name)
                .IsUnique();

            modelBuilder.Entity<ProfilePropertyOption>()
                .HasIndex(o => new { o.KindId, o.Value })
                .IsUnique();
        }
    }
}
