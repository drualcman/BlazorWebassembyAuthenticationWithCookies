using BlazorWebassembyAuthenticationWithCookies.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorWebassembyAuthenticationWithCookies.Server.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin login)
        {
            // proceso de buscar al usuario
            if (login.UserName == login.Password)
            {
                Claim claim = new Claim(ClaimTypes.NameIdentifier, login.UserName);
                ClaimsIdentity identity = new ClaimsIdentity(new [] { claim }, 
                    CookieAuthenticationDefaults.AuthenticationScheme);                
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                try
                {
                    await HttpContext.SignInAsync(principal);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else return BadRequest();
        }

        
        [HttpGet("get-acutal-user")]
        public UserLogin ActualUser()
        {
            UserLogin result = null;
            if (HttpContext != null
                && HttpContext.User != null
                && HttpContext.User.Identity.IsAuthenticated)
            {
                result = new UserLogin() { UserName = HttpContext.User.Claims
                    .Where(c=> c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value };
            }
            else
            {
                result = new UserLogin();
            }
            return result;
        }
    }
}
