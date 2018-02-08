using System;
using System.Linq;

namespace GPWebpayNet.Sdk
{
    public static class Extensions
    {
        public static string GetInASCII(this string str)
        {
            str = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(str));
            return new string(str.Where(e =>
            {
                var num = (int) e;
                return num >= 32 && num <= 126;
            }).ToArray());
        }

        public static string GetEnumValueName<T>(T value)
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}