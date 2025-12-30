using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(string id)
        {
            var guid = GuidHelper.GetGuidFromString(id);
            var user = _userService.GetById(guid);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Create(RegisterDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            var user = new User() { Name = userDto.Name, Surname = userDto.Surname, Email = userDto.Email, Password = userDto.Password };
            var createdUser = _userService.Create(user);

            return CreatedAtAction(nameof(Create), createdUser);
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Guid guid = GuidHelper.GetGuidFromString(id);
            var user = _userService.GetById(guid)!;
            _userService.Delete(user);
            return Ok("Successfully deleted user.");
        }
    }
}
