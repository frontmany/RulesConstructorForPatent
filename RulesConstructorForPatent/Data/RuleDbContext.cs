using Microsoft.EntityFrameworkCore;
using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Data
{
    public class RuleDbContext : DbContext
    {
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileProperty> ProfileProperties { get; set; }
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

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Properties)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guidance>()
                .HasMany(g => g.Organizations)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProfilePropertyKind>()
                .HasMany(k => k.Options)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            SeedProfilePropertyKinds(modelBuilder);
        }

        // Начальные условия для пп. 5–6.4 ТЗ; записываются при создании базы.
        // Дальше условия меняют в самой базе — код при этом не трогают.
        private static void SeedProfilePropertyKinds(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfilePropertyKind>().HasData(
                new ProfilePropertyKind { Id = 1, Name = "Цель въезда" },
                new ProfilePropertyKind { Id = 2, Name = "Гражданство" },
                new ProfilePropertyKind { Id = 3, Name = "Особый статус" });

            modelBuilder.Entity<ProfilePropertyOption>().HasData(
                new { Id = 1, ProfilePropertyKindId = 1, Value = "Трудовая деятельность" },
                new { Id = 2, ProfilePropertyKindId = 1, Value = "Иная цель" },
                new { Id = 3, ProfilePropertyKindId = 2, Value = "Азербайджан" },
                new { Id = 4, ProfilePropertyKindId = 2, Value = "Молдова" },
                new { Id = 5, ProfilePropertyKindId = 2, Value = "Таджикистан" },
                new { Id = 6, ProfilePropertyKindId = 2, Value = "Узбекистан" },
                new { Id = 7, ProfilePropertyKindId = 2, Value = "Украина" },
                new { Id = 8, ProfilePropertyKindId = 2, Value = "Другое государство" },
                new { Id = 9, ProfilePropertyKindId = 3, Value = "Высококвалифицированный специалист" },
                new { Id = 10, ProfilePropertyKindId = 3, Value = "Член семьи высококвалифицированного специалиста" },
                new { Id = 11, ProfilePropertyKindId = 3, Value = "Участник госпрограммы переселения соотечественников" },
                new { Id = 12, ProfilePropertyKindId = 3, Value = "Член семьи участника госпрограммы" },
                new { Id = 13, ProfilePropertyKindId = 3, Value = "Нет особого статуса" });
        }
    }
}
