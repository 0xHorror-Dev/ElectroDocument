using ElectroDocument.Controllers.AppContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace ElectroDocument.Models
{
    public static class Utils
    {
        public static string sha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2")); // Convert byte to hexadecimal string
                }
                return builder.ToString();
            }
        }

        static string[] units = { "", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
        static string[] teens = { "", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
        static string[] tens = { "", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
        static string[] hundreds = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
        static string[] thousands = { "", "тысяча", "тысячи", "тысяч" };

        public static string ConvertNumberToWords(int number)
        {
            if (number == 0)
                return "ноль";

            if (number < 0)
                return "минус " + ConvertNumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000) > 0)
            {
                words += ConvertNumberToWords(number / 1000) + " " + GetNounForm(number / 1000, thousands) + " ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += hundreds[number / 100] + " ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "и ";

                if (number < 10)
                    words += units[number];
                else if (number < 20)
                    words += teens[number - 10];
                else
                {
                    words += tens[number / 10];
                    if ((number % 10) > 0)
                        words += " " + units[number % 10];
                }
            }

            return words;
        }

        public static string GetNounForm(int number, string[] forms)
        {
            if (number % 10 == 1 && number % 100 != 11)
                return forms[1];
            else if ((number % 10 >= 2 && number % 10 <= 4) && !(number % 100 >= 12 && number % 100 <= 14))
                return forms[2];
            else
                return forms[3];
        }
    }

    public static class MyExtensions
    {
        public static IEnumerable<string> OrderByCyrillicFirst(this IEnumerable<string> list)
        {
            var cyrillicOrderedList = list.Where(l => string.IsNullOrEmpty(l) ? false : IsCyrillic(l[0])).OrderBy(l => l);
            var latinOrderedList = list.Where(l => string.IsNullOrEmpty(l) ? true : !IsCyrillic(l[0])).OrderBy(l => l);
            return cyrillicOrderedList.Concat(latinOrderedList);
        }

        public static IEnumerable<string> OrderByCyrillicFirstDescending(this IEnumerable<string> list)
        {
            var cyrillicOrderedList = list.Where(l => string.IsNullOrEmpty(l) ? false : IsCyrillic(l[0])).OrderByDescending(l => l);
            var latinOrderedList = list.Where(l => string.IsNullOrEmpty(l) ? true : !IsCyrillic(l[0])).OrderByDescending(l => l);
            return cyrillicOrderedList.Concat(latinOrderedList);
        }


        //cyrillic symbols start with code 1024 and end with 1273.
        private static bool IsCyrillic(char ch) =>
            ch >= 1024 && ch <= 1273;
    }
}

