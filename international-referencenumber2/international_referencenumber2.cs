using System;
using Ekoodi.Utilities.Finance;

namespace international_referencenumber2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt for a domestic reference number
            Console.WriteLine("Anna viitenumero:");
            string referenceNumber = Console.ReadLine().Replace(" ", "");
            referenceNumber = referenceNumber.TrimStart(Convert.ToChar("0"));

            // If the input is not numerical or is not 4 to 20 digits long, prompt again
            while (!NumberUtility.CheckNumberLength(referenceNumber, 4, 20) || !PaymentReference.ValidateReferenceNumber(referenceNumber, false))
            {
                Console.WriteLine("Viitenumero virheellinen! Viitenumeron tulee olla 4-20 merkkia pitka ja se saa sisaltaa vain numeroita. Myös tarkistenumeron tulee olla oikein.");
                Console.WriteLine("Anna viitenumero:");
                referenceNumber = Console.ReadLine().Replace(" ", "");
                referenceNumber = referenceNumber.TrimStart(Convert.ToChar("0"));
            }

            Console.WriteLine("Viitenumero {0} on OK", NumberUtility.GroupDigits(referenceNumber, 5, true));

            Console.WriteLine("Se on kansainvalisessa muodossa {0}", NumberUtility.GroupDigits(PaymentReference.ConvertReferenceNumberFromDomesticToInternational(referenceNumber), 4, false));

            Console.WriteLine("Paina mita tahansa nappainta jatkaaksesi...");
            Console.ReadKey();
        }
    }
}
