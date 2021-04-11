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
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> AddAccount(AddAccountDto _account)
        {
            var result = await accountService.AddAccount(_account);
            result.Data = null;
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate(LoginDto _account)
        {
            var result = await accountService.Authenticate(_account);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("users/{userId}")]
        public async Task<IActionResult> GetAccountById(int userId)
        {
            var result = await accountService.GetAccountById(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        [Route("users/{userId}")]
        public async Task<IActionResult> UpdateAccountById(UpdateAccountDto updateAccountDto, int userId)
        {
            var result = await accountService.UpdateAccount(updateAccountDto, userId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = AccountRoles.Admin)]
        [HttpGet]
        [Route("admin/users")]
        public async Task<IActionResult> GetAllUserAccounts()
        {
            var result = await accountService.GetAllAccountsForRole(AccountRoles.User);
            return StatusCode(result.StatusCode, result);
        }

    }
}
