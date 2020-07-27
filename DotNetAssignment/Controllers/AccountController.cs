using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DotNetAssignment.Models;
using System.Linq;
using System.Data.Entity;

namespace DotNetAssignment.Controllers
{
    [Authorize]
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public AccountController()
        {
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // POST Account/RegisterUser
        [AllowAnonymous]
        [Route("RegisterUser")]
        public async Task<IHttpActionResult> RegisterUser(RegisterUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                Organization = model.Organization
            };
            var userExist = await UserManager.FindByEmailAsync(user.Email);
            if (userExist == null)
            {
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                UserManager.AddToRole(user.Id, "admin");

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            else
            {
                return BadRequest("Email Already Exist!");
            }

            return Ok();
        }

        // POST Account/RegisterAdmin
        [AllowAnonymous]
        [Route("RegisterAdmin")]
        public async Task<IHttpActionResult> RegisterAdmin(RegisterBaseUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email
            };

            var userExist = await UserManager.FindByEmailAsync(user.Email);
            if (userExist == null)
            {
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                UserManager.AddToRole(user.Id, "admin");

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            else
            {
                return BadRequest("Email Already Exist!");
            }

            return Ok();
        }

        // GET Account/GetAll
        [Authorize(Roles = "admin")]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAllUser()
        {
            var users = await UserManager.Users.Select(x => new { FirstName = x.FirstName, LastName = x.LastName }).ToListAsync();

            return Ok(users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}
