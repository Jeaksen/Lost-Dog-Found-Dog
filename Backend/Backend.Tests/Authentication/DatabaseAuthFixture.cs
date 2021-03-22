using AutoMapper;
using Backend.DataAccess;
using Backend.Models.Authentication;
using Backend.Services.AuthenticationService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

namespace Backend.Tests
{
    public class DatabaseAuthFixture : IDisposable
    {

        public AuthenticationDbContext IdentityDbContext { get; }
        public UserManager<Account> UserManager { get; }
        public RoleManager<IdentityRole<int>> RoleManager { get; }
        public IMapper Mapper { get; }
        public ILoggerFactory LoggerFactory { get; }
        public IAccountService AccountService { get; }

        private AuthenticationDbContext dbContext;

        public DatabaseAuthFixture()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<AuthenticationDbContext>(options => options.UseSqlite("Filename=AuthTest.db"));

            dbContext = serviceCollection.BuildServiceProvider().GetRequiredService<AuthenticationDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();


            IdentityDbContext = serviceCollection.BuildServiceProvider().GetService<AuthenticationDbContext>();
            IdentityDbContext.Database.OpenConnection();
            IdentityDbContext.Database.EnsureCreated();


            serviceCollection.AddIdentity<Account, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<AuthenticationDbContext>()
                .AddDefaultTokenProviders();

            serviceCollection.AddLogging();
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

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

            var serviceProvide = serviceCollection.BuildServiceProvider();

            LoggerFactory = serviceProvide.GetService<ILoggerFactory>();

            UserManager = serviceProvide.GetService<UserManager<Account>>();
            RoleManager = serviceProvide.GetService<RoleManager<IdentityRole<int>>>();

            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });

            Mapper = mapperConfig.CreateMapper();

            AccountService = new AccountService(UserManager, RoleManager, configuration, Mapper, LoggerFactory.CreateLogger<AccountService>());
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
