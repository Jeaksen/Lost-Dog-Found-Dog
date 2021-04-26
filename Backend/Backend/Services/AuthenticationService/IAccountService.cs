using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using Backend.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services.AuthenticationService
{
    public interface IAccountService
    {
        Task<ServiceResponse<IList<GetAccountDto>>> GetAllAccountsForRole(string role);
        Task<ServiceResponse<GetAccountDto>> GetAccountById(int id);
        Task<ServiceResponse<GetAccountDto>> AddAccount(AddAccountDto _account);
        Task<ServiceResponse<AuthenticationResult>> Authenticate(LoginDto loginDto);
        Task<ServiceResponse<GetAccountDto>> UpdateAccount(UpdateAccountDto accountDto, int userId);
    }
}
