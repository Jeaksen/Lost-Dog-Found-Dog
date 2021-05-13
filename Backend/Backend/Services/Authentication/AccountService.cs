using AutoMapper;
using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using Backend.Models.Response;
using Backend.Models.Shelters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Backend.Services.Authentication
{
    public class AccountService : IAccountService

    {
        private readonly UserManager<Account> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ILogger<AccountService> logger;
        private readonly string tokenPrefix = "Bearer ";

        public AccountService(UserManager<Account> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration, IMapper mapper, ILogger<AccountService> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.logger = logger;

            ConfigureRoles().Wait();
        }

        private async Task ConfigureRoles()
        {
            if (!await roleManager.RoleExistsAsync(AccountRoles.Regular))
                await roleManager.CreateAsync(new IdentityRole<int>(AccountRoles.Regular));
            if (!await roleManager.RoleExistsAsync(AccountRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole<int>(AccountRoles.Admin));
            if (!await roleManager.RoleExistsAsync(AccountRoles.Shelter))
                await roleManager.CreateAsync(new IdentityRole<int>(AccountRoles.Shelter));
        }

        public async Task<ServiceResponse<AuthenticationResult>> Authenticate(LoginDto loginDto)
        {
            var serviceResponse = new ServiceResponse<AuthenticationResult>();
            var user = await userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
                user = await userManager.FindByEmailAsync(loginDto.UserName);

            if (user != null)
            {
                if (await userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    var userRole = (await userManager.GetRolesAsync(user)).First();

                    if (userRole == AccountRoles.Shelter)
                        user.Id = user.ShelterId;

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, userRole)
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: configuration["JWT:ValidIssuer"],
                        audience: configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    serviceResponse.Data = new AuthenticationResult()
                    {
                        UserType = userRole,
                        Id = user.Id,
                        Token = tokenPrefix + new JwtSecurityTokenHandler().WriteToken(token),
                    };
                }
                else
                {
                    serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                    serviceResponse.Message = "Invalid user password";
                    serviceResponse.Successful = false;
                }
            }
            else
            {
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                serviceResponse.Message = "User not defined in the system";
                serviceResponse.Successful = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAccountDto>> GetAccountById(int id)
        {
            var serviceResponse = new ServiceResponse<GetAccountDto>();
            var savedUser = await userManager.FindByIdAsync(id.ToString());
            if (savedUser == null)
            {
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                serviceResponse.Successful = false;
                serviceResponse.Message = $"Failed to fetch User with id: {id}";
            }
            else
            {
                serviceResponse.Data = mapper.Map<GetAccountDto>(savedUser);
                serviceResponse.Message = "User found";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<IList<GetAccountDto>>> GetAllAccountsForRole(string role)
        {
            var serviceResponse = new ServiceResponse<IList<GetAccountDto>>();
            var savedUsers = await userManager.GetUsersInRoleAsync(role);
            if (savedUsers == null)
            {
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                serviceResponse.Successful = false;
                serviceResponse.Message = "Failed to fetch Users";
            }
            else
            {
                serviceResponse.Data = mapper.Map<IList<GetAccountDto>>(savedUsers);
                serviceResponse.Message = $"{savedUsers.Count} User found";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAccountDto>> AddShelterAccount(Shelter shelter)
        {
            var shelterAccount = new AddShelterAccountDto()
            {
                Email = shelter.Email,
                Name = Regex.Replace(shelter.Name, "[^a-zA-Z_.0-9]+", "", RegexOptions.Compiled),
                Password = "SafePass22",
                PhoneNumber = shelter.PhoneNumber,
                AccountRole = AccountRoles.Shelter,
                ShelterId = shelter.Id
            };
            return await AddAccount(shelterAccount);
        }

        public async Task<ServiceResponse<GetAccountDto>> AddAccount(AddAccountDto _account)
        {
            var serviceResponse = new ServiceResponse<GetAccountDto>();

            Account account = mapper.Map<Account>(_account);
            account.SecurityStamp = Guid.NewGuid().ToString();
            var result = await userManager.CreateAsync(account, _account.Password);

            if (result.Succeeded)
            {
                result = await userManager.AddToRoleAsync(account, _account.AccountRole);
                var savedUser = await userManager.FindByNameAsync(_account.Name);
                if (result.Succeeded)
                {
                    serviceResponse.Message = $"User sucessfully created!";
                    serviceResponse.Data = mapper.Map<GetAccountDto>(savedUser);
                }
                else
                {
                    serviceResponse.Message = GetErrorsString(result.Errors, "Failed to add user to the role! ");
                    serviceResponse.Successful = false;
                    serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                    await userManager.DeleteAsync(savedUser);
                }
            }
            else
            {
                serviceResponse.Message = GetErrorsString(result.Errors, "User creation failed! ");
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAccountDto>> UpdateAccount(UpdateAccountDto accountDto, int userId)
        {
            var serviceResponse = new ServiceResponse<GetAccountDto>();
            var savedAccount = await userManager.FindByIdAsync(userId.ToString());

            if (savedAccount is null)
            {
                serviceResponse.Message = $"Failed to update user with id {userId}: Account not found";
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                savedAccount.UserName = accountDto.Name;
                savedAccount.Email = accountDto.Email;
                savedAccount.PhoneNumber = accountDto.PhoneNumber;
                var result = await userManager.UpdateAsync(savedAccount);
                serviceResponse = await GetAccountById(userId);
                if (result.Succeeded && serviceResponse.Successful)
                {
                    serviceResponse.Message = $"User with id {userId} was updated";
                }
                else
                {
                    serviceResponse.Message = GetErrorsString(result.Errors, $"Failed to update user with id { userId }! ");
                    serviceResponse.Successful = false;
                    serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            return serviceResponse;
        }

        private string GetErrorsString(IEnumerable<IdentityError> errors, string message)
        {
            var errorBuilder = new StringBuilder(message);
            foreach (var error in errors)
            {
                errorBuilder.Append(error.Code);
                errorBuilder.Append(": ");
                errorBuilder.Append(error.Description);
                errorBuilder.Append(" ");
            }
            return errorBuilder.ToString();
        }

        private async Task<ServiceResponse<Account>> GetAccount(int? id = null, string username = null, string email = null)
        {
            var serviceResponse = new ServiceResponse<Account>();
            Account savedAccount = null;
            if (id.HasValue)
            {
                savedAccount = await userManager.FindByIdAsync(id.ToString());
                if (savedAccount == null)
                {
                    serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                    serviceResponse.Successful = false;
                    serviceResponse.Message = $"Failed to fetch User with id: {id.Value}";
                }
                else
                {
                    serviceResponse.Data = savedAccount;
                    serviceResponse.Message = "User found";
                }
            }
            else if (!string.IsNullOrEmpty(email))
            {
                savedAccount = await userManager.FindByEmailAsync(email);
                if (savedAccount == null)
                {
                    serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                    serviceResponse.Successful = false;
                    serviceResponse.Message = $"Failed to fetch User with email: {email}";
                }
                else
                {
                    serviceResponse.Data = savedAccount;
                    serviceResponse.Message = "User found";
                }
            }
            else if (!string.IsNullOrEmpty(username))
            {
                savedAccount = await userManager.FindByNameAsync(username);
                if (savedAccount == null)
                {
                    serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                    serviceResponse.Successful = false;
                    serviceResponse.Message = $"Failed to fetch User with username: {username}";
                }
                else
                {
                    serviceResponse.Data = savedAccount;
                    serviceResponse.Message = "User found";
                }
            }
            else
            {
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                serviceResponse.Successful = false;
                serviceResponse.Message = $"No data was given to identify an account";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteAccount(int? id = null, string username = null, string email = null)
        {
            var accountResponse = await GetAccount(id.HasValue? id.Value : null, username, email);
            var response = mapper.Map<ServiceResponse<Account>, ServiceResponse>(accountResponse);
            if (accountResponse.Successful)
            {
                var result = await userManager.DeleteAsync(accountResponse.Data);
                if (result.Succeeded)
                    response.Message = $"Account with id {accountResponse.Data.Id} deleted";
                else
                {
                    response.Successful = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = GetErrorsString(result.Errors, $"Failed to delete account with id {accountResponse.Data.Id}");
                }
            }

            return response;
        }
    }
}
