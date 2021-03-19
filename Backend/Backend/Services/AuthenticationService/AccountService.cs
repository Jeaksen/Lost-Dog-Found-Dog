using AutoMapper;
using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
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
using System.Threading.Tasks;

namespace Backend.Services.AuthenticationService
{
    public class AccountService : IAccountService

    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<Account> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration, IMapper mapper, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<Account>> AddAccount(AddAccountDto _account)
        {
            var userExists = await _userManager.FindByNameAsync(_account.UserName);
            if (userExists != null)
                return new ServiceResponse<Account>() { Data = null, Successful = false, Message = "User already exists!" };
            Account account = _mapper.Map<Account>(_account);
            account.SecurityStamp = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(account, _account.Password);
            if (!result.Succeeded)
                return new ServiceResponse<Account>() { Data = null, Successful = false, Message = "User creation failed! Please check user details and try again." };

            if (!await _roleManager.RoleExistsAsync(AccountRoles.User))
                await _roleManager.CreateAsync(new IdentityRole<int>(AccountRoles.User));

            if (await _roleManager.RoleExistsAsync(AccountRoles.User))
                await _userManager.AddToRoleAsync(account, AccountRoles.User);

            return new ServiceResponse<Account>() { Data = account, Successful = true, Message = "User created successfully!" };
        }

        public async Task<ServiceResponse<Account>> GetAccountById(int id)
        {
            var serviceResponse = new ServiceResponse<Account>();
            serviceResponse.Data = await _userManager.FindByIdAsync(id.ToString());
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.Message = $"Failed to fetch User with id: {id}";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<IList<Account>>> GetAllAccountsForRole(string role)
        {
            var serviceResponse = new ServiceResponse<IList<Account>>();
            serviceResponse.Data = await _userManager.GetUsersInRoleAsync(role);
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.Message = "Failed to fetch Users";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<AuthenticationResult>> Authenticate(LoginDto loginDto)
        {
            var response = new ServiceResponse<AuthenticationResult>();
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    var userRole = (await _userManager.GetRolesAsync(user)).First();
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    response.Data = new AuthenticationResult()
                    {
                        UserType = userRole,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                    };
                }
                else
                {
                    response.Message = "Invalid user password";
                    response.Successful = false;
                }
            }
            else
            {
                response.Message = "User not defined in the system";
                response.Successful = false;
            }
            return response;
        }
    }
}
