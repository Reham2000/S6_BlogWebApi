using Blog.Core.DTos;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        // Get All Roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var Roles = await _roleManager.Roles.Select(role => new
                {
                    role.Id,
                    role.Name,
                }).ToListAsync();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Roles Retrived Successfully",
                    Data =  Roles
                });
            }catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error Ouccred while Apliying Api",
                    Error = ex.Message
                });
            }
        }
        // Assign User Roles
        [HttpPost]
        public async Task<IActionResult> AssignRoles(AssignRole assignRole)
        {
            try
            {
                // get User
                var User = await _userManager.FindByIdAsync(assignRole.UserId);
                if(User == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User Not Found"
                    });
                }
                else
                {
                    var Errors = new List<string>();
                    var Successes = new List<string>();

                    foreach(var Role in assignRole.Roles)
                    {
                        if(! await _roleManager.RoleExistsAsync(Role))
                        {
                            Errors.Add($"Role {Role} does not exist");
                            continue;
                        }
                        var Result = await _userManager.AddToRoleAsync(User, Role);
                        if (Result.Succeeded)
                        {
                            Successes.Add($"User {User.UserName} Added To Role {Role}");
                        }
                        else
                        {
                            Errors.AddRange(Result.Errors.Select(e => e.Description));
                        }
                    }

                    if (Errors.Any())
                        return BadRequest(new
                        {
                            StatusCode = 400,
                            Message = "An Error Ouccred while Apliying Api",
                            Errors = Errors
                        });

                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Roles Assigned successfully",
                        Data = Successes
                    });


                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error Ouccred while Apliying Api",
                    Error = ex.Message
                });
            }
        }
    }
}
