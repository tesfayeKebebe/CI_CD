using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;
using Domain.Identity;
using Infrastructure.Identity.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.API.Helpers;
using Web.API.ViewModel.Security;
namespace Web.API.Controllers;

[Route("api/Account")]
public class AccountController : ControllerBase
{
    private readonly IAccountManager _accountManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IConfiguration _config;
    public AccountController(IAccountManager accountManager, UserManager<ApplicationUser> userManager, IConfiguration config,  ICurrentUserService currentUserService)
    {
        _accountManager = accountManager;
        _userManager = userManager;
        _config = config;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<AuthenticationViewModel?> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return new AuthenticationViewModel();
        }
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        _ = int.TryParse(_config["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        var token = GetToken(authClaims);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
        await _userManager.UpdateAsync(user);
        return new AuthenticationViewModel
        {
            Username = model.Username,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = DateTime.Now.AddMinutes(60),
            RefreshToken = refreshToken,
            ExpiresIn = user.RefreshTokenExpiryTime,
            Type = "JWT",
            roles=userRoles,
            UserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id))
        };
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
        ApplicationUser user = new()
        {
            Email =  model.Email??"user@gmail.com",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
            PhoneNumber = model.PhoneNumber,
            IdNumber = model.IdNumber,
            FullName = model.FullName,
            EmailConfirmed = true,
            IsEnabled = true, IsExisting = true
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        var roles = await _userManager.AddToRoleAsync(user, "user");
        if (!result.Succeeded || !roles.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }
    [HttpPost]
    [Route("RegisterByRole")]
    [Authorize(Roles ="administrator, supervisor" )]
    public async Task<IActionResult> RegisterByAdmin([FromBody] RegisterUser model)
    {
        
        await EnsureRoleAsync(model.RoleName, model.RoleDescription, new string[] { });
        
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
        ApplicationUser user = new()
        {
            Email =  model.Email??"healthofficer@gmail.com",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
            PhoneNumber = model.PhoneNumber,
            IdNumber = model.IdNumber,
            FullName = model.FullName,
            EmailConfirmed = true,
            IsEnabled = true, IsExisting = true
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        var roles = await _userManager.AddToRoleAsync(user, model.RoleName);
        if (!result.Succeeded || !roles.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
    
        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }
    
    [HttpPut("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword( [FromBody] ChangePasswordModel model)
    {
        var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
        var user = await _userManager.FindByNameAsync(model.UserName);
        var   result = await _userManager.ChangePasswordAsync(user, model.OldPassword,
            model.NewPassword);
        return !result.Succeeded ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password changed failed! Please check user details and try again." }) 
            : Ok(new Response { Status = "Success", Message = "Password changed successfully!" });
    }
    [HttpPost]
    [Route("refresh-token")]
    public async Task<AuthenticationViewModel?> Refresh([FromBody]  RefreshTokenQuery query)
    {
        if (query.Username != null)
        {
            var user = await _userManager.FindByNameAsync(query.Username);
            if (user != null )
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                _ = int.TryParse(_config["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                _currentUserService.UserId = user.Id;
                var token = GetToken(authClaims);
                if (user?.RefreshTokenExpiryTime?.Day >= DateTime.Now.Day)
                {
                    return new AuthenticationViewModel
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        ExpiresAt = DateTime.Now.AddMinutes(60),
                        RefreshToken =query.RefreshToken,
                        ExpiresIn=user.RefreshTokenExpiryTime,
                        Type= "JWT",
                        roles=userRoles,
                        Username = query.Username,
                        UserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id))
                    };   
                }

            }
        }

        return null;
    }
    [HttpPost]
    [Route("Logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {

        var user = await _userManager.FindByNameAsync(User.Identity?.Name);
        if (user != null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime =null; ;
         var   result= await _userManager.UpdateAsync(user);
            await HttpContext.SignOutAsync();
            
            return !result.Succeeded ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Logout failed!" })
       : Ok(new Response { Status = "Success", Message = "Logout successfully!" });
        }

        return Unauthorized();
    }

    [HttpGet]
    [Authorize(Roles ="administrator, supervisor" )]
    [Route("GetUsers")]
    public async Task<IEnumerable<ApplicationUser>> GetUsers()
    {
        var users = await _userManager.Users.Where(x=>x.IsExisting).ToListAsync();
        return users;
    }
    
    [HttpPut]
    [Authorize(Roles ="administrator, supervisor" )]
    [Route("UpdateUser")]
    public async Task<string> UpdateUser([FromBody] UserDto model)
    {
        var oldUser = await _userManager.FindByIdAsync(model.Id);
        if (oldUser == null)
        {
            return "Failed to update";
        }
        oldUser.FullName = model.FullName;
        oldUser.PhoneNumber = model.PhoneNumber;
        oldUser.IsEnabled = model.IsEnabled;
     var result=   await _userManager.UpdateAsync(oldUser);
     return result.Succeeded ? "Updated Successfully" : "Failed to update";

    }
    [HttpDelete("DeleteUser/{id}")]
    [Authorize(Roles = "administrator")]
    public async Task<string> Delete(string id)
    {
        var data = await _userManager.FindByIdAsync(id);
        if (data == null)
        {
            return "Fail to delete";
        }
        data.IsExisting = false;
        var result=   await _userManager.UpdateAsync(data);
        return result.Succeeded ? "Deleted Successfully" : "Fail to delete";
    }
    private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
    {
        if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
        {
            ApplicationRole applicationRole = new ApplicationRole(roleName, description);
            var result = await this._accountManager.CreateRoleAsync(applicationRole, claims);

            if (!result.Succeeded)
                throw new Exception($"Creating \"{description}\" role failed. Errors: {string.Join("Environment.NewLine", result.Errors)}");
        }
    }
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret.Secret));

        var token = new JwtSecurityToken(
            issuer: _config["JWT:ValidIssuer"],
            audience: _config["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    


    private void AddError(IEnumerable<string> errors, string key = "")
    {
        foreach (var error in errors)
        {
            AddError(error, key);
        }
    }

    private void AddError(string error, string key = "")
    {
        ModelState.AddModelError(key, error);
    }
}

