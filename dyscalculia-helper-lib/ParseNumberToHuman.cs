using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

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

        public NUMBERFORMATS ConvertNumberToFormats (float number)
        {
            string[] numberIntDecimals = number.ToString().Split('.'); // TODO -- add setting for decimal separator

            string numberToWords = int.Parse(numberIntDecimals[0]).ToWords();
            
            if (numberIntDecimals.Length > 1)
            {
                numberToWords += " point " + int.Parse(numberIntDecimals[1]).ToWords();
            }


            return new NUMBERFORMATS
            {
                Number = number.ToString(),
                ThousandsSeparated = number.ToString("N0"),
                Words = numberToWords,
                PhoneNumber = number.ToString("00-00-00-00-")
            };
        }
    }
}
