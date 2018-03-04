using System;
using System.Linq;

namespace GPWebpayNet.Sdk
{
    /// <summary>
    /// Sdk extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Transform string into ASCII characters string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>String of ASCII characters.</returns>
        public static string GetInASCII(this string str)
        {
            str = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(str));
            return new string(str.Where(e =>
            {
                var num = (int) e;
                return num >= 32 && num <= 126;
            }).ToArray());
        }

        /// <summary>
        /// Gets the name of the enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Name of the enum value.</returns>
        public static string GetEnumValueName<T>(T value)
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}