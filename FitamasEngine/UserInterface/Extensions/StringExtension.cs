using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Fitamas.UserInterface
{
    public static class StringExtension
    {
        public static int ConvertToInt(this string input) //TODO fix
        {
            input = Regex.Replace(input, "[^0-9-]", "");

            input = input.RemoveAt('-', 1);

            if (int.TryParse(input, out int result))
            {
                return result;  
            }

            return 0;
        }

        public static float ConvertToFloat(this string input) //TODO fix
        {
            input = Regex.Replace(input, "[^0-9.-]", "");

            int count = input.IndexOf('.');
            if (count >= 0)
            {
                count++;
                input = input.RemoveAt('.', count);
            }

            input = input.RemoveAt('-', 1);

            if (float.TryParse(input, out float result))
            {
                return result;
            }

            return 0;
        }

        public static string RemoveAt(this string input, char removeChar, int indexFrom)
        {
            if (indexFrom >= input.Length)
            {
                return input;
            }

            int current = input.IndexOf(removeChar, indexFrom);

            while (current != -1)
            {
                input = input.Remove(current, 1);
                current = input.IndexOf(removeChar, current);
            }

            return input;
        }
    }
}
