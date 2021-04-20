using Backend.DataAccess;
using Backend.DataAccess.Dogs;
using Backend.Models.Authentication;
using Backend.Services;
using Backend.Services.AuthenticationService;
using Backend.Services.LostDogService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IActionResult MakeMessage (ActionContext context)
        {
            var x = new ValidationProblemDetails(context.ModelState);
            StringBuilder errorMessageBuilder = new StringBuilder();
            errorMessageBuilder.Append("Model Validation error! ");
            foreach (var error in x.Errors)
            {
                errorMessageBuilder.Append(error.Key);
                errorMessageBuilder.Append(": ");
                foreach (var errorMessage in error.Value)
                {
                    errorMessageBuilder.Append(errorMessage);    
                    errorMessageBuilder.Append("  ");    
                }
            }

            var result = new ServiceResponse<bool>() { 
                Successful = false, 
                Message = errorMessageBuilder.ToString(),
                StatusCode = StatusCodes.Status400BadRequest
            };
            return new BadRequestObjectResult(result);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = context => MakeMessage(context));
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILostDogRepository, LostDogDataRepository>();
            services.AddScoped<ILostDogService, LostDogService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddIdentity<Account, IdentityRole<int>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddAuthentication(options =>
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
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        StringBuilder responseBuilder = new StringBuilder("Unauthorised! ");

                        if (!context.Request.Headers.ContainsKey("Authorization"))
                            responseBuilder.Append("Authorization header is missing in the request");
                        else
                        {
                            context.Request.Headers.TryGetValue("Authorization", out var value);
                            var token = value.ToString();

                            if (!token.StartsWith("Bearer "))
                                responseBuilder.Append("Bearer prefix missing in the token");
                            else
                                responseBuilder.Append("The header and token prefix are proper, invalid token");
                        }

                        context.Response.StatusCode = 401;

                        await context.Response.WriteAsJsonAsync(new ServiceResponse<bool>()
                        {
                            Successful = false,
                            Message = responseBuilder.ToString(),
                            StatusCode = StatusCodes.Status401Unauthorized
                        });
                    }
                };
                   
                });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));
            }

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                await context.Response.WriteAsJsonAsync(new ServiceResponse<bool> { Message = exception.Message, Successful = false, StatusCode = StatusCodes.Status400BadRequest });
            }));

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
