using Microsoft.Extensions.Logging;
using SimpleX.Helpers.Settings;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SimpleX.Core.Crypto
{
    public interface ICryptoService
    {
        string Decrypt(string textToDecrypt);
        string Encrypt(string textToEncrypt);
    }

    public class CryptoService : ICryptoService
    {
        private readonly ILogger _logger;
        private readonly SecuritySettings _security;

        public CryptoService(ILoggerFactory loggerFactory, SecuritySettings security)
        {
            _logger = loggerFactory?.CreateLogger<CryptoService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            _security = security ??
                throw new ArgumentNullException(nameof(security));

            _logger.LogTrace("LoggerFactory injected in CryptoService");
            _logger.LogTrace("SecuritySettings injected in CryptoService");

        }

        public string Encrypt(string textToEncrypt)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(textToEncrypt);

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(_security.SecurityKey));

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return (Convert.ToBase64String(resultArray, 0, resultArray.Length));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public string Decrypt(string textToDecrypt)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(textToDecrypt);

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(_security.SecurityKey));

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider()
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };


                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
