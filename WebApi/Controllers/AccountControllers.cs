using Core.DTOs.User;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountControllers : Controller
    {
        private readonly IAccountService _accountService;

        public AccountControllers(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet("get-by-email-or-phone")]
        public async Task<IActionResult> GetUserByEmailOrPhone([FromQuery] string email = null, [FromQuery] string phone = null)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(phone))
            {
                return BadRequest(new { Message = "You must provide either an email or a phone number." });
            }

            try
            {
                // Call the service method with either email or phone
                var user = await _accountService.GetByEmailOrPhone(email, phone);

                return Ok(user);
            }
            catch (CustomHttpException ex)
            {
                return StatusCode((int)ex.StatusCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }
    
    [HttpPost("SendSMS")]
        public async Task<IActionResult> SendSMS(string phone)
        {
            try
            {
                var result = await _accountService.SendSMS(phone);
                return Ok(new { VerificationSid = result });
            }
            catch (CustomHttpException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            var token = await _accountService.Login(loginDTO);
            return Ok(new { token });
        }

        [HttpPost("LoginByPhone")]
        public async Task<IActionResult> LoginByPhone(string phone)
        {
            var token = await _accountService.LoginByPhone(phone);
            return Ok(new { token });
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(UserRegistrationDTO registrationDTO)
        {
            await _accountService.Registration(registrationDTO);
            return Ok();
        }

      
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(UserEditDTO editDTO)
        {
            await _accountService.Edit(editDTO);
            return Ok();
        }

        [HttpPost("DeleteUserImage")]
        public async Task<IActionResult> DeleteUserImageAsync([FromForm] string email)
        {
            await _accountService.DeleteUserImage(email);
            return Ok();
        }
        
        [HttpPost("EditUserImage")]
        public async Task<IActionResult> EditUserImageAsync(ImageUserEditDTO editDTO)
        {
            await _accountService.EditUserImageAsync(editDTO);
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                await _accountService.ForgotPasswordAsync(email);
                return Ok();
            }
            catch (CustomHttpException ex)
            {
                return NotFound();
            }
        }
        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            try
            {
                await _accountService.ResetPasswordAsync(model);
                return Ok();
            }
            catch (CustomHttpException ex)
            {
                return NotFound();
            }
        }
        
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO model)
        {
            try
            {
                await _accountService.ConfirmEmailAsync(model);
                return Ok();
            }
            catch (CustomHttpException ex)
            {
                return NotFound();
            }
        }
        
        [HttpPost("ConfirmMyEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] string email)
        {
            try
            {
                await _accountService.ConfirmEmailAsync(email);
                return Ok();
            }
            catch (CustomHttpException ex)
            {
                return NotFound();
            }
        }
        
        [HttpPut("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDTO refreshToken)
        {
            var newToken = await _accountService.RefreshTokenAsync(refreshToken.Token);
            return Ok(newToken);
        }


        // CRUD Category
        [HttpGet("UsersByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UsersByPage(int page)
        {
            var users = await _accountService.UsersyByPageAsync(page);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }
        
        [HttpGet("GetUserById/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        [HttpGet("UsersQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UsersQuantity()
        {
            var quantity = await _accountService.UsersQuantity();
            return Ok(quantity);
        }
        
        [HttpPost("CreateUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _accountService.CreateUserAsync(userCreateDTO);
            return Ok();
        }

        [HttpPost("EditUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUserAsync(UserEditDTO userEditDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _accountService.EditUserAsync(userEditDTO);
            return Ok();
        }

        [HttpDelete("DeleteUserByID/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUserByIDAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _accountService.DeleteUserByIDAsync(id);
            return Ok();
        }
    }
}
