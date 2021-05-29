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
        public DbSet<PictureDog> DogPictures { get; set; }
        public DbSet<PictureComment> CommentPictures { get; set; }
        public DbSet<DogBehavior> DogBehaviors { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().HasOne(a => a.Shelter).WithOne().HasForeignKey<Account>(a => a.ShelterId).IsRequired(false);

            builder.Entity<Dog>().Property("Discriminator").HasMaxLength(50);
            builder.Entity<Dog>().HasOne(d => d.Picture).WithOne(p => p.Dog).HasForeignKey<PictureDog>(p => p.DogId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LostDog>().HasOne(d => d.Location).WithOne(l => l.LostDog).HasForeignKey<Location>(l => l.LostDogId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<LostDog>().HasMany(d => d.Comments).WithOne(c => c.LostDog);

            builder.Entity<LostDogComment>().HasOne(d => d.Picture).WithOne(p => p.LostDogComment).HasForeignKey<PictureComment>(p => p.LostDogCommentId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<LostDogComment>().HasOne(d => d.Author).WithOne().HasForeignKey<LostDogComment>(c => c.AuthorId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Shelter>().HasIndex(s => s.Name).IsUnique();
            builder.Entity<Shelter>().HasIndex(s => s.Email).IsUnique();
            builder.Entity<Shelter>().HasOne(s => s.Address).WithOne(a => a.Shelter).HasForeignKey<Address>(a => a.ShelterId).IsRequired();

            builder.Entity<ShelterDog>().HasOne(d => d.Shelter).WithMany().OnDelete(DeleteBehavior.Cascade);

        }

    }
}
