using System;
using System.Numerics;

namespace Ekoodi.Utilities.Finance
{
    public static class PaymentReference
    {
        public static int CalculateDomesticCheckDigit(string baseForReferenceNumber)
        {
            // The check digit is calculated using multipliers 7, 3 and 1; store these values in an array.
            int[] multiplier = new int[3] { 7, 3, 1 };

            // Declare a variable to hold the calculated check digit and another to keep count of which
            // multiplier should be used in each iteration
            double calculatedCheckDigit = 0;
            int counter = 0;

            // Loop through each digit of the input reference number
            for (int i = 0; i < baseForReferenceNumber.Length; i++)
            {
                // Multiply the current reference number digit with the corresponding multiplier
                calculatedCheckDigit = calculatedCheckDigit + int.Parse(baseForReferenceNumber.Substring(baseForReferenceNumber.Length - i - 1, 1)) * multiplier[counter];

                // Move counter to the next position
                if (counter == 2)
                    counter = 0;
                else
                    counter++;
            }

            // Substract the calculation result from the next ten to get the actual check digit
            calculatedCheckDigit = Math.Ceiling(calculatedCheckDigit / 10) * 10 - calculatedCheckDigit;

            return int.Parse(calculatedCheckDigit.ToString());
        }

        public static bool ValidateReferenceNumber(string referenceNumber, bool isInternational)
        {
            // If the check digit in the input reference number corresponds to the calculated check digit, return true
            if (isInternational)
                return referenceNumber.Substring(2, 2) == CalculateInternationalCheckDigits(referenceNumber);
            else
                return int.Parse(referenceNumber.Substring(referenceNumber.Length - 1, 1)) == CalculateDomesticCheckDigit(referenceNumber.Substring(0, referenceNumber.Length - 1));
        }

        public static string ConvertReferenceNumberFromDomesticToInternational(string domesticReferenceNumber)
        {
            return "RF" + CalculateInternationalCheckDigits("RF00" + domesticReferenceNumber) + domesticReferenceNumber;
        }

        public static string CalculateInternationalCheckDigits(string internationalReferenceNumber)
        {
            internationalReferenceNumber = internationalReferenceNumber.Remove(0, 4) + "271500";

            BigInteger internationalCheckDigits = 98 - (BigInteger.Parse(internationalReferenceNumber) % 97);

            if (internationalCheckDigits < 10)
            {
                return "0" + internationalCheckDigits.ToString();
            }
            else
            {
                return internationalCheckDigits.ToString();
            }
        }

        public static bool ValidateIBANNumber(string ibanNumber)
        {
            return BigInteger.Parse(ibanNumber.Remove(0, 4) + (ibanNumber[0] - 55).ToString() + (ibanNumber[1] - 55).ToString() + ibanNumber.Substring(2, 2)) % 97 == 1;
        }
    }
}
