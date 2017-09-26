using System;
using Ekoodi.Utilities.Finance;

namespace finnish_referencenumber2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create multiple domestic reference numbers
            Console.WriteLine("Anna viitenumeron perusosa (esim. laskunro):");
            string baseForReferenceNumber = Console.ReadLine().Replace(" ", "");
            while (!NumberUtility.CheckNumberLength(baseForReferenceNumber, 3, 19))
            {
                Console.WriteLine("Viitenumeron perusosa virheellinen. Numeron pitää olla 3-19 merkkiä pitka ja se saa sisaltaa vain numeroita.");
                Console.WriteLine("Anna viitenumeron perusosa (esim. laskunro):");
                baseForReferenceNumber = Console.ReadLine().Replace(" ", "");
            }

            baseForReferenceNumber = baseForReferenceNumber.TrimStart(Convert.ToChar("0"));

            int numberOfReferenceNumbers = 0;

            Console.Write("Kuinka monta viitenumeroa luodaan: ");
            string userInput = Console.ReadLine();
            while (!int.TryParse(userInput, out numberOfReferenceNumbers))
            {
                Console.Write("Maara virheellinen. Anna numeraalinen arvo: ");
                userInput = Console.ReadLine();
            }

            for (int i = 1; i <= numberOfReferenceNumbers; i++)
            {
                Console.WriteLine(NumberUtility.GroupDigits(baseForReferenceNumber + i + PaymentReference.CalculateDomesticCheckDigit(baseForReferenceNumber + i), 5, true));
            }

            Console.WriteLine("Paina mita tahansa nappainta jatkaaksesi...");
            Console.ReadKey();
        }
    }
}
