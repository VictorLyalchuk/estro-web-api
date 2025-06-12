using Core.DTOs.User;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        Task<UserDTO> GetByEmailOrPhone(string email = null, string phone = null);
        Task<string> SendSMS(string phone);
        Task<string> Login(UserLoginDTO loginDTO);
        Task<LoginByPhoneResultDTO> LoginByPhone(string phone);
        Task Registration(UserRegistrationDTO registrationDTO);
        Task Edit(UserEditDTO editDTO);
        Task DeleteUserImage(string email);
        Task EditUserImageAsync(ImageUserEditDTO editDTO);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDTO model);
        Task SendConfirmationEmailAsync(string email);
        Task ConfirmEmailAsync(ConfirmEmailDTO model);
        Task ConfirmEmailAsync(string email);
        Task<string> RefreshTokenAsync(string oldToken);
        Task<List<UserDTO>> UsersyByPageAsync(int page);
        Task<int> UsersQuantity();
        Task CreateUserAsync(UserCreateDTO userCreateDTO);
        Task EditUserAsync(UserEditDTO userEditDTO);
        Task DeleteUserByIDAsync(string id);
        Task <UserDTO> GetUserByIdAsync(string id);
    }
}
