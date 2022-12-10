using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Models.Discord;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public LoginController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet("GetToken")]
        public async Task<object> GetToken([FromQuery] string code)
        {
            var httpClient = httpClientFactory.CreateClient();
            // get access_token
            var accessRequest = new HttpRequestMessage(HttpMethod.Post, "https://discord.com/api/oauth2/token");
            var accessRequestBody = new Dictionary<string, string>()
            {
                { "client_id" , configuration.GetValue<string>("Discord:ClientId")},
                { "client_secret" , configuration.GetValue<string>("Discord:ClientSecret")},
                {"grant_type", "authorization_code" },
                {"code", code },
                {"redirect_uri", configuration.GetValue<string>("BaseUrl") +  "discord-redirect" }
            };
            accessRequest.Content = new FormUrlEncodedContent(accessRequestBody);
            var accessResponse = await httpClient.SendAsync(accessRequest, HttpContext.RequestAborted).ConfigureAwait(false);
            accessResponse.EnsureSuccessStatusCode();
            var accessResponseBody = JsonDocument.Parse(await accessResponse.Content.ReadAsStringAsync()).RootElement;
            var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var discordAccessToken = JsonSerializer.Deserialize<DiscordAccessToken>(accessResponseBody, serializerOptions);

            // get guild member info
            var guildRequest = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me/guilds/671787052572082176/member");
            guildRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            guildRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", discordAccessToken?.access_token);
            var guildResponse = await httpClient.SendAsync(guildRequest, HttpContext.RequestAborted);
            guildResponse.EnsureSuccessStatusCode();

            var guildBody = JsonDocument.Parse(await guildResponse.Content.ReadAsStringAsync()).RootElement;
            var guilds = JsonSerializer.Deserialize<DiscordGuildMember>(guildBody, serializerOptions);


            var permClaims = new List<Claim>();
            var regionRoles = guilds?.roles.Intersect(DiscordConstants.FangsRegionRoles);

            if (!regionRoles.Any())
            {
                return Unauthorized("Need to be in the Fangs Server to authenticate");
            }

            permClaims.Add(new Claim(DiscordConstants.Claim_userId, guilds.user.id));
            permClaims.Add(new Claim(DiscordConstants.Claim_regionId, string.Join(",", regionRoles)));
            permClaims.Add(new Claim(DiscordConstants.Claim_ismod, guilds?.roles.Contains(DiscordConstants.FangsRole_Moderator).ToString()));
            permClaims.Add(new Claim(DiscordConstants.Claim_isdev, guilds?.roles.Contains(DiscordConstants.FangsRole_Developer).ToString()));
            permClaims.Add(new Claim(DiscordConstants.Claim_userNick, guilds.user.username));

            // generate jwt
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:EncryptionKey")));
            var validIssuer = configuration.GetValue<string>("Jwt:Issuer");
            var validAudience = configuration.GetValue<string>("Jwt:Audience");

            var credentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);

            var expiration = TimeSpan.FromDays(6);
            var token = new JwtSecurityToken(
                validIssuer,
                validAudience,
                permClaims,
                expires: DateTime.Now.Add(expiration),
                signingCredentials: credentials);

            var jwt_Token = new JwtSecurityTokenHandler().WriteToken(token);

            Response.Cookies.Append("auth_token", jwt_Token,
                new CookieOptions() { Secure = true, HttpOnly = true, SameSite = SameSiteMode.Strict, MaxAge = expiration }
                );
            Response.Cookies.Append(DiscordConstants.Claim_userId, permClaims[0].Value,
               new CookieOptions() { Secure = true, SameSite = SameSiteMode.Strict, MaxAge = expiration }
               );
            Response.Cookies.Append(DiscordConstants.Claim_ismod, permClaims[2].Value,
               new CookieOptions() { Secure = true, SameSite = SameSiteMode.Strict, MaxAge = expiration }
               );
            Response.Cookies.Append(DiscordConstants.Claim_isdev, permClaims[3].Value,
               new CookieOptions() { Secure = true, SameSite = SameSiteMode.Strict, MaxAge = expiration }
               );
            Response.Cookies.Append(DiscordConstants.Claim_userNick, permClaims[4].Value,
              new CookieOptions() { Secure = true, SameSite = SameSiteMode.Strict, MaxAge = expiration }
              );
            return Ok();
        }

    }
}

