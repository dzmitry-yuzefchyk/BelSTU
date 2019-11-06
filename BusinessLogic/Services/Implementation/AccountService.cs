using BusinessLogic.Constants;
using BusinessLogic.Models.Account;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly TaskboardContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public AccountService(TaskboardContext context, UserManager<User> userManager,
            ILoggerFactory loggerFactory, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<FileLogger>();
            _emailSender = emailSender;
        }

        public async Task<bool> SignUpAsync(RegistrationModel model)
        {
            var userSettings = new UserSettings
            {
                EmailNotifications = false,
                Theme = (int)Theme.LIGHT
            };
            var userProfile = new UserProfile { };
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Settings = userSettings,
                Profile = userProfile
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError($"Account service, SignUpAsync: {model.Email}");
                return false;
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebUtility.HtmlEncode(code);

            return true;
        }

        private async Task<bool> SendEmailConfirmationAsync()
        {
            return false;
        }
    }
}
