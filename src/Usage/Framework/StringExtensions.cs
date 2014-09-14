using System;
using System.Security.Cryptography;
using System.Text;

namespace Usage.Framework
{
    internal static class StringExtensions
    {
        private static readonly Encoding Encoding = new UTF8Encoding(false);

        public static byte[] ToMD5Hash(this string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            using (var _ = MD5.Create())
                return _.ComputeHash(Encoding.GetBytes(value));
        }
    }
}