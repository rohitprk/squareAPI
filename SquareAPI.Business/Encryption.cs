using System.Security.Cryptography;
using System.Text;

namespace SquareAPI.Business
{
    /// <summary>
    /// Class to define extension methods to encrypt and decrypt data.
    /// </summary>
    public static class Encryption
    {
        /// <summary>
        /// Encrypt data using AES Algorithm.
        /// </summary>
        /// <param name="data">Data to encrypt.</param>
        /// <param name="key">Symmetric key for AES Encryption.</param>
        /// <param name="vector">Vector to start encryption.</param>
        /// <returns>Encrypted base64 string</returns>
        public static string AESEncrypt(this string data, string key)
        {
            byte[] array;
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Padding = PaddingMode.PKCS7;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(data);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// Decrypted AES encrypted data.
        /// </summary>
        /// <param name="data">Data to decrypt.</param>
        /// <param name="key">Symmetric key for AES Encryption.</param>
        /// <returns>Decrypted string.</returns>
        public static string AESDecrypt(this string data, string key)
        {
            var byteData = Convert.FromBase64String(data);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16];
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream memoryStream = new MemoryStream(byteData))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generate HMACSHA256 hash.
        /// </summary>
        /// <param name="data">Data to hash.</param>
        /// <param name="key">Key for HMACSHA256.</param>
        /// <returns>Hashed base64 string data.</returns>
        public static string GenerateHash(this string data, string key)
        {
            var hash = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            byte[] hashedData = hash.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashedData);
        }
    }
}