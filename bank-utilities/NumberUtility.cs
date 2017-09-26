using System;
using System.Numerics;

namespace Ekoodi.Utilities.Finance
{
    public static class NumberUtility
    {
        // This method takes in four parameters: the string to manipulate, starting position, desired string length and the character (set) which to insert. It keeps inserting the character (set)
        // to the string at the starting position until the string has the desired length. It is recommended to use a single character string as a character (set)
        public static string AddZeroPadding(string stringToPad, int startingPosition, int desiredLength, string characterSet = "0")
        {
            while (stringToPad.Length < desiredLength)
            {
                stringToPad = stringToPad.Insert(startingPosition, characterSet);
            }

            return stringToPad;
        }

        // This method takes in one mandatory and three optional parameters: input string, minimum length, maximum length and variable type. It checks
        // whether the input string can be parsed into the variable type and whether the string meets the minimum and maximum length requirements. Only
        // BigInteger (default) and double can be used as a variable type.
        public static bool CheckNumberLength(string inputNumber, int minLength = 1, int maxLength = 999, string variabletype = "BigInteger")
        {
            // If the input reference number consists solely of numbers and the length is between given parameters, return true
            if (variabletype != "double")
            {
                return (BigInteger.TryParse(inputNumber, out BigInteger success) && inputNumber.Length >= minLength && inputNumber.Length <= maxLength);
            }
            else
            {
                return (double.TryParse(inputNumber, out double successdouble) && double.Parse(inputNumber).ToString("F0").Length >= minLength && double.Parse(inputNumber).ToString("F0").Length <= maxLength);
            }
        }

        // This method takse in three parameters: input string, desired digit group length and a boolean to indicate whether the shorter digit group should
        // be placed in the beginning of the string (rather than in the end). It returns a string in which the digits are grouped by these rules. The input
        // string can contain any characters, not only numerical ones
        public static string GroupDigits(string inputNumber, int desiredDigitGroupLength, bool shorterGroupInBeginning)
        {
            string inputNumberGrouped = "";

            // By default, digit group length is set in a method parameter. However, when the digits cannot equally
            // be divided into groups of such length, the length of the first group is the modulo of total length
            // divided by the parameter

            int digitGroupLength;

            // string spaceBeforeDigitGroup = "";

            // Loop through each digit group
            for (int i = 0; i < inputNumber.Length; i = i + digitGroupLength)
            {
                // By default, the length of the digit group is what is set in the method parameter and the digit
                // group has a leading space
                digitGroupLength = desiredDigitGroupLength;
                string spaceBeforeDigitGroup = " ";

                // Which digit group is left shorter is decided in a method parameter. Length of the shorter digit
                // group is defined with the help of the modulo operator
                if (shorterGroupInBeginning)
                {
                    if (i == 0)
                        if (inputNumber.Length % desiredDigitGroupLength != 0)
                            digitGroupLength = inputNumber.Length % desiredDigitGroupLength;
                }
                else
                {
                    if (i + digitGroupLength >= inputNumber.Length)
                        if (inputNumber.Length % desiredDigitGroupLength != 0)
                            digitGroupLength = inputNumber.Length % desiredDigitGroupLength;
                }

                // The very first digit group doesn't have a leading space
                if (i == 0)
                    spaceBeforeDigitGroup = "";

                // Add the current digit group to the string to be returned
                inputNumberGrouped = inputNumberGrouped + spaceBeforeDigitGroup + inputNumber.Substring(i, digitGroupLength);
            }

            return inputNumberGrouped;
        }

        // This method takes in one parameter: a country code. It converts the code characters into digits accordingly to the IBAN number documentation
        public static string CalculateCountryCodeDigits(string countryCode)
        {
            // Convert the ascii codes of the country code characters to their corresponding codes in the IBAN number
            // documentation (A = 10, B = 11... Z = 35)
            return (countryCode[0] - 55).ToString() + (countryCode[1] - 55).ToString();
        }
    }
}
