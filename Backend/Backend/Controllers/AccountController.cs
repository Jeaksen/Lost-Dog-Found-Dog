using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using Backend.Services;
using Backend.Services.AuthenticationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> AddAccount(AddAccountDto _account)
        {
            var result = await _accountService.AddAccount(_account);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate(LoginDto _account)
        {
            var result = await _accountService.Authenticate(_account);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = AccountRoles.Admin)] //temporary
        [HttpGet]
        [Route("users/{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var result = await _accountService.GetAccountById(id);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = AccountRoles.Admin)]
        [HttpGet]
        [Route("admin/users")]
        public async Task<IActionResult> GetAllUserAccounts()
        {
            var result = await _accountService.GetAllAccountsForRole(AccountRoles.User);
            return StatusCode(result.StatusCode, result);
        }

    }
}
