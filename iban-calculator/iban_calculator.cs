using System;
using Ekoodi.Utilities.Finance;

namespace iban_calculator
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

            while (!NumberUtility.CheckNumberLength(userInput, 7, 14))
            {
                Console.Clear();
                Console.WriteLine("Virheellinen tilinumero!\nTilinumeron pitaa olla 7-14 merkkia pitka eika se saa sisaltaa muita merkkeja kuin numeroita ja valiviivan.");
                Console.WriteLine("Kirjoita tilinumero:");
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
            string bbanNumber = NumberUtility.AddPadding(userInput, characterPositionToPadFrom, 14);

            // Check the input for length (min length 7) and validity (no alphabetical characters). If not valid, prompt for a valid number.
            while (!NumberUtility.CheckNumberLength(bbanNumber, 7, 14) || !AccountNumber.ValidateBBANNumber(bbanNumber))
            {
                Console.Clear();

                if (!NumberUtility.CheckNumberLength(userInput, 7, 14))
                {
                    Console.WriteLine("Virheellinen tilinumero!\nTilinumeron pitaa olla 7-14 merkkia pitka eika se saa sisaltaa muita merkkeja kuin numeroita ja valiviivan.");

                } else
                {
                    Console.WriteLine("Tilinumeron tarkiste vaarin. Tarkista tilinumero!");
                }

                Console.WriteLine("Kirjoita tilinumero:");
                userInput = Console.ReadLine().Replace("-", "");

                // Introduce a variable for handling different pad starting positions
                characterPositionToPadFrom = 0;

                // For account numbers within banks 4 and 5, set the pad starting position to 7. For numbers within other banks, set it to 6
                if (userInput.Substring(0, 1) == "4" || userInput.Substring(0, 1) == "5")
                    characterPositionToPadFrom = 7;
                else
                    characterPositionToPadFrom = 6;

                // Pad the number with 0's until the length of the number is 14
                bbanNumber = NumberUtility.AddPadding(userInput, characterPositionToPadFrom, 14);
            }

            Console.WriteLine("Tilinumerosi IBAN-paperimuodossa on {0}", NumberUtility.GroupDigits(AccountNumber.ConvertBBANToIBAN(bbanNumber), 4, false));
            Console.WriteLine("Paina mita tahansa nappainta jatkaaksesi...");
            Console.ReadKey();

            Console.WriteLine("Anna tilinumero IBAN-muodossa (valilyonnein tai ilman):");
            userInput = Console.ReadLine().Replace(" ", "");

            while(!NumberUtility.CheckNumberLength(userInput.Substring(2, userInput.Length - 2)))
            {
                Console.WriteLine("Virheellinen tilinumero!\nTilinumeron ilman valilyonteja pitaa olla 14 merkkia pitka eika se saa sisaltaa kahta ensimmaista merkkia lukuunottamatta muita merkkeja kuin numeroita.");
                Console.WriteLine("Anna tilinumero IBAN-muodossa (valilyonnein tai ilman):");
                userInput = Console.ReadLine().Replace(" ", "");
            }

            if (AccountNumber.ValidateIBANNumber(userInput))
            {
                Console.WriteLine("Tilinumero {0} OK!", NumberUtility.GroupDigits(userInput, 4, false));
                Console.WriteLine("BIC-koodi: {0}", AccountNumber.GetBICCode(AccountNumber.ConvertIBANtoBBAN(userInput)));
            } else
            {
                Console.WriteLine("Tilinumero {0} on vaarin. Tarkista numero!", NumberUtility.GroupDigits(userInput, 4, false));
            }

            Console.WriteLine("Paina mita tahansa nappainta jatkaaksesi...");
            Console.ReadKey();
        }
    }
}
