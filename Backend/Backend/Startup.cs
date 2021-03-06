using Backend.DataAccess;
using Backend.Models.Authentication;
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
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using Backend.Services.Security;
using Backend.Models.Response;
using Backend.DataAccess.LostDogs;
using Backend.Services.Authentication;
using Backend.Services.LostDogs;
using Backend.DataAccess.Shelters;
using Backend.Services.Shelters;
using Backend.DataAccess.ShelterDogs;

namespace Backend
{
    class JsonDateConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => DateTime.ParseExact(reader.GetString(),"yyyy-MM-dd", CultureInfo.InvariantCulture);


        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }

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
            var errorMessageBuilder = new StringBuilder();
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

            var result = new ControllerResponse() { 
                Successful = false, 
                Message = errorMessageBuilder.ToString()
            };
            return new BadRequestObjectResult(result);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = context => MakeMessage(context))
                .AddJsonOptions(o => 
                {
                    o.JsonSerializerOptions.Converters.Add(new JsonDateConverter());
                });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILostDogRepository, LostDogDataRepository>();
            services.AddScoped<ILostDogService, LostDogService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IShelterRepository, ShelterDataRepository>();
            services.AddScoped<IShelterService, ShelterService>();
            services.AddScoped<IShelterDogRepository, ShelterDogDataRepository>();
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
                options.UseSqlServer(Configuration.GetConnectionString("Docker-Compose"));
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

                        var responseBuilder = new StringBuilder("Unauthorised! ");

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

                        await context.Response.WriteAsJsonAsync(new ControllerResponse()
                        {
                            Successful = false,
                            Message = responseBuilder.ToString()
                        });
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;

                        await context.Response.WriteAsJsonAsync(new ControllerResponse()
                        {
                            Successful = false,
                            Message = "Your role does not allow to access this resource!"
                        }); ;
                    },
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
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                  var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                  context.Database.Migrate();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

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
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ControllerResponse { Message = exception.Message, Successful = false });
            }));

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
