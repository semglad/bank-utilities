using System;
using Ekoodi.Utilities.Finance;
using System.Globalization;

namespace bank_barcode
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt for an IBAN account number
            Console.WriteLine("Kirjoita laskuttajan tilinumero IBAN-muodossa:");
            string userInput = Console.ReadLine().Replace(" ", "");
            while (!NumberUtility.CheckNumberLength(userInput.Remove(0, 2), 16, 16) || !PaymentReference.ValidateIBANNumber(userInput))
            {
                Console.WriteLine("Tilinumero virheellinen. Tarkista tilinumero.");
                Console.WriteLine("Kirjoita laskuttajan tilinumero IBAN-muodossa:");
                userInput = Console.ReadLine().Replace(" ", "");
            }

            string ibanNumber = userInput;

            // Prompt for total invoice sum
            Console.Write("Kirjoita laskun loppusumma: ");
            userInput = Console.ReadLine();

            while ((!NumberUtility.CheckNumberLength(userInput, 1, 999, "double") || (double.Parse(userInput).ToString("F0").Length > 6)))
            {
                Console.WriteLine("Laskun summa virheellinen. Kayta pilkkua erottimena. Summa saa olla korkeintaan 999999.");
                Console.Write("Kirjoita laskun loppusumma:");
                userInput = Console.ReadLine();
            }

            double invoiceSum = double.Parse(userInput);

            // Ask if the reference number is a domestic one or international. This information will be used later to determine which validation method to use
            Console.Write("Onko laskulla kansainvalinen RF-viite (kotimaisen viitteen sijaan)? (k/e): ");
            var userKeyInput = Console.ReadKey();
            while (userKeyInput.Key != ConsoleKey.K && userKeyInput.Key != ConsoleKey.E)
            {
                Console.WriteLine("\nVirheellinen vastaus. Kirjoita vastaukseksi k tai e.");
                Console.Write("Onko laskulla kansainvalinen RF-viite (kotimaisen viitteen sijaan)? (k/e): ");
                userKeyInput = Console.ReadKey();
            }

            bool isInternationalReferenceNumber = (userKeyInput.Key == ConsoleKey.K);

            // Prompt for a reference number
            Console.WriteLine("\nKirjoita laskuttajan viite:");
            userInput = Console.ReadLine().Replace(" ", "");
            if (isInternationalReferenceNumber)
            {
                while (!NumberUtility.CheckNumberLength(userInput.Remove(0, 2)) || !PaymentReference.ValidateReferenceNumber(userInput, true))
                {
                    Console.WriteLine("Viite virheellinen. Tarkista viite.");
                    Console.WriteLine("Kirjoita laskuttajan viite:");
                    userInput = Console.ReadLine().Replace(" ", "");
                }
            }
            else
            {
                while (!NumberUtility.CheckNumberLength(userInput) || !PaymentReference.ValidateReferenceNumber(userInput, false))
                {
                    Console.WriteLine("Viite virheellinen. Tarkista viite.");
                    Console.WriteLine("Kirjoita laskuttajan viite:");
                    userInput = Console.ReadLine().Replace(" ", "");
                }
            }

            string referenceNumber = userInput.TrimStart(Convert.ToChar("0"));

            // Prompt for a due date
            DateTime dueDate;

            Console.WriteLine("Kirjoita laskun erapaiva (muodossa 06.08.2017):");
            userInput = Console.ReadLine();
            while (!DateTime.TryParseExact(userInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate))
            {
                Console.WriteLine("Erapaiva virheellinen. Tarkista erapaiva.");
                Console.WriteLine("Kirjoita laskun erapaiva (muodossa 06.08.2017):");
                userInput = Console.ReadLine();
            }

            Console.WriteLine("Virtuaaliviivakoodisi:\n{0}\n", BarcodeUtility.CreateBarcode(ibanNumber, invoiceSum, referenceNumber, dueDate));

            Console.WriteLine("Paina mita tahansa nappainta jatkaaksesi...");
            Console.ReadKey();
        }
    }
}
