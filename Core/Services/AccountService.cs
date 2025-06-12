using AutoMapper;
using Core.DTOs.User;
using Core.Entities.UserEntity;
using Core.Helpers;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Twilio.Rest.Verify.V2.Service;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;
        private readonly IFilesService _filesService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IImageService _image;
        private readonly IWebHostEnvironment _env;
        public string VerificationCode { get; set; }
        public AccountService(UserManager<User> userManager, IConfiguration configuration, IMapper mapper, IImageService image, EmailService emailService, IWebHostEnvironment env, IFilesService filesService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _image = image;
            _emailService = emailService;
            _env = env;
            _filesService = filesService;
        }
        private string GenerateRandomVerificationCode()
        {
            Random generator = new Random();
            VerificationCode = generator.Next(100000, 999999).ToString("D6"); // Generate a 6-digit random number
            return VerificationCode;
        }
        public async Task<UserDTO> GetByEmailOrPhone(string email = null, string phone = null)
        {


            // Fetch user by email or phone
            var user = !string.IsNullOrEmpty(email)
                ? await _userManager.FindByEmailAsync(email)
                : _userManager.Users.FirstOrDefault(x => x.PhoneNumber == phone);

            // Check if user was found
            if (user != null)
            {
                // Map to DTO
                var currentUser = _mapper.Map<UserDTO>(user);

                // Get user role
                var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                currentUser.Role = currentRole;

                return currentUser;
            }
            else
            {
                // Throw appropriate exception based on which parameter was used
                var errorMessage = !string.IsNullOrEmpty(email)
                    ? ErrorMessages.UserNotFoundByEmail
                    : ErrorMessages.UserNotFoundByPhone;

                throw new CustomHttpException(errorMessage, HttpStatusCode.NotFound);
            }
        }

        public async Task<string> SendSMS(string phone)
        {
            string verifyCode = GenerateRandomVerificationCode();

            var verification = await VerificationResource.CreateAsync(
            to: "+38" + phone,
            channel: "sms", // Specify the channel as SMS
            pathServiceSid: _configuration["Twilio:VerificationServiceSID"]);

            return verification.Sid;
        }
        public async Task<LoginByPhoneResultDTO> LoginByPhone(string phone)
        {
            var user = _userManager.Users.Where(x => x.PhoneNumber == phone).FirstOrDefault();
            if (user == null)
            {
                throw new CustomHttpException(ErrorMessages.UserNotFoundByPhone, HttpStatusCode.BadRequest);
            }
            string verifyCode = GenerateRandomVerificationCode();

            var verification = await VerificationResource.CreateAsync(
            to: "+38" + phone,
            channel: "sms", // Specify the channel as SMS
            pathServiceSid: _configuration["Twilio:VerificationServiceSID"]);


            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var claimsList = new List<Claim>()
            {

                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("ImagePath", user.ImagePath),
                new Claim("Role", currentRole!),
                new Claim("AuthType", user.AuthType),
                new Claim("Id", user.Id),
            };
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                claimsList.Add(new Claim("PhoneNumber", user.PhoneNumber));
            }
            var jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claimsList,
                expires: DateTime.Now.AddMinutes(jwtOptions.LifeTime),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var result = new LoginByPhoneResultDTO
            {
                sid = verification.Sid,
                token = tokenString
            };

            return result;
        }
        public async Task<string> Login(UserLoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                throw new CustomHttpException("Login or password cannot be null", HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (loginDTO.AuthType == "standard")
            {
                if (user == null)
                {
                    throw new CustomHttpException(ErrorMessages.ErrorLogin, HttpStatusCode.BadRequest);
                }
                if (!(await _userManager.CheckPasswordAsync(user, loginDTO.Password)))
                {
                    throw new CustomHttpException(ErrorMessages.ErrorPassword, HttpStatusCode.BadRequest);
                }
            }
            else if (loginDTO.AuthType == "google")
            {
                if (user == null)
                {
                    throw new CustomHttpException(ErrorMessages.ErrorLoginorPassword, HttpStatusCode.BadRequest);
                }
            }

            bool isUserLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow;
            if (isUserLockedOut)
            {
                throw new CustomHttpException("User is blocked", HttpStatusCode.BadRequest);
            }

            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var claimsList = new List<Claim>()
            {
                new Claim("Email", loginDTO.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Role", currentRole!),
                new Claim("AuthType", user.AuthType),
                new Claim("Id", user.Id),
            };
            if (!string.IsNullOrEmpty(user.ImagePath))
            {
                claimsList.Add(new Claim("ImagePath", user.ImagePath));
            }

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                claimsList.Add(new Claim("PhoneNumber", user.PhoneNumber));
            }
            var jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claimsList,
                expires: DateTime.Now.AddMinutes(jwtOptions.LifeTime),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task Registration(UserRegistrationDTO registrationDTO)
        {
            User user = _mapper.Map<User>(registrationDTO);

            if (user.AuthType == "standard")
            {
                var userExist = await _userManager.FindByEmailAsync(registrationDTO.Email);
                if (userExist != null)
                {
                    throw new CustomHttpException("Email already exists", HttpStatusCode.BadRequest);
                }

                user.UserName = registrationDTO.Email;

                if (registrationDTO.ImageFile != null)
                {
                    user.ImagePath = await _image.CreateUserImageAsync(registrationDTO.ImageFile);
                }

                var result = await _userManager.CreateAsync(user, registrationDTO.Password);
                //_userManager.AddToRoleAsync(user, "User").Wait();

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "User").Wait();
                }

                await SendConfirmationEmailAsync(registrationDTO.Email);

                if (!result.Succeeded)
                {
                    var messageError = string.Join(",", result.Errors.Select(er => er.Description));
                    throw new CustomHttpException(messageError, System.Net.HttpStatusCode.BadRequest);
                }
                return;
            }

            else if (user.AuthType == "phone")
            {

           
                user.UserName = registrationDTO.FirstName;

                user.PhoneNumberConfirmed = true;
                if (registrationDTO.ImageFile != null)
                {
                    user.ImagePath = await _image.CreateUserImageAsync(registrationDTO.ImageFile);
                }

                var resultPhone = await _userManager.CreateAsync(user);

                _userManager.AddToRoleAsync(user, "User").Wait();
                if (!resultPhone.Succeeded)
                {
                    var messageError = string.Join(",", resultPhone.Errors.Select(er => er.Description));
                    throw new CustomHttpException(messageError, System.Net.HttpStatusCode.BadRequest);
                }
            }

            else
            {
                user.UserName = registrationDTO.Email;

                user.EmailConfirmed = true;

                if (registrationDTO.ImagePath != null)
                {
                    user.ImagePath = await _image.SaveImageFromUrlAsync(registrationDTO.ImagePath);
                }
                user.ClientId = registrationDTO.ClientId;

                var resultgoogle = await _userManager.CreateAsync(user);
                _userManager.AddToRoleAsync(user, "User").Wait();
                if (!resultgoogle.Succeeded)
                {
                    var messageError = string.Join(",", resultgoogle.Errors.Select(er => er.Description));
                    throw new CustomHttpException(messageError, System.Net.HttpStatusCode.BadRequest);
                }
            }

        }
        public async Task Edit(UserEditDTO editDTO)
        {
            if (editDTO.ID != null)
            {
                var user = await _userManager.FindByIdAsync(editDTO.ID);
                if (user.AuthType == "standard")
                {
                    var pass = await _userManager.CheckPasswordAsync(user, editDTO.Password);
                    if (user == null || !pass)
                    {
                        throw new CustomHttpException(ErrorMessages.ErrorPassword, HttpStatusCode.BadRequest);
                    }
                    if (user.Email != editDTO.Email)
                    {
                        user.EmailConfirmed = false;
                        var confirmationResut = await _userManager.UpdateAsync(user);
                    }

                    if (!string.IsNullOrEmpty(editDTO.NewPassword))
                    {
                        user.Password = editDTO.NewPassword;
                        var result = await _userManager.ChangePasswordAsync(user, editDTO.Password, editDTO.NewPassword);
                        await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            throw new CustomHttpException("Failed to change password", HttpStatusCode.BadRequest);
                        }
                    }

                    if (user != null)
                    {
                        User updatedUser = _mapper.Map<User>(user);
                        updatedUser.UserName = editDTO.Email;
                        updatedUser.Email = editDTO.Email;
                        updatedUser.FirstName = editDTO.FirstName;
                        updatedUser.LastName = editDTO.LastName;
                        updatedUser.PhoneNumber = editDTO.PhoneNumber;
                        updatedUser.ImagePath = editDTO.ImagePath;
                        updatedUser.Birthday = editDTO.Birthday;
                        var result = await _userManager.UpdateAsync(updatedUser);
                        if (!result.Succeeded)
                        {
                            throw new CustomHttpException("Failed to update user", HttpStatusCode.BadRequest);
                        }
                    }
                }

                if (user.AuthType == "google")
                {
                    if (user == null)
                    {
                        throw new CustomHttpException(ErrorMessages.ErrorLoginorPassword, HttpStatusCode.BadRequest);
                    }

                    if (user != null)
                    {
                        User updatedUser = _mapper.Map<User>(user);
                        updatedUser.UserName = editDTO.Email;
                        updatedUser.Email = editDTO.Email;
                        updatedUser.FirstName = editDTO.FirstName;
                        updatedUser.LastName = editDTO.LastName;
                        updatedUser.PhoneNumber = editDTO.PhoneNumber;
                        updatedUser.ImagePath = editDTO.ImagePath;
                        updatedUser.Birthday = editDTO.Birthday;
                        var result = await _userManager.UpdateAsync(updatedUser);
                        if (!result.Succeeded)
                        {
                            throw new CustomHttpException("Failed to update user", HttpStatusCode.BadRequest);
                        }
                    }
                }

                if (user.AuthType == "phone")
                {

                    if (user.Email != editDTO.Email)
                    {
                        user.EmailConfirmed = false;
                        var confirmationResut = await _userManager.UpdateAsync(user);
                    }

                    if (!string.IsNullOrEmpty(editDTO.NewPassword))
                    {
                        user.Password = editDTO.NewPassword;
                        var result = await _userManager.ChangePasswordAsync(user, editDTO.Password, editDTO.NewPassword);
                        await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            throw new CustomHttpException("Failed to change password", HttpStatusCode.BadRequest);
                        }
                    }

                    if (user != null)
                    {
                        User updatedUser = _mapper.Map<User>(user);
                        updatedUser.UserName = editDTO.Email;
                        updatedUser.Email = editDTO.Email;
                        updatedUser.FirstName = editDTO.FirstName;
                        updatedUser.LastName = editDTO.LastName;
                        updatedUser.PhoneNumber = editDTO.PhoneNumber;
                        updatedUser.ImagePath = editDTO.ImagePath;
                        updatedUser.Birthday = editDTO.Birthday;
                        var result = await _userManager.UpdateAsync(updatedUser);
                        if (!result.Succeeded)
                        {
                            throw new CustomHttpException("Failed to update user", HttpStatusCode.BadRequest);
                        }
                    }
                }
            }
        }
        public async Task DeleteUserImage(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.ImagePath = "";
                await _userManager.UpdateAsync(user);
            }
        }
        public async Task EditUserImageAsync(ImageUserEditDTO editDTO)
        {
            var user = await _userManager.FindByEmailAsync(editDTO.Email);
            if (user != null)
            {
                User updatedUser = _mapper.Map<User>(user);
                updatedUser.ImagePath = editDTO.ImagePath;
                await _userManager.UpdateAsync(updatedUser);
            }
        }
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new CustomHttpException(ErrorMessages.ErrorLoginorPassword, HttpStatusCode.BadRequest);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);

            string templateFilePath = Path.Combine(_env.WebRootPath, "email", "ForgotPasswordTemplate.html");
            string emailBody = File.ReadAllText(templateFilePath);

            string url = $"{_configuration["HostSettings:URL"]}/auth/{email}/{validEmailToken}";

            emailBody = emailBody.Replace("{url}", url);

            await _emailService.SendEmailAsync(email, "Reset password", emailBody);
        }
        public async Task ResetPasswordAsync(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new CustomHttpException(ErrorMessages.ErrorLoginorPassword, HttpStatusCode.BadRequest);
            }
            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            try
            {
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
                    user.Password = model.NewPassword;
                    await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new CustomHttpException("Failed to reset password", HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomHttpException($"Error resetting password: {ex.Message}", HttpStatusCode.InternalServerError);
            }

        }
        public async Task SendConfirmationEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new CustomHttpException(ErrorMessages.ErrorLoginorPassword, HttpStatusCode.BadRequest);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);

            string templateFilePath = Path.Combine(_env.WebRootPath, "email", "ConfirmationEmailTemplate.html");
            string emailBody = File.ReadAllText(templateFilePath);

            string url = $"{_configuration["HostSettings:URL"]}/{email}/{validEmailToken}";

            emailBody = emailBody.Replace("{url}", url);

            await _emailService.SendEmailAsync(email!, "Email confirmation", emailBody);
        }
        public async Task ConfirmEmailAsync(ConfirmEmailDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new CustomHttpException(ErrorMessages.ErrorLoginorPassword, HttpStatusCode.BadRequest);
            }
            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            try
            {
                var result = await _userManager.ConfirmEmailAsync(user, normalToken);
                if (!result.Succeeded)
                {
                    throw new CustomHttpException("Failed to confirm your email", HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                throw new CustomHttpException($"Error confirm email: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
        public async Task ConfirmEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new CustomHttpException(ErrorMessages.ErrorLoginorPassword, HttpStatusCode.BadRequest);
            }
            try
            {
                await SendConfirmationEmailAsync(user.Email);

            }
            catch (Exception ex)
            {
                throw new CustomHttpException($"Error confirm email: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
        public async Task<string> RefreshTokenAsync(string oldToken)
        {
            var principal = GetPrincipalFromToken(oldToken);
            if (principal == null)
                throw new CustomHttpException(ErrorMessages.UserNotFoundById, HttpStatusCode.NotFound);

            // Отримання ідентифікатора користувача з старого токену
            var userId = principal.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (userId == null)
                throw new CustomHttpException(ErrorMessages.UserNotFoundById, HttpStatusCode.BadRequest);

            // Знаходження користувача за ідентифікатором
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new CustomHttpException(ErrorMessages.UserNotFoundById, HttpStatusCode.NotFound);

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (role == null)
                throw new CustomHttpException(ErrorMessages.UserNotFoundById, HttpStatusCode.NotFound);

            // Генерація нового токену
            var newToken = GenerateToken(user, role);

            return newToken;
        }
        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
        private string GenerateToken(User user, string role)
        {
            var claimsList = new List<Claim>()
            {
                new Claim("Email", user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Role", role!),
                new Claim("AuthType", user.AuthType),
                new Claim("Id", user.Id),
            };
            if (!string.IsNullOrEmpty(user.ImagePath))
            {
                claimsList.Add(new Claim("ImagePath", user.ImagePath));
            }

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                claimsList.Add(new Claim("PhoneNumber", user.PhoneNumber));
            }
            var jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claimsList,
                expires: DateTime.Now.AddMinutes(jwtOptions.LifeTime),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Admin Panel
        public async Task<List<UserDTO>> UsersyByPageAsync(int page)
        {
            var usersQuery = _userManager.Users.UsersByPage(page);

            var users = usersQuery.ToList();
            if (users == null || !users.Any())
            {
                return null;
            }
            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync((User)user);
                var userDto = _mapper.Map<UserDTO>(user);
                userDto.Role = roles.FirstOrDefault()!;
                userDtos.Add(userDto);
            }
            return userDtos;
        }
        public async Task<int> UsersQuantity()
        {
            return _userManager.Users.Count();
        }
        public async Task CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            User user = _mapper.Map<User>(userCreateDTO);

            var userExist = await _userManager.FindByEmailAsync(userCreateDTO.Email);
            if (userExist != null)
            {
                throw new CustomHttpException("Email already exists", HttpStatusCode.BadRequest);
            }

            user.UserName = userCreateDTO.Email;

            var result = await _userManager.CreateAsync(user, userCreateDTO.Password);
            if (userCreateDTO.Role != null)
            {
                _userManager.AddToRoleAsync(user, userCreateDTO.Role).Wait();
            }

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User").Wait();
            }

            await SendConfirmationEmailAsync(userCreateDTO.Email);

            if (!result.Succeeded)
            {
                var messageError = string.Join(",", result.Errors.Select(er => er.Description));
                throw new CustomHttpException(messageError, System.Net.HttpStatusCode.BadRequest);
            }
            return;
        }
        public async Task EditUserAsync(UserEditDTO userEditDTO)
        {
            if (userEditDTO.ID != null)
            {
                var user = await _userManager.FindByIdAsync(userEditDTO.ID);

                if (user.Email != userEditDTO.Email)
                {
                    user.EmailConfirmed = false;
                    await _userManager.UpdateAsync(user);
                }

                if (!string.IsNullOrEmpty(userEditDTO.Password))
                {
                    var result = await _userManager.ChangePasswordAsync(user, user.Password, userEditDTO.Password);
                    user.Password = userEditDTO.Password;
                    await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new CustomHttpException("Failed to change password", HttpStatusCode.BadRequest);
                    }
                }

                if (user != null)
                {
                    User updatedUser = _mapper.Map<User>(user);
                    updatedUser.UserName = userEditDTO.Email;
                    updatedUser.Email = userEditDTO.Email;
                    updatedUser.FirstName = userEditDTO.FirstName;
                    updatedUser.LastName = userEditDTO.LastName;
                    updatedUser.PhoneNumber = userEditDTO.PhoneNumber;
                    updatedUser.ImagePath = userEditDTO.ImagePath;
                    updatedUser.Birthday = userEditDTO.Birthday;
                    updatedUser.AuthType = userEditDTO.AuthType;

                    if (userEditDTO.IsBlocked.HasValue)
                    {
                        user.LockoutEnd = userEditDTO.IsBlocked.Value ? DateTime.UtcNow.AddYears(100) : (DateTime?)null;
                        //userEditDTO.IsBlocked.Value ? DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc) : null;
                    }

                    var result = await _userManager.UpdateAsync(updatedUser);
                    
                    if (!result.Succeeded)
                    {
                        throw new CustomHttpException("Failed to update user", HttpStatusCode.BadRequest);
                    }
                }
                if (userEditDTO.Role != null)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        throw new CustomHttpException("Failed to remove user from roles", HttpStatusCode.BadRequest);
                    }

                    var addResult = await _userManager.AddToRoleAsync(user, userEditDTO.Role);
                    if (!addResult.Succeeded)
                    {
                        throw new CustomHttpException("Failed to add user to role", HttpStatusCode.BadRequest);
                    }
                }
            }
        }
        public async Task DeleteUserByIDAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.ImagePath != null)
                {
                    await _filesService.DeleteUserImageAsync(user.ImagePath!);
                }
                await _userManager.DeleteAsync(user);
            }
        }
        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var currentUser = _mapper.Map<UserDTO>(user);
                var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                if (currentRole != null)
                    currentUser.Role = currentRole;

                bool isUserLockedOut = await _userManager.IsLockedOutAsync(user);
                currentUser.IsBlocked = isUserLockedOut;

                return currentUser;
            }
            else
            {
                throw new CustomHttpException(ErrorMessages.UserNotFoundByEmail, HttpStatusCode.NotFound);
            }
        }
    }
}