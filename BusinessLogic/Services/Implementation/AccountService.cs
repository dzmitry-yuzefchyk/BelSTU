using BusinessLogic.Constants;
using BusinessLogic.Models.Account;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Configuration;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogic.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly TaskboardContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        //TODO: fix params in here
        public AccountService(TaskboardContext context, UserManager<User> userManager, IConfiguration configuration, IWebHostEnvironment env,
            SignInManager<User> signInManager, ILoggerFactory loggerFactory, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<FileLogger>();
            _emailSender = emailSender;
            _config = configuration;
            _env = env;
        }

        public async Task<(bool IsDone, string Message)> SignUpAsync(RegistrationModel model, string clientAppHost)
        {
            try
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                await _userManager.CreateAsync(user, model.Password);
                var userSettings = new UserSettings
                {
                    Id = user.Id,
                    EmailNotifications = false,
                    Theme = (int)Theme.LIGHT
                };
                var userProfile = new UserProfile
                {
                    Id = user.Id,
                    Tag = $"{model.Email.Split('@').First()}#{model.Email.GetHashCode()}"
                };
                await _context.UserProfiles.AddAsync(userProfile);
                await _context.UserSettings.AddAsync(userSettings);
                var result = await _context.SaveChangesAsync();
                if (result < 0)
                {
                    _logger.LogError($"Account service, SignUpAsync()", model.Email, result);
                    return (IsDone: false, Message: "Can't create such user");
                }

                return await SendConfirmEmailAsync(model.Email, clientAppHost);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: SignUpAsync()", e);
                return (IsDone: false, Message: "Can't create such user, please try again later");
            }
        }

        public async Task<(bool IsDone, string Message, string Token)> SignInAsync(LoginModel model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.IsNotAllowed)
                {
                    _logger.LogWarning("AccountService: SignInAsync", model.Email);
                    return (IsDone: false, Message: "Email is not confirmed", Token: null);
                }
                else if (!result.Succeeded)
                {
                    _logger.LogError("AccountService: SignInAsync, can't signin user", model.Email);
                    return (IsDone: false, Message: "Email or password is wrong", Token: null);
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = GenerateJSONWebToken(user);
                return (IsDone: true, Message: "Welcome!", Token: token);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: SignInAsync, can't signin user", e);
                return (IsDone: false, Message: "Something went wrong, please try again later", Token: null);
            }
        }

        public async Task<bool> SignOutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: SignOutAsync()", e);
                return false;
            }
        }

        public async Task<(bool IsDone, string Message)> ConfirmEmailAsync(string email, string token)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("AccountService: ConfirmEmailAsync", "Can't find such user", email);
                    return (IsDone: false, Message: "There is no user with such email");
                }

                var codeDecodedBytes = WebEncoders.Base64UrlDecode(token);
                var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, codeDecoded);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("AccountService: ConfirmEmailAsync", "Can't activate email", email, token);
                    return (IsDone: false, Message: "Can't confirm email, please try again later");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: ConfrimEmailAsync()", e);
                return (IsDone: false, Message: "Can't confirm email, please try again later");
            }

            return (IsDone: true, Message: "You may now sign-in");
        }

        public async Task<(bool IsDone, string Message)> SendConfirmEmailAsync(string email, string clientAppHost)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("AccountService: ResendConfirmEmailAsync", "Can't find such user", email);
                    return (IsDone: false, Message: "There is no user with such email");
                }

                var profile = _context.UserProfiles.Find(user.Id);
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var emailConfirmationToken = HttpUtility.HtmlEncode(code);
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(emailConfirmationToken);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var emailResult = await SendEmailConfirmationAsync(email, profile.Tag.Split('#').First(), codeEncoded, clientAppHost);
                if (!emailResult)
                {
                    _logger.LogWarning($"Account service, ResendConfirmEmailAsync", email);
                    return (IsDone: true, Message: "Cant send email, please try again later");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: SendConfirmEmailAsync", e);
                return (IsDone: false, Message: "Can't send email, please try again later");
            }

            return (IsDone: true, Message: "Confirmation was sent to your email");
        }

        public async Task<(bool IsDone, string Message)> UpdateProfileAsync(Guid userId, UpdateProfileModel model)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning("AccountService: UpdateProfileAsync, can't find user", userId);
                    return (IsDone: false, Message: "There is no such user");
                }

                var profile = await _context.UserProfiles.FindAsync(userId);
                profile.Name = model.Name;
                profile.Tag = $"{model.Tag}#{user.Email.GetHashCode()}";
                profile.Icon = model.Icon;

                _context.UserProfiles.Update(profile);
                var updateResult = await _context.SaveChangesAsync();

                if (updateResult < 0)
                {
                    _logger.LogWarning("AccountService: UpdateProfileAsync, can't update profile", userId);
                    return (IsDone: false, Message: "Something went wrong, please try again later");
                }

                return (IsDone: true, Message: "Success");
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: UpdateProfileAsync", e);
                return (IsDone: false, Message: "Something went wrong, please try again later");
            }
        }

        public async Task<(bool IsDone, string Message)> UpdateSettingsAsync(Guid userId, UpdateSettingsModel model)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning("AccountService: UpdateSettingsAsync, can't find user", userId);
                    return (IsDone: false, Message: "There is no such user");
                }

                var settings = await _context.UserSettings.FindAsync(userId);
                settings.EmailNotifications = model.EmailNotifications;
                settings.Theme = model.Theme;

                _context.UserSettings.Update(settings);
                var updateResult = await _context.SaveChangesAsync();

                if (updateResult < 0)
                {
                    _logger.LogWarning("AccountService: UpdateSettingsAsync, can't update profile", userId);
                    return (IsDone: false, Message: "Something went wrong, please try again later");
                }

                return (IsDone: true, Message: "Success");
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: UpdateSettingsAsync", e);
                return (IsDone: false, Message: "Something went wrong, please try again later");
            }
        }

        public async Task<(UserProfileModel Model, string Message)> GetProfile(Guid userId)
        {
            try
            {
                var profile = await _context.UserProfiles.FindAsync(userId);
                if (profile == null)
                {
                    _logger.LogWarning("AccountService: GetProfile, can't find profile", userId);
                    return (Model: null, Message: "Can't find profile, please try again later");
                }

                var profileModel = new UserProfileModel
                {
                    Icon = profile.Icon,
                    Name = profile.Name,
                    Tag = profile.Tag.Split('#').First()
                };
                return (Model: profileModel, Message: null);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: GetProfile", e);
                return (Model: null, Message: "Can't find profile, please try again later");
            }
        }

        public async Task<(UserSettingsModel Model, string Message)> GetSettings(Guid userId)
        {
            try
            {
                var settings = await _context.UserSettings.FindAsync(userId);
                if (settings == null)
                {
                    _logger.LogWarning("AccountService: GetSettings, can't find settings", userId);
                    return (Model: null, Message: "Can't find settings, please try again later");
                }

                var settingsModel = new UserSettingsModel
                {
                    EmailNotifications = settings.EmailNotifications,
                    Theme = settings.Theme
                };
                return (Model: settingsModel, Message: null);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: GetSettings", e);
                return (Model: null, Message: "Can't find settings, please try again later");
            }
        }

        private async Task<bool> SendEmailConfirmationAsync(string email, string tag, string token, string clientAppHost)
        {
            try
            {
                var link = $"<a href=\"https://{clientAppHost}/confirmEmail?token={token}&email={email}\">Click Me!</a>";
                var htmlMessage = $"Dear, {tag}, {link}";
                await _emailSender.SendEmailAsync(email, "Confirmation", htmlMessage);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountService: SendEmailConfirmationAsync()", e);
                return false;
            }
            return true;
        }

        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>(_env, "", "Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = SetupUserClaims(user);

            var token = new JwtSecurityToken(_config.GetValue<string>(_env, "", "Jwt:Issuer"),
              _config.GetValue<string>(_env, "", "Jwt:Issuer"),
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private List<Claim> SetupUserClaims(User user) => new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Email",user.Email)
        };
    }
}
