using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RPG_MV_Trans_API.Models;
using RPG_MV_Trans_API.Models.JWT;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RPG_MV_Trans_API.Controllers
{
    /// <summary>
    /// Контроллер аутентификации.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        /// <summary>
        /// Конструктор аутентификации.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Запрос на получение токена
        /// </summary>
        /// <param name="authRequest">Запрос</param>
        /// <param name="signingEncodingKey">Ключ signing</param>
        /// <param name="encryptingEncodingKey">Ключ encrypting</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string>> PostAsync([FromBody] AuthenticationRequest authRequest,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey,
            [FromServices] IJwtEncryptingEncodingKey encryptingEncodingKey)
        {
            if ((await Task.Run(() => _signInManager.PasswordSignInAsync(authRequest.Name, authRequest.Password, false, false))).Succeeded)
            {
                User user = await Task.Run(() => _userManager.FindByNameAsync(authRequest.Name));
                string role;
                List<Claim> games = new List<Claim>();
                if (await Task.Run(() => _userManager.IsInRoleAsync(user, "admin"))) { role = "admin"; }
                else if (await Task.Run(() => _userManager.IsInRoleAsync(user, "user"))) { role = "user"; }
                else { role = "uncknown"; }
                if (role != "uncknown")
                {
                    foreach (Claim u in await Task.Run(() => _userManager.GetClaimsAsync(user)))
                    {
                        if (u.Type == "game") games.Add(u);
                    }
                }
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, authRequest.Name),
                    new Claim(ClaimTypes.Role, role)
                };
                claims.AddRange(games);

                var tokenHandler = new JwtSecurityTokenHandler();

                JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(
                    issuer: "TranslatorApi",
                    audience: "TranslatiorClient",
                    subject: new ClaimsIdentity(claims),
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(5),
                    issuedAt: DateTime.Now,
                    signingCredentials: new SigningCredentials(
                        signingEncodingKey.GetKey(),
                        signingEncodingKey.SigningAlgorithm),
                    encryptingCredentials: new EncryptingCredentials(
                        encryptingEncodingKey.GetKey(),
                        encryptingEncodingKey.SigningAlgorithm,
                        encryptingEncodingKey.EncryptingAlgorithm));

                var jwtToken = tokenHandler.WriteToken(token);
                return jwtToken;
            }
            else
            {
                return "Unauthorized";
            }
        }
    }
}
