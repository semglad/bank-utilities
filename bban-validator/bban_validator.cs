using System;
using Ekoodi.Utilities.Finance;
using System.Globalization;

namespace bban_validator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt user for input
            Console.Clear();
            Console.WriteLine("Kirjoita tilinumero:");

            // Store user input to a variable. Account number can be input with or without hyphen but the utility handles the account number
            // without it. Therefore all hyphens are removed.
            string userInput = Console.ReadLine().Replace("-", "");

            // Check the input for length (min length 7) and validity (no alphabetical characters). If not valid, prompt for a valid number.
            while (!NumberUtility.CheckNumberLength(userInput.Replace("-", ""), 7, 14))
            {
                Console.Clear();
                Console.WriteLine("Virheellinen tilinumero!\nTilinumeron pitaa olla 7-14 merkkia pitka eika se saa sisaltaa muita merkkeja kuin numeroita ja valiviivan.\nKirjoita tilinumero:");

                userInput = Console.ReadLine().Replace("-", "");
            }

            // Introduce a variable for handling different pad starting positions
            int characterPositionToPadFrom = 0;

            // For account numbers within banks 4 and 5, set the pad starting position to 7. For numbers within other banks, set it to 6
            if (userInput.Substring(0, 1) == "4" || userInput.Substring(0, 1) == "5")
                characterPositionToPadFrom = 7;
            else
                characterPositionToPadFrom = 6;

            // Pad the number with 0's until the length of the number is 14
            string bbanNumber = NumberUtility.AddZeroPadding(userInput, characterPositionToPadFrom, 14);

            if (AccountNumber.ValidateBBANNumber(bbanNumber))
            {
                Console.WriteLine("Tilinumerosi konekielisessa BBAN-muodossa: {0}", bbanNumber);
                Console.WriteLine("Tilinumerosi konekielisessa IBAN-muodossa: {0}", AccountNumber.ConvertBBANToIBAN(bbanNumber));
            } else
            {
                Console.WriteLine("Tilinumeron tarkiste vaarin. Tarkista tilinumero!");
            }

            Console.ReadKey();
        }
    }
}
