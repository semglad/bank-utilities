using System;

namespace Ekoodi.Utilities.Test
{
    public static class TestUtility
    {
        public static void PrintGreetings()
        {
            Console.WriteLine("Hello Ekoodi!");
        }
    }

    public class BBAN
    {
        private string OriginalBBANNumber;

        public void SetBBANNumber()
        {
            // Prompt user for input
            Console.WriteLine("Kirjoita tilinumero:");

            // Store user input to a variable
            string userInput = Console.ReadLine();

            // Account number can be input with or without hyphen but the utility handles the account number without it. Therefore all hyphens are removed.
            string inputBBANNumber = userInput.Replace("-", "");

            // Check the input for length (min length 7) and validity (no alphabetical characters). If not valid, prompt for a valid number.
            while (!ulong.TryParse(inputBBANNumber, out ulong success) || inputBBANNumber.Length < 7 || inputBBANNumber.Length > 14)
            {
                Console.Clear();
                Console.WriteLine("Virheellinen tilinumero!\nTilinumeron pitaa olla 7-14 merkkia pitka eika se saa sisaltaa muita merkkeja kuin numeroita ja valiviivan.\nKirjoita tilinumero:");

                inputBBANNumber = Console.ReadLine();

                // Account number can be input with or without hyphen but the utility handles the account number without it. Therefore all hyphens are removed.
                inputBBANNumber = inputBBANNumber.Replace("-", "");
            }

            // Assign the verified and trimmed account number to the class field OriginalBBANNumber
            OriginalBBANNumber = inputBBANNumber;

        }

        public void CheckBBANValidity ()
        {
            // Introduce variables for a check digit and handling different pad starting positions
            int characterPositionToPadFrom = 0;
            int checkDigit = 0;

            // For account numbers within banks 4 and 5, set the pad starting position to 7. For numbers within other banks, set it to 6
            if (OriginalBBANNumber.Substring(0, 1) == "4" || OriginalBBANNumber.Substring(0, 1) == "5")
                characterPositionToPadFrom = 7;
            else
                characterPositionToPadFrom = 6;

            // Assign the original BBAN number to a string variable that can be manipulated
            string convertedBBANNumber = OriginalBBANNumber;

            // Pad the number with 0's until the length of the number is 14
            while (convertedBBANNumber.Length < 14)
            {
                convertedBBANNumber = convertedBBANNumber.Insert(characterPositionToPadFrom, "0");
            }

            // Go through the number digit by digit
            for (int i = 0; i < convertedBBANNumber.Length-1; i++)
            {
                int BBANDigit = int.Parse(convertedBBANNumber.Substring(i, 1));

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

            // If the check digit consists of two digits, multiply the first digit by 10 and substract the check digit from this result
            if (checkDigit < 100)
                checkDigit = (int.Parse(checkDigit.ToString().Substring(0, 1))+1) * 10 - checkDigit;
            else // If the check digit consists of three digits multiply the first two digits by 10 and substract the check digit from this result
                checkDigit = (int.Parse(checkDigit.ToString().Substring(0, 2))+1) * 10 - checkDigit;

            // Check if the calculated check digit equals the input check digit (last digit of the account number). Inform the user accordingly.
            if (checkDigit == int.Parse(OriginalBBANNumber.Substring(OriginalBBANNumber.Length - 1, 1)))
                Console.WriteLine("\nTilinumerosi on konekielisessa muodossa {0}. Tarkistemerkki oikein!", convertedBBANNumber);
            else
                Console.WriteLine("\nTilinumerosi on konekielisessa muodossa {0}. Tarkistemerkki vaarin! Tarkista tilinumero.", convertedBBANNumber);

            // Wait for the user to press any key to exit the program
            Console.WriteLine("\nPaina mita tahansa painiketta jatkaaksesi");
            Console.ReadKey();
        }
    }
}
