using System;
using System.Collections.Generic;
using System.Text;

namespace Ekoodi.Utilities.Finance
{
    public class BarcodeUtility
    {
        // This method takes in four parameters: IBAN account number, invoice total sum, reference number (in Finnish domestic or in international form)
        // and a due date. It forms a virtual barcode that can be input into bank applications.
        public static string CreateBarcode(string ibanNumber, double invoiceSum, string referenceNumber, DateTime dueDate)
        {
            // Add IBAN account number and invoice sum to the barcode
            string barcode = ibanNumber.Remove(0, 2) + NumberUtility.AddPadding(Math.Round(invoiceSum, 2).ToString("F2").Replace(",", ""), 0, 8);

            // Add reference number to the barcode
            if (referenceNumber.Substring(0, 2) == "RF")
                barcode = "5" + barcode + NumberUtility.AddPadding(referenceNumber.Remove(0, 2), 2, 23);
            else
                barcode = "4" + barcode + "000" + NumberUtility.AddPadding(referenceNumber, 0, 20);

            // Add due date to the barcode
            barcode = barcode + dueDate.ToString("yyyyMMdd").Remove(0, 2);

            return barcode;
        }
    }
}
