using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public class Encryption
    {
        private string algorithm { get; set; }
        private TripleDESCryptoServiceProvider GenerateProvider()
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            provider.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(algorithm));
            hashmd5.Clear();

            provider.Mode = CipherMode.ECB;
            provider.Padding = PaddingMode.PKCS7;

            return provider;
        }

        public Encryption(string algorithm)
        {
            this.algorithm = algorithm;
        }

        public string Encrypt(string value)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(value);

            TripleDESCryptoServiceProvider provider = GenerateProvider();
            ICryptoTransform cTransform = provider.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(byteArray, 0, byteArray.Length);
            provider.Clear();

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string Decrypt(string encrypt64)
        {
            try
            {
                byte[] byteArray = Convert.FromBase64String(encrypt64.Replace(" ", "+"));

                TripleDESCryptoServiceProvider provider = GenerateProvider();
                ICryptoTransform cTransform = provider.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(byteArray, 0, byteArray.Length);
                provider.Clear();

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
