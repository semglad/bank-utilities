using System;
using System.Numerics;

namespace Ekoodi.Utilities.Test
{
    public class AccountNumber
    {
        private string BBANNumber;
        private string IBANNumber;
        private string BICCode;

        public void ShowAccountNumberData()
        {
            Console.Clear();
            Console.WriteLine("Tilinumero konekielisessa muodossa: {0}", BBANNumber);
            Console.WriteLine("Pankkitilin numero IBAN muodossa: {0} {1} {2} {3} {4}", IBANNumber.Substring(0, 4), IBANNumber.Substring(4, 4), IBANNumber.Substring(8, 4), IBANNumber.Substring(12, 4), IBANNumber.Substring(16, 2));
            Console.WriteLine("BIC-koodi: {0}", BICCode);

            // Wait for the user to press any key to exit the program
            Console.WriteLine("\nPaina mita tahansa painiketta jatkaaksesi\n");
            Console.ReadKey();
        }

        public void SetBBANNumber()
        {
            // Prompt user for input
            Console.Clear();
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

            // Introduce a variable for handling different pad starting positions
            int characterPositionToPadFrom = 0;

            // For account numbers within banks 4 and 5, set the pad starting position to 7. For numbers within other banks, set it to 6
            if (inputBBANNumber.Substring(0, 1) == "4" || inputBBANNumber.Substring(0, 1) == "5")
                characterPositionToPadFrom = 7;
            else
                characterPositionToPadFrom = 6;

            // Pad the number with 0's until the length of the number is 14
            while (inputBBANNumber.Length < 14)
            {
                inputBBANNumber = inputBBANNumber.Insert(characterPositionToPadFrom, "0");
            }

            // Assign the verified and trimmed account number to the class field BBANNumber
            BBANNumber = inputBBANNumber;
        }

        public bool CheckBBANValidity ()
        {
            // Introduce variables for a check digit
            double checkDigit = 0;

            // Go through the number digit by digit
            for (int i = 0; i < BBANNumber.Length-1; i++)
            {
                int BBANDigit = int.Parse(BBANNumber.Substring(i, 1));

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
            checkDigit = Math.Ceiling(checkDigit / 10) * 10 - checkDigit;

            // Check if the calculated check digit equals the input check digit (last digit of the account number). Inform the user accordingly.
            if (checkDigit == int.Parse(BBANNumber.Substring(BBANNumber.Length - 1, 1)))
            {
                Console.WriteLine("\nTilinumerosi on konekielisessa muodossa {0}. Tarkistemerkki oikein!", BBANNumber);
                // Wait for the user to press any key to exit the program
                Console.WriteLine("\nPaina mita tahansa painiketta jatkaaksesi\n");
                Console.ReadKey();
                return true;
            }
            else
            {
                Console.WriteLine("\nTilinumerosi on konekielisessa muodossa {0}. Tarkistemerkki vaarin! Tarkista tilinumero.", BBANNumber);
                // Wait for the user to press any key to exit the program
                Console.WriteLine("\nPaina mita tahansa painiketta jatkaaksesi\n");
                Console.ReadKey();
                return false;
            }
        }

        public void ConvertBBANToIBAN(string countryCode = "FI")
        {
            // Convert the ascii codes of the country code characters to their corresponding codes in the IBAN number documentation (A = 10, B = 11... Z = 35). Then concatenate
            // the input BBAN number and the result
            string countryCodeDigits = (countryCode[0] - 55).ToString() + (countryCode[1] - 55).ToString();
            string BBANNumberWithCountryCodeDigits = BBANNumber + countryCodeDigits;

            // Introduce a variable to hold the BBAN number with check digits
            string BBANNumberWithCheckDigits = "";

            // Test for every number between 00 and 99 as the check digits. When the correct combination is found, concatenate the BBAN number and the check digits and break the loop.
            for (int i = 0; i < 100; i++)
            {
                if (BigInteger.Parse(BBANNumberWithCountryCodeDigits + i.ToString()) % 97 == 1)
                {
                    BBANNumberWithCheckDigits = i.ToString() + BBANNumber;
                    break;
                }
            }

            // Concatenate the original country code and the input BBAN number (with check digits) to form the IBAN number and assign the result to the class field IBANNumber
            IBANNumber = countryCode + BBANNumberWithCheckDigits;

            // Print out the IBAN account number
            Console.WriteLine("Pankkitilisi numero IBAN muodossa on {0} {1} {2} {3} {4}", IBANNumber.Substring(0,4), IBANNumber.Substring(4, 4), IBANNumber.Substring(8, 4), IBANNumber.Substring(12, 4), IBANNumber.Substring(16, 2));

            // Wait for the user to press any key to exit the program
            Console.WriteLine("\nPaina mita tahansa painiketta jatkaaksesi");
            Console.ReadKey();
        }

        public void ShowBICCode()
        {
            int bankCodeLength;

            // Figure out the bank code length. Account numbers starting with 3 have the bank code length of 2 digits. Numbers starting with 4 or 7 have 3 digits. All other have 1.
            switch (BBANNumber.Substring(0,1))
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
            string bankCode = BBANNumber.Substring(0, bankCodeLength);

            // Figure out the BIC code. Documentation for Finnish BIC codes can be found at:
            // http://www.finanssiala.fi/maksujenvalitys/dokumentit/Suomalaiset_rahalaitostunnukset_ja_BIC-koodit.pdf
            switch (bankCode)
            {
                case "405":
                    BICCode = "HELSFIHH";
                    break;

                case "497":
                    BICCode = "HELSFIHH";
                    break;

                case "717":
                    BICCode = "BIGKFIH1";
                    break;

                case "713":
                    BICCode = "CITIFIHX";
                    break;

                case "8":
                    BICCode = "DABAFIHH";
                    break;

                case "34":
                    BICCode = "DABAFIHX";
                    break;

                case "37":
                    BICCode = "DNBAFIHX";
                    break;

                case "31":
                    BICCode = "HANDFIHH";
                    break;

                case "799":
                    BICCode = "HOLVFIHH";
                    break;

                case "1":
                    BICCode = "NDEAFIHH";
                    break;

                case "2":
                    BICCode = "NDEAFIHH";
                    break;

                case "5":
                    BICCode = "OKOYFIHH";
                    break;

                case "33":
                    BICCode = "ESSEFIHX";
                    break;

                case "39":
                    BICCode = "SBANFIHH";
                    break;

                case "36":
                    BICCode = "SBANFIHH";
                    break;

                case "38":
                    BICCode = "SWEDFIHH";
                    break;

                case "6":
                    BICCode = "AABAFI22";
                    break;
            }

            if (bankCode.Length == 3 && BICCode == "")
            {
                if (bankCode.Substring(0, 2) == "47")
                    BICCode = "POPFFI22";
                else
                    BICCode = "ITELFIHH";
            }

            // Print out the BIC code
            Console.WriteLine("\nBIC-koodi: {0}", BICCode);

            // Wait for the user to press any key to exit the program
            Console.WriteLine("\nPaina mita tahansa painiketta jatkaaksesi");
            Console.ReadKey();
        }
    }
}
