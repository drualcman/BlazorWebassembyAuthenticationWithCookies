using BlazorWebassembyAuthenticationWithCookies.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BlazorWebassembyAuthenticationWithCookies.Client.Services
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        readonly HttpClient Client;

        public ServerAuthenticationStateProvider(HttpClient client)
        {
            Client = client;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState result = null;

            HttpResponseMessage response = await Client.GetAsync("account/get-acutal-user");
            if (response.IsSuccessStatusCode)
            {
                UserLogin user = await response.Content.ReadFromJsonAsync<UserLogin>();
                if (user != null &&  !string.IsNullOrEmpty(user.UserName))
                {
                    ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>()
                    {
                      new Claim(ClaimTypes.NameIdentifier,user.UserName),
                      new Claim(ClaimTypes.Name,user.UserName)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    result = new AuthenticationState(new ClaimsPrincipal(identity));
                }
                else result = new AuthenticationState(new ClaimsPrincipal());
            }
            else
            {
                result = new AuthenticationState(new ClaimsPrincipal());
            }

            return result;
        }
    }
}
