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
        Task<ServiceResponse<IList<Account>>> GetAllAccounts();
        Task<ServiceResponse<Account>> GetAccountsById(int id);
        Task<ServiceResponse<Account>> AddAccount(AddAccountDto _account); 
    }
}
