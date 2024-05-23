﻿using AutoMapper;
using INFORCE_.NET_TASK.Server.Account;
using INFORCE_.NET_TASK.Server.DbLogic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace INFORCE_.NET_TASK.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UrlShortenerContext _context;
        private readonly IMapper _mapper;

        public AccountController(UrlShortenerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (model.Password != null &&
                model.Password == model.PasswordConfirm)
            {
                return  BadRequest(
                    new 
                    { 
                        error = "The password field is not filled in or does not match confirm password field. Please ensure that both the password and the confirmation password are identical"
                    });
            }
            bool isLoginUnique = !await _context.Users.AnyAsync(u=>u.Login == model.Login);
            if (isLoginUnique)
            {
                var user = _mapper.Map<User>(model);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await SignInAsync(user);
                return Ok();
            }
            else
            {
                return Conflict(
                    new
                    {
                        error = "Chosen login is already in use"
                    });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticationModel model)
        {
            if(model.Login != null)
            {
                return BadRequest(
                    new
                    {
                        error = "The login field is required to fill"
                    });
            }
            if(model.Password != null)
            {
                return BadRequest(
                    new
                    {
                        error = "The password field is required to fill"
                    });
            }

            var userOrNull = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == model.Login);
            if(userOrNull is User user)
            {
                var isPasswordCorrect = PasswordHasher.IsCorrectPassword(user, model.Password);
                if (isPasswordCorrect)
                {
                    await SignInAsync(user);
                    return Ok();
                }
            }
            return Unauthorized(
                new
                {
                    error = "Incorrect login or password. Please try again"
                });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
        private async Task SignInAsync(User user)
        {
            string role;
            if (user.IsAdmin)
            {
                role = "Admin";
            }
            else
            {
                role = "User";
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, role),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
        }
    }
}