using HW6.Helpers;
using HW6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HW6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = AccountStorage.GetValueOrDefault(id);
            if (user is null)
            {
                return NotFound($"Can't get user by [{id}] id");
            }

            if (!IsRequestedUserAutorizedOrAdmin(user))
            {
                return Forbid();
            }

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult RegisterUser([FromBody] CreateUserModel model)
        {
            var user = AccountStorage.GetByUserName(model.UserName);
            if (user != null)
            {
                return Conflict($"User with '{model.UserName}' username already exist. (Id = {user.Id})");
            }

            AccountStorage.Register(model.FirstName, model.LastName, model.UserName);

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] UpdateUserModel value)
        {
            var existingUser = AccountStorage.GetValueOrDefault(id);
            if (existingUser == null)
            {
                return NotFound($"Can't get user by [{id}] id");
            }

            if (!IsRequestedUserAutorizedOrAdmin(existingUser))
            {
                return Forbid();
            }

            if (value.FirstName != null)
            {
                existingUser.FirstName = value.FirstName;
            }

            if (value.LastName != null)
            {
                existingUser.LastName = value.LastName;
            }

            return Ok();
        }

        private bool IsRequestedUserAutorizedOrAdmin(User user)
        {
            if (User.Identity?.Name is null)
            {
                return false;
            }

            return user.UserName.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
