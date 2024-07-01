using Application.DTOs.InsertDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("/api/login")]
        public async Task<ActionResult<string>> Login(LoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByEmailAsync(userLoginDTO.EmailOrUserName);

            if (user == null)
            {
                user = await userManager.FindByNameAsync(userLoginDTO.EmailOrUserName);

                if (user == null)
                {
                    return BadRequest();
                }
            }

            var claims = await userManager.GetClaimsAsync(user);

            var cl = claims.FirstOrDefault(c => c.Type == "Role");

            IdentityResult identityRes = new IdentityResult();

            if (cl != null)
            {
                identityRes = await userManager.RemoveClaimAsync(user, cl);
            }

            var r = await userManager.GetRolesAsync(user);

            identityRes = await userManager.AddClaimAsync(user, new Claim("Role", r[0]));


            cl = claims.FirstOrDefault(c => c.Type == "Id");

            if (cl != null)
            {
                identityRes = await userManager.RemoveClaimAsync(user, cl);
            }

            var id = await userManager.GetUserIdAsync(user);

            identityRes = await userManager.AddClaimAsync(user, new Claim("Id", id));

            cl = claims.FirstOrDefault(c => c.Type == "UserName");

            if (cl != null)
            {
                identityRes = await userManager.RemoveClaimAsync(user, cl);
            }

            var userName = await userManager.GetUserNameAsync(user);

            identityRes = await userManager.AddClaimAsync(user, new Claim("UserName", userName ?? ""));

            cl = claims.FirstOrDefault(c => c.Type == "ExpireDate");

            if (cl != null)
            {
                identityRes = await userManager.RemoveClaimAsync(user, cl);
            }

            identityRes = await userManager.AddClaimAsync(user, new Claim("ExpireDate", DateTime.Now.AddDays(1).ToString("f")));

            cl = claims.FirstOrDefault(c => c.Type == "UserType");

            if (cl != null)
            {
                identityRes = await userManager.RemoveClaimAsync(user, cl);
            }

            identityRes = await userManager.AddClaimAsync(user, new Claim("UserType", user.UserType.ToString()));

            claims = await userManager.GetClaimsAsync(user);

            var sKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("SKey").Value??""));

            var signingCreds = new SigningCredentials(sKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signingCreds
                );

            var givenToken = new JwtSecurityTokenHandler().WriteToken(token);

            IdentityResult identityResult = new IdentityResult();

            cl = claims.FirstOrDefault(c => c.Type == "Token");

            if (cl != null)
            {
                identityResult = await userManager.RemoveClaimAsync(user, cl);
            }

            identityResult = await userManager.AddClaimAsync(user, new Claim("Token", givenToken));

            if (identityResult.Succeeded)
            {
                await signInManager.SignInAsync(user, false);

                return Ok(givenToken);
            }

            return Accepted();
        }
    }
}
