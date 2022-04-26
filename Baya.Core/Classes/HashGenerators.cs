using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace Baya.Core.Classes
{
   public class HashGenerators
    {
        public static string HashWithMD5(string password)
        {
            Byte[] mainBytes;
            Byte[] encodeBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            mainBytes = ASCIIEncoding.Default.GetBytes(password);
            encodeBytes = md5.ComputeHash(mainBytes);
            return BitConverter.ToString(encodeBytes);

        }
    }
}
