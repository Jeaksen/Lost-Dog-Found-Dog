using Backend.Models.Authentication;
using Backend.Models.Dogs;
using Backend.Models.Dogs.LostDogs;
using Backend.Models.Dogs.ShelterDogs;
using Backend.Models.Shelters;
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
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Dog>()
                .Property("Discriminator")
                .HasMaxLength(50);
            builder.Entity<Shelter>().Property(s => s.AddressId).IsRequired(true);
            builder.Entity<Shelter>().HasIndex(s => s.Name).IsUnique(true);
            builder.Entity<Shelter>().HasIndex(s => s.Email).IsUnique(true);
            builder.Entity<Shelter>()
            .HasOne(s => s.Address)
            .WithOne()
            .HasPrincipalKey<Shelter>(s => s.AddressId);
            builder.Entity<ShelterDog>()
            .HasOne(d => d.Shelter)
            .WithOne()
            .HasForeignKey<ShelterDog>(d => d.ShelterId);
        }

    }
}
