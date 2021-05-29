using AutoMapper;
using Backend.DataAccess;
using Backend.DataAccess.LostDogs;
using Backend.DataAccess.ShelterDogs;
using Backend.DataAccess.Shelters;
using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using Backend.Models.Dogs;
using Backend.Models.Dogs.LostDogs;
using Backend.Models.Shelters;
using Backend.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Backend.Tests
{
    public class DatabaseFixture : IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        private UserManager<Account> UserManager { get; }
        private RoleManager<IdentityRole<int>> RoleManager { get; }
        public IMapper Mapper { get; }
        public ILoggerFactory LoggerFactory { get; }

        public IAccountService AccountService => new AccountService(UserManager, RoleManager, configuration, Mapper, LoggerFactory.CreateLogger<AccountService>());
        public ILostDogRepository LostDogRepository => new LostDogDataRepository(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite("Filename=AuthTest.db").Options), LoggerFactory.CreateLogger<LostDogDataRepository>());
        public IShelterRepository ShelterRepository => new ShelterDataRepository(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite("Filename=AuthTest.db").Options), LoggerFactory.CreateLogger<ShelterDataRepository>());
        public IShelterDogRepository ShelterDogRepository => new ShelterDogDataRepository(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite("Filename=AuthTest.db").Options), LoggerFactory.CreateLogger<ShelterDogDataRepository>());

        public DatabaseFixture()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Filename=AuthTest.db"));

            var dbContext = serviceCollection.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            serviceCollection.AddIdentity<Account, IdentityRole<int>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            serviceCollection.AddLogging();
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });

            serviceProvider = serviceCollection.BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();

            UserManager = serviceProvider.GetService<UserManager<Account>>();
            RoleManager = serviceProvider.GetService<RoleManager<IdentityRole<int>>>();

            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });

            Mapper = mapperConfig.CreateMapper();

            Seed();
        }

        public void Seed()
        {
            var account =  new AddAccountDto()
            {
                Name = "bob",
                Email = "bob@gmail.com",
                PhoneNumber = "222333444",
                Password = "SafePass66",
                AccountRole = AccountRoles.Regular
            };
            if (!AccountService.AddAccount(account).Result.Successful)
                throw new ApplicationException("Could not seed the database with a user");

            var saveDogDog = new LostDog()
            {
                Breed = "dogdog",
                Age = 5,
                Size = "Large, very large",
                Color = "Orange but a bit yellow and green dots",
                SpecialMark = "tattoo of you on the neck",
                Name = "Cat",
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new Location() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            if (LostDogRepository.AddLostDog(saveDogDog).Result == null)
                throw new ApplicationException("Could not seed the database with a dog assigned to user");

            var shelter = new Shelter()
            {
                IsApproved = true,
                Name = $"VeryNiceShelter-1",
                PhoneNumber = $"2111111111",
                Email = $"sheltermock1@gmail.com",
                Address = new Address
                {
                    City = "Gdańsk",
                    PostCode = "12-345",
                    Street = "Bursztynowa",
                    BuildingNumber = "123"
                }
            };
            if (ShelterRepository.AddShelter(shelter).Result == null)
                throw new ApplicationException("Could not seed the database with a shelter");
        }


        public void Dispose()
        {

        }
    }
}
