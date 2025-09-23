using Blog.Core.DTos;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // UserManger
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        public AuthController(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        // Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var NewUser = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email

                    };
                    IdentityResult Result = await _userManager.CreateAsync(NewUser,model.Password);
                    if(Result.Succeeded)
                    {
                        return StatusCode(201, new
                        {
                            StatusCode = 201,
                            Message = "User Added Successfully"
                        });
                    }
                    else
                    {
                        foreach(var Error in Result.Errors)
                            ModelState.AddModelError("Errors",Error.Description);
                    }
                }
                return BadRequest(ModelState);

            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User? User = await _userManager.FindByNameAsync(model.UserName);
                    if(User is not null)
                    {
                        if (await _userManager.CheckPasswordAsync(User, model.Password))
                        {
                            // payload  [data in token]
                            var Claims = new List<Claim>();
                            // custom claim
                            //Claims.Add(new Claim("TokenNumber", "1"));
                            // pre defined
                            // user data
                            Claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id)); // user id
                            Claims.Add(new Claim(ClaimTypes.Name, User.UserName));  // UserName
                            // Scurity Code  like id
                            Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                            // Roles
                            Claims.Add(new Claim(ClaimTypes.Role, "User"));

                            var Roles = await _userManager.GetRolesAsync(User);
                            foreach(var Role in Roles)
                            {
                                Claims.Add(new Claim(ClaimTypes.Role, Role.ToString()));
                            }
                            // Generate JWT Token
                            // signing credintioals

                            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
                            var signingCredentials = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);



                            var Token = new JwtSecurityToken(
                                    claims: Claims,
                                    audience: _config["JWT:Audience"],
                                    issuer: _config["JWT:Issuer"],
                                    expires: DateTime.Now.AddMinutes(10),
                                    signingCredentials: signingCredentials
                                );
                            var _token = new
                            {
                                Token =new JwtSecurityTokenHandler().WriteToken(Token),
                                Expiration = Token.ValidTo
                            };
                            return Ok(_token);
                        }
                        else
                        {

                            return Unauthorized();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Errors", "UserName Not Valid");
                    }
                }
                return BadRequest(ModelState);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
