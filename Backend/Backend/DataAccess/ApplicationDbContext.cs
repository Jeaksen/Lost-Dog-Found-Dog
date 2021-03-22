using Backend.Models.Authentication;
using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using Backend.Models.DogBase.ShelterDog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<Account, IdentityRole<int>, int>
    {
        public DbSet<LostDog> LostDogs { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<ShelterDog> ShelterDogs { get; set; }
        public DbSet<LostDogComment> LostDogComments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<DogBehavior> DogBehaviors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Dog>()
                .Property("Discriminator")
                .HasMaxLength(50);
        }

    }
}
