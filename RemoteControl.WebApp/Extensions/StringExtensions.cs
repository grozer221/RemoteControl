using System.Security.Cryptography;
using System.Text;

namespace RemoteControl.WebApp.Extensions;

public static class StringExtensions
{
    public static string CreateMD5(this string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.Unicode.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }

    public static string CreateMD5WithSalt(this string input, out string salt)
    {
        salt = CreateSalt(30);
        return input.CombiteWithSalt(salt).CreateMD5();
    }

    public static string CombiteWithSalt(this string input, string salt)
    {
        return input + salt;
    }

    public static string CreateSalt(int size)
    {
        var rng = new RNGCryptoServiceProvider();
        byte[] buff = new byte[size];
        rng.GetBytes(buff);
        return Convert.ToBase64String(buff);
    }
}
