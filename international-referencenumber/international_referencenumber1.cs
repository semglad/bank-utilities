using System;
using Ekoodi.Utilities.Finance;

namespace international_referencenumber
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt for an international reference number
            Console.WriteLine("Anna kansainvalinen viitenumero:");
            string referenceNumber = Console.ReadLine().Replace(" ", "");
            referenceNumber = referenceNumber.TrimStart(Convert.ToChar("0"));

            // If the input is not numerical except for the first two digits or doesn't have the proper check digits, prompt again
            while (!NumberUtility.CheckNumberLength(referenceNumber.Remove(0, 2)) || !PaymentReference.ValidateReferenceNumber(referenceNumber, true))
            {
                Console.WriteLine("Viitenumero virheellinen! Tarkista viitenumero.");
                Console.WriteLine("Anna viitenumero:");
                referenceNumber = Console.ReadLine().Replace(" ", "");
                referenceNumber = referenceNumber.TrimStart(Convert.ToChar("0"));
            }

            Console.WriteLine("Viitenumero {0} on OK", NumberUtility.GroupDigits(referenceNumber, 4, false));
            
            Console.WriteLine("Paina mita tahansa nappainta jatkaaksesi...");
            Console.ReadKey();
        }
    }
}
