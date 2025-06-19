using Fitamas.Events;
using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Fitamas.UserInterface
{
    public static class StringExtension
    {
        public static int ConvertToInt(this string input)
        {
            input = Regex.Replace(input, "[^0-9-]", "");

            input = input.RemoveAt('-', 1);

            if (int.TryParse(input, out int result))
            {
                return result;
            }

            return 0;
        }

        public static float ConvertToFloat(this string input)
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

        public static int CountOf(this string input, char countChar)
        {
            return input.CountOf(countChar, 0, input.Length);
        }

        public static int CountOf(this string input, char countChar, int indexFrom)
        {
            return input.CountOf(countChar, indexFrom, input.Length - indexFrom);
        }

        public static int CountOf(this string input, char countChar, int indexFrom, int lenght)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            if (indexFrom + lenght > input.Length)
            {
                throw new Exception();
            }
            
            int count = 0;
            for (int i = indexFrom; i < lenght; i++)
            {
                if (input[i] == countChar)
                {
                    count++;
                }
            }

            return count;
        }

        public static int FirstIndexOfLine(this string input, int lineIndex)
        {
            if (string.IsNullOrEmpty(input))
            {
                return -1;
            }

            if (lineIndex == 0)
            {
                return 0;
            }

            char nextLine = '\n';
            int count = 0;
            int result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (nextLine == input[i])
                {
                    count++;
                    result = i + 1;

                    if (count == lineIndex)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public static string SubstringLine(this string input, int startIndex)
        {
            char nextLine = '\n';
            int num = input.IndexOf(nextLine, startIndex);

            if (input.Length == startIndex)
            {
                return "";
            }
            else if (num != -1)
            {
                return input.Substring(startIndex, num - startIndex);
            }
            else
            {
                return input;
            }
        }
    }
}
