using AutoMapper;
using INFORCE_.NET_TASK.Server.Account;
using INFORCE_.NET_TASK.Server.DbLogic;
using INFORCE_.NET_TASK.Server.DTOs;
using INFORCE_.NET_TASK.Server.Repositories.Interfaces;
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
        //private readonly UrlShortenerContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public AccountController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (model.Password != null &&
                model.Password == model.PasswordConfirm)
            {

                //bool isLoginUnique = !await _context.Users.AnyAsync(u => u.Login == model.Login);
                bool isLoginUnique = !await _userRepository.CheckIfExistsUserWithSuchLogin(model.Login);
                if (isLoginUnique)
                {
                    User user = new User
                    {
                        Id = Guid.NewGuid(),
                        Salt = Guid.NewGuid(),
                        Login= model.Login,
                        IsAdmin = false
                    };
                    user.PasswordHash = PasswordHasher.HashPassword(model.Password+user.Salt.ToString());
                    /*_context.Users.Add(user);
                    await _context.SaveChangesAsync();*/
                    await _userRepository.AddAsync(user);
                    await SignInAsync(user);
                    var userDto = _mapper.Map<UserDTO>(user);
                    return Ok(userDto);
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
            return BadRequest(
                    new
                    {
                        error = "The password field is not filled in or does not match confirm password field. Please ensure that both the password and the confirmation password are identical"
                    });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticationModel model)
        {
            if (model.Login == null)
            {
                return BadRequest(
                    new
                    {
                        error = "The login field is required to fill"
                    });
            }
            if (model.Password == null)
            {
                return BadRequest(
                    new
                    {
                        error = "The password field is required to fill"
                    });
            }

            /*var userOrNull = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == model.Login);*/
            var userOrNull = await _userRepository.GetByLoginAsync(model.Login);
            if (userOrNull is User user)
            {
                var isPasswordCorrect = PasswordHasher.IsCorrectPassword(user, model.Password);
                if (isPasswordCorrect)
                {
                    await SignInAsync(user);
                    var userDto = _mapper.Map<UserDTO>(user);
                    return Ok(userDto);
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

        [HttpGet("checkIfLoggedIn")]
        public async Task<IActionResult> CheckIfLoggedIn()
        {
            UserDTO userDto = null;
            bool isLoggedIn = false;
            if (User?.Identity?.Name != null)
            {
                //var user = await _context.Users.FirstAsync(u => u.Login == User.Identity.Name);
                var user = await _userRepository.GetByLoginAsync(User.Identity.Name);
                userDto = _mapper.Map<UserDTO>(user);
                isLoggedIn = true;
            }
            return Ok(
                new
                {
                    isLoggedIn = isLoggedIn,
                    user = userDto
                });
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
