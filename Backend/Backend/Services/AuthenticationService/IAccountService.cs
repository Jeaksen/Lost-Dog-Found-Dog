using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.AuthenticationService
{
    public interface IAccountService
    {
        Task<ServiceResponse<IList<Account>>> GetAllAccountsForRole(string role);
        Task<ServiceResponse<Account>> GetAccountById(int id);
        Task<ServiceResponse<Account>> AddAccount(AddAccountDto _account);
        Task<ServiceResponse<AuthenticationResult>> Authenticate(LoginDto loginDto);
    }
}
