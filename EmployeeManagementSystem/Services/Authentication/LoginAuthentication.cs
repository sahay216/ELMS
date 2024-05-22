using Domain.Models;
using Common.Encryption;
using Microsoft.Extensions.Options;

namespace Services.Authentication
{
    public class LoginAuthentication
    {
        private readonly EmployeeTrackerContext _trackerContext;
        private readonly IOptions<EncryptionSettings> _encryptionsettings;
        public LoginAuthentication(EmployeeTrackerContext trackerContext, IOptions<EncryptionSettings> encryptionsettings)
        {
            _trackerContext = trackerContext;
            _encryptionsettings = encryptionsettings;
        }

        public bool AuthLogin(string userPassword, string storedPasswordSalt, string storedPasswordHash)
        {
            string encryptionkey = _encryptionsettings.Value.EncryptionKey;
            var AesEncryptor = new AesEncryptor(Options.Create(_encryptionsettings.Value));
            string passwordhashed = AesEncryptor.Authencrypt(userPassword, storedPasswordSalt);

            if (passwordhashed == storedPasswordHash)
            {                
                return true;
            }
            return false;

        }
    }

}
