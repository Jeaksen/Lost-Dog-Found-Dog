using AutoMapper;
using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using Backend.Models.Response;
using Backend.Services.Authentication;
using Backend.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            this.accountService = accountService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddAccount([FromForm][Required] string username,
                                                    [FromForm][Required] string password,
                                                    [FromForm][Required] string phone_number,
                                                    [FromForm][Required] string email)
        {
            var _account = new AddAccountDto()
            {
                Name = username,
                Password = password,
                PhoneNumber = phone_number,
                Email = email
            };
            var serviceResponse = await accountService.AddAccount(_account);
            var controllerResponse = mapper.Map<ServiceResponse, ControllerResponse>(serviceResponse);
            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate([FromForm][Required] string username,
                                                      [FromForm][Required] string password)
        {
            var _account = new LoginDto() { UserName = username, Password = password };
            var serviceResponse = await accountService.Authenticate(_account);
            var controllerResponse = mapper.Map<ServiceResponse<AuthenticationResult>, ControllerResponse<AuthenticationResult>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            return Ok(new ControllerResponse() { Message = "User logged out" });
        }

        [Authorize(Roles = AccountRoles.Admin + "," + AccountRoles.Regular)]
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetAccountById(int userId)
        {
            var serviceResponse = await accountService.GetAccountById(userId);
            var controllerResponse = mapper.Map<ServiceResponse<GetAccountDto>, ControllerResponse<GetAccountDto>>(serviceResponse);
            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [Authorize(Roles = AccountRoles.Admin + "," + AccountRoles.Regular)]
        [HttpPut]
        [Consumes("multipart/form-data")]
        [Route("user/{userId}")]
        public async Task<IActionResult> UpdateAccountById([ModelBinder(BinderType = typeof(JsonModelBinder))][Required][FromForm(Name = "userdata")] UpdateAccountDto updateAccountDto, 
                                                          [FromRoute] int userId)
        {
            if (User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value == userId.ToString())
            {
                var serviceResponse = await accountService.UpdateAccount(updateAccountDto, userId);
                var controllerResponse = mapper.Map<ServiceResponse<GetAccountDto>, ControllerResponse<GetAccountDto>>(serviceResponse);

                return StatusCode(serviceResponse.StatusCode, controllerResponse);
            }
            else
                return Unauthorized(new ControllerResponse() { Message = "You cannot update details of other users!", Successful = false});
        }

        [Authorize(Roles = AccountRoles.Admin)]
        [HttpGet]
        [Route("admin/users")]
        public async Task<IActionResult> GetAllUserAccounts()
        {
            var serviceResponse = await accountService.GetAllAccountsForRole(AccountRoles.Regular);
            var controllerResponse = mapper.Map<ServiceResponse<IList<GetAccountDto>>, ControllerResponse<IList<GetAccountDto>>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

    }
}
