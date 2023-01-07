using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopBridgeInventory.Managers;
using ShopBridgeInventory.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopBridgeInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly InventoryManager _InventoryManager;
        private readonly IDataProtector _protector;
        public LoginController(IConfiguration config, IDataProtectionProvider provider, InventoryManager param)
        {
            _config = config;
            _protector = provider.CreateProtector("DX&!%_YXZ");
            _InventoryManager = param ?? throw new ArgumentNullException(nameof(param));
        }

        [HttpGet]
        public IActionResult Login(string UserName, string Password, string keyVaue)
        {
            string EventName = "Userdata";
            try
            {

                if (keyVaue == "Encrypt")
                {
                    var EncryptUserName = _protector.Protect(UserName);
                    var EncryptPassword = _protector.Protect(Password);

                    return Ok(new { status = 200, userName = EncryptUserName, password = EncryptPassword });
                }
                else
                {
                    var DecrypttUserName = _protector.Unprotect(UserName);
                    var DecryptPassword = _protector.Unprotect(Password);

                    LoginDataModel Login = new LoginDataModel();
                    Login.UserName = DecrypttUserName;
                    Login.Password = DecryptPassword;
                    IActionResult response = Unauthorized();
                    var user = AuthenticateUser(Login);
                    if (user != null)
                    {
                        var tokenStr = GenerateJSONWebToken(user);
                        response = Ok(new { status = 200, token = tokenStr });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                _InventoryManager.LogDataInsert("Login Error", ex.Message.ToString(), EventName);
                return BadRequest(new { status = 400, Error = "Not a Valid User!" });
            }
        }
        private LoginDataModel AuthenticateUser(LoginDataModel Login)
        {
            LoginDataModel user = null;
            if (Login.UserName == "DnyanHelp" && Login.Password == "123_123")
            {
                user = new LoginDataModel
                {
                    UserName = "DnyanHelp",
                    Password = "123_123",
                    Email_ID = "dnyanchand@gmail.com"
                };
            }
            return user;
        }
        private string GenerateJSONWebToken(LoginDataModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credetials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email,userInfo.Email_ID),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credetials
                );

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }


    }
}
