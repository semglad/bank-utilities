using System;
using System.Numerics;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ekoodi.Utilities.Finance
{
    public static class AccountNumber
    {
        public static bool ValidateBBANNumber (string bbanNumber)
        {
            // Check if the calculated check digit equals the input check digit (last digit of the account number).
            return (CalculateBBANCheckDigit(bbanNumber) == bbanNumber.Substring(bbanNumber.Length - 1, 1));
        }

        public static bool ValidateIBANNumber (string ibanNumber)
        {
            // Check if the calculated check digit equals the input check digit (digits 3 and 4 of the account number).
            return (CalculateIBANCheckDigit(ibanNumber.Remove(0, 4)) == ibanNumber.Substring(2, 2));
        }

        public static string CalculateBBANCheckDigit (string bbanNumber)
        {
            // Introduce variables for a check digit
            double checkDigit = 0;

            // Go through the number digit by digit
            for (int i = 0; i < bbanNumber.Length - 1; i++)
            {
                int BBANDigit = int.Parse(bbanNumber.Substring(i, 1));

                // If the position of the digit is even and the digit multiplied by two consists of a single digit, multiply the digit by two and add the result to the check digit    
                if (i % 2 == 0)
                    if (BBANDigit * 2 < 9)
                        checkDigit = checkDigit + BBANDigit * 2;
                    else // If the position of the digit is even and the digit multiplied by two consists of two digits, multiply the digit by two and add both result digits
                         // to the check digit
                        checkDigit = checkDigit + int.Parse((BBANDigit * 2).ToString().Substring(0, 1)) + int.Parse((BBANDigit * 2).ToString().Substring(1, 1));
                else // If the position of the digit is odd, add the digit to the check digit
                    checkDigit = checkDigit + BBANDigit;
            }

            // Substract the check digit from the next ten
            return (Math.Ceiling(checkDigit / 10) * 10 - checkDigit).ToString();
        }

        public static string CalculateIBANCheckDigit (string bbanNumber)
        {
            string bbanNumberWithCountryCodeDigits = bbanNumber + NumberUtility.CalculateCountryCodeDigits("FI") + "00";

            string checkDigit = NumberUtility.AddPadding((98 - BigInteger.Parse(bbanNumberWithCountryCodeDigits) % 97).ToString(), 0, 2);
            
            return checkDigit;
        }

        public static string ConvertBBANToIBAN(string bbanNumber, string countryCode = "FI")
        {
            // Concatenate the original country code, check digits and the input BBAN number to form the IBAN number
            return countryCode + CalculateIBANCheckDigit(bbanNumber) + bbanNumber;
        }

        public static string ConvertIBANtoBBAN (string ibanNumber)
        {
            return ibanNumber.Remove(0, 4);
        }

        public static string GetBICCode(string bbanNumber)
        {
            int bankCodeLength;

            // Figure out the bank code length. Account numbers starting with 3 have the bank code length of 2 digits. Numbers starting with 4 or 7 have 3 digits. All other have 1.
            switch (bbanNumber.Substring(0, 1))
            {
                case "3":
                    bankCodeLength = 2;
                    break;

                case "4":
                    bankCodeLength = 3;
                    break;

                case "7":
                    bankCodeLength = 3;
                    break;

                default:
                    bankCodeLength = 1;
                    break;
            }

            // Extract the bank code from the account number
            string bankCode = bbanNumber.Substring(0, bankCodeLength);

            // Extract BIC codes from a file to a list object, find a match for the bank code and return the corresponding BIC
            List<BicCode> bicCodes = new List<BicCode>();
            using (var fs = new FileStream("biclist.json", FileMode.Open))
            using (var sr = new System.IO.StreamReader(fs))
            {
                string json = sr.ReadToEnd();
                bicCodes = JsonConvert.DeserializeObject<List<BicCode>>(json);
            }
            Console.WriteLine(bankCode);
            string bicCode = bicCodes.Find(c => c.ID == bankCode).Code;

            Console.WriteLine(bicCodes[0].ID);

            if (bicCode == null)
                Console.WriteLine("VIRHE!");

            return bicCode;
        }
    }
}
