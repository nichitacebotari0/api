using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public override DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Hero> Hero { get; set; }
        public DbSet<HeroClass> HeroClass { get; set; }
        public DbSet<AugmentEvent> AugmentEvent { get; set; }
        public DbSet<AugmentCategory> AugmentCategory { get; set; }
        public DbSet<AbilityType> AbilityType { get; set; }
        public DbSet<BoonEvent> BoonEvent { get; set; }
        public DbSet<ArtifactEvent> ArtifactEvent { get; set; }
        public DbSet<ArtifactType> ArtifactType { get; set; }
        public DbSet<Build> Build { get; set; }
        public DbSet<BuildVote> BuildVote { get; set; }
        public DbSet<Change> ChangeLog { get; set; }
        public DbSet<Patch> Patch { get; set; }
        public DbSet<AugmentSlot> AugmentSlot { get; set; }
        public DbSet<AugmentArrangement> AugmentArrangement { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().Where(e => !e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);

            // innitial patch
            builder.Entity<Patch>()
                .HasData(new Patch
                {
                    Id = 1,
                    Version = "initial",
                    GameDate = new DateTime(2022, 12, 28),
                    Title = "Initial Patch",
                    WebsiteTimeUtc = new DateTime(2022, 12, 01),
                });
        }
    }
}
