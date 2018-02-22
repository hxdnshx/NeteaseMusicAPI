using System;
using System.Collections.Generic;
using System.Text;

namespace NeteaseMusicAPI
{
    public static class StringHelper
    {
        public static string ToBase64(this string src)
        {
            var bytes = Encoding.UTF8.GetBytes(src);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64(this string src)
        {
            var bytes = Convert.FromBase64String(src);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
