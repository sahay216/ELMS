using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;
using Azure.Core.Cryptography;
using Microsoft.Extensions.Options;

namespace Common.Encryption
{
    //code to generate key 
    public class AesEncryptor
    {
        private readonly string _encryptionKey;
        public AesEncryptor(IOptions<EncryptionSettings> settings)
        {
            _encryptionKey = settings.Value.EncryptionKey;
        }


        public string Encrypt(string Plaintext, out string IVkey)
        {
            using Aes aesObject = Aes.Create();

            aesObject.Key = Convert.FromBase64String(_encryptionKey);
            aesObject.BlockSize = 128;
            aesObject.Padding = PaddingMode.Zeros;
            aesObject.GenerateIV();

            IVkey = Convert.ToBase64String(aesObject.IV);

            ICryptoTransform encryptor = aesObject.CreateEncryptor();

            byte[] encryptdata;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(Plaintext);

                    }
                    encryptdata = ms.ToArray();
                }
            }


            return Convert.ToBase64String(encryptdata);
        }


        public string Authencrypt(string Password, in string IVkey)
        {
            using Aes aesObject = Aes.Create();
            aesObject.Key = Convert.FromBase64String(_encryptionKey);
            aesObject.BlockSize = 128;
            Console.WriteLine(IVkey);                                                  //remove
            aesObject.Padding = PaddingMode.Zeros; //key generation logic 
            aesObject.IV = Convert.FromBase64String(IVkey);

            Console.WriteLine("validation\n\n");                                      //remove
            foreach (byte i in aesObject.IV)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine("\n\n");


            ICryptoTransform enryptor = aesObject.CreateEncryptor();
            byte[] encryptedData;
            //here the decrypted plain text will be stored  
            // byte[] passwordBytes = Convert.FromBase64String(Password);
            using (MemoryStream ms = new MemoryStream())
            {
                using CryptoStream cs = new CryptoStream(ms, enryptor, CryptoStreamMode.Write);
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(Password);

                }
                encryptedData = ms.ToArray();
            }


            Console.WriteLine(Convert.ToBase64String(encryptedData));
            return Convert.ToBase64String(encryptedData);
        }

    }
}


