using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using System.Globalization;

namespace dyscalculia_helper_lib
{
    public class ParseNumberToHuman
    {
        public struct NUMBERFORMATS
        {
            public string Number;
            public string ThousandsSeparated;
            public string Words;
            public string PhoneNumber;
        }

        public static NUMBERFORMATS ConvertNumberToFormats (decimal number, char decimalSeparator)
        {
            var numberString = number.ToString();
            var numberToWords = "";

            // Split the number into its integer and decimal parts, and ToWords() both separately as humanizer doesn't support decimal numbers
            if (numberString.Contains(decimalSeparator))
            {
                var numberParts = numberString.Split(decimalSeparator);
                numberToWords = Convert.ToInt64(numberParts[0]).ToWords() + " point " + Convert.ToInt64(numberParts[1]).ToWords();
            }
            else
            {
                numberToWords = Convert.ToInt64(number).ToWords();
            }


            return new NUMBERFORMATS
            {
                Number = number.ToString(),
                ThousandsSeparated = number.ToString("N0"),
                Words = numberToWords,
                PhoneNumber = number.ToString("00 00 00 00")
            };
        }

        public static decimal AttemptParseNumber(string text, char decimalSeparator = ',')
        {
            var numberFormatInfo = new NumberFormatInfo
            {
                NumberDecimalSeparator = decimalSeparator.ToString()
            };


            if (decimal.TryParse(text, NumberStyles.Float, numberFormatInfo, out decimal parsedNumber))
            {
                return parsedNumber;
            }

            return decimal.MinValue; 
        }
    }
}
