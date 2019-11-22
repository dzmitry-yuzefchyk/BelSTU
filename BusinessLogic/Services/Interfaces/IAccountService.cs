using BusinessLogic.Models.Account;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool IsDone, string Message, string Token)> SignInAsync(LoginModel model);
        Task<(bool IsDone, string Message)> SignUpAsync(RegistrationModel model);
        Task<bool> SignOutAsync();
        Task<(bool IsDone, string Message)> ConfirmEmailAsync(string email, string token);
        Task<(bool IsDone, string Message)> SendConfirmEmailAsync(string email);
        Task<(bool IsDone, string Message)> UpdateProfileAsync(Guid userId, UpdateProfileModel model);
        Task<(bool IsDone, string Message)> UpdateSettingsAsync(Guid userId, UpdateSettingsModel model);
        Task<(UserProfileModel Model, string Message)> GetProfile(Guid userId);
        Task<(UserSettingsModel Model, string Message)> GetSettings(Guid userId);
    }
}
