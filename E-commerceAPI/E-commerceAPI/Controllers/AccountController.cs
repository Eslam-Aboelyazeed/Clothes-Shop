using E_commerceAPI.DTOs.DisplayDTOs;
using E_commerceAPI.DTOs.InsertDTOs;
using E_commerceAPI.DTOs.UpdateDTOs;
using E_commerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("/api/register")]
        public async Task<IActionResult> Register(ApplicationUserInsertDTO userInsertDTO)
        {
            if (userInsertDTO == null)
            {
                return BadRequest();
            }

            var userEmail = await userManager.FindByEmailAsync(userInsertDTO.Email);
            if (userEmail != null)
            {
                return Conflict();
            }

            var user = new ApplicationUser()
            {
                UserName = $"{userInsertDTO.FirstName}_{userInsertDTO.LastName}",
                Email = userInsertDTO.Email,
                PasswordHash = userInsertDTO.Password,
                PhoneNumber = userInsertDTO.PhoneNumber,
                Address = userInsertDTO.Address
            };

            var adminRole = await roleManager.FindByNameAsync("Admin");
            var userRole = await roleManager.FindByNameAsync("User");
            if (adminRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (userRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            var identityResult = await userManager.CreateAsync(user, userInsertDTO.Password);


            if (identityResult.Succeeded)
            {

                IdentityResult identityResult1;

                if (user.Email == configuration.GetSection("adminEmail").Value)
                {
                    identityResult1 = await userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
                }
                else
                {
                    identityResult1 = await userManager.AddToRoleAsync(user, UserRoles.User.ToString());
                }

                if (identityResult1.Succeeded)
                {
                    var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(action: "ConfirmEmail", controller: "Account", values: new { userId = user.Id, token = emailToken }, protocol: Request.Scheme, host: Request.Host.Host + ":" + Request.Host.Port);

                    var smtpClient = new SmtpClient();

                    var mailMessage = new MailMessage(configuration.GetSection("email").Value ?? "", user.Email, "Email Confirmation", $"Please confirm your account by <a href='http://localhost:4200/confirm?userId={user.Id}&token={HttpUtility.UrlEncode(emailToken)}'>clicking here</a>.");

                    mailMessage.IsBodyHtml = true;

                    smtpClient.Host = "smtp.gmail.com";

                    smtpClient.Port = 587;

                    smtpClient.Credentials = new NetworkCredential(configuration.GetSection("email").Value ?? "", configuration.GetSection("pass").Value ?? "");

                    smtpClient.EnableSsl = true;

                    await smtpClient.SendMailAsync(mailMessage);

                    return NoContent();
                }
                return Accepted();
            }

            return BadRequest(identityResult);
        }

        [HttpPost("/api/login")]
        public async Task<ActionResult<ClaimsDTO>> Login(ApplicationUserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByEmailAsync(userLoginDTO.Email);

            if (user == null)
            {
                return BadRequest();
            }

            var passFlag = await userManager.CheckPasswordAsync(user, userLoginDTO.Password);

            if (!passFlag)
            {
                return BadRequest("Wrong Password");
            }

            var claims = await userManager.GetClaimsAsync(user);

            var cl = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            IdentityResult identityRes = new IdentityResult();

            if (cl != null)
            {
                identityRes = await userManager.RemoveClaimAsync(user, cl);
            }

            var r = await userManager.GetRolesAsync(user);

            identityRes = await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, r[0]));


            cl = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (cl != null)
            {
                identityRes = await userManager.RemoveClaimAsync(user, cl);
            }

            var id = await userManager.GetUserIdAsync(user);

            identityRes = await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, id));

            cl = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (cl != null)
            {
                identityRes = await userManager.RemoveClaimAsync(user, cl);
            }

            var userName = await userManager.GetUserNameAsync(user);

            identityRes = await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, userName??""));

            claims = await userManager.GetClaimsAsync(user);

            var sKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("sKey").Value ?? ""));

            var signingCreds = new SigningCredentials(sKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signingCreds
                );

            var givenToken = new JwtSecurityTokenHandler().WriteToken(token);

            await signInManager.SignInAsync(user, userLoginDTO.IsPersistent);

            ClaimsDTO claimsDTO = new ClaimsDTO()
            {
                Token = givenToken
            };

            return Ok(claimsDTO);
        }

        [HttpGet("/api/logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromQuery] string UserId)
        {
            var user = await userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return BadRequest();
            }

            await signInManager.SignOutAsync();

            return NoContent();
        }

        [HttpGet("/api/confirmemail")]
        public async Task<IActionResult> ConfirmEmail( [FromQuery] string userId, [FromQuery] string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Please Try Again Later");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }

            var identityResult = await userManager.ConfirmEmailAsync(user, token);

            return Ok(identityResult.Succeeded);
        }

        [HttpPut("{id:alpha}")]
        [SwaggerOperation(
            Summary = "This Endpoint updates the specified user element",
            Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given user object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the user element was updated successfully", Type = typeof(void))]
        [Authorize]
        public async Task<IActionResult> PutUser(string id, ApplicationUserUpdateDTO userUpdateDTO)
        {
            var user = await userManager.FindByIdAsync(userUpdateDTO.Id);

            if (user == null)
            {
                return NotFound();
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            user.PhoneNumber = userUpdateDTO.PhoneNumber;
            user.Address = userUpdateDTO.Address;
            user.Email = userUpdateDTO.Email;
            user.UserName = $"{userUpdateDTO.FirstName} {userUpdateDTO.LastName}";

            var identityResult = await userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
            {
                return NoContent();
            }

            return Accepted();
        }

        [HttpPut("/api/User/{id:alpha}")]
        [SwaggerOperation(
            Summary = "This Endpoint updates the specified user element password",
            Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given user object or the current password was wrong", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the user element password was updated successfully", Type = typeof(void))]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id, ApplicationUserUpdatePasswordDTO userUpdatePasswordDTO)
        {
            var user = await userManager.FindByIdAsync(userUpdatePasswordDTO.Id);

            if (user == null)
            {
                return NotFound();
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            var currentRes = await userManager.CheckPasswordAsync(user, userUpdatePasswordDTO.CurrentPassword);

            if (!currentRes)
            {
                return BadRequest("Wrong Current Password");
            }

            var identityResult = await userManager.ChangePasswordAsync(user, userUpdatePasswordDTO.CurrentPassword, userUpdatePasswordDTO.NewPassword);

            if (identityResult.Succeeded)
            {
                return NoContent();
            }

            return Accepted();
        }

        [HttpGet("/api/User")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUserInformation(string userName)
        {
            if (userName == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new UserDTO()
            {
                UserName = userName,
                Address = user.Address,
                Email = user.Email??"",
                PhoneNumber = user.PhoneNumber?? ""
            };

            return Ok(userDTO);
        }

        [HttpGet("/api/ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string userEmail)
        {
            if (userEmail == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var newPassword = GeneratePassword();

                await userManager.ChangePasswordAsync(user, user.PasswordHash??"", newPassword);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("eslamaboelyazeed22@gmail.com");
                mail.To.Add(user.Email ?? "");
                mail.Subject = "Password Reset";
                mail.Body = $"Dear User,\n\nYour password has been reset to:{newPassword}";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("eslamaboelyazeed22@gmail.com", "trdh xkap nbmu nowt");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return NoContent();
            }
            catch (Exception) { }

            return Accepted();
        }

        private string GeneratePassword()
        {
            const string _validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";

            Random random = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < 12; i++)
            {
                int randomIndex = random.Next(0, _validChars.Length);
                password.Append(_validChars[randomIndex]);
            }

            return password.ToString();
        }
    }
}
