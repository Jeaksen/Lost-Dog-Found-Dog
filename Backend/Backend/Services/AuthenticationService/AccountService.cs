using AutoMapper;
using AutoMapper.Configuration;
using Backend.DataAccess;
using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.AuthenticationService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

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
            
            return new ServiceResponse<Account>() { Data = account, Successful = true, Message = "User created successfully!" };
        }

        public async Task<ServiceResponse<Account>> GetAccountsById(int id)
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

        public async Task<ServiceResponse<IList<Account>>> GetAllAccounts()
        {
            var serviceResponse = new ServiceResponse<IList<Account>>();
            serviceResponse.Data = await _userManager.GetUsersInRoleAsync(AccountRoles.User);
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.Message = "Failed to fetch Users";
            }
            return serviceResponse;
        }

        public AccountService(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
        }
    }
}
