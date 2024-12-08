    using System.Text;

    namespace BudgifyAPI.Auth.CA.Entities;

    using System.Security.Cryptography;

    public static class CustomEncryptor
    {
        public static string EncryptString(string plaintext)
        {
            // Convert the plaintext string to a byte array
            byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);
    
            Byte[] key =
                Convert.FromBase64String( Environment.GetEnvironmentVariable(
                    "keys__budgify_key"));
           
    
            // Use the password to encrypt the plaintext
            Aes encryptor = Aes.Create();
            encryptor.Key = key[..32];
            encryptor.IV = key[..16];
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    
        public static string DecryptString(string encrypted)
        {
            // Convert the encrypted string to a byte array
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);
    
            Byte[] key =
                Convert.FromBase64String( Environment.GetEnvironmentVariable(
                    "keys__budgify_key"));
    
            // Use the password to encrypt the plaintext
            Aes encryptor = Aes.Create();
            encryptor.Key = key[..32];
            encryptor.IV = key[..16];
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }