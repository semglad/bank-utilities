using System;
using Ekoodi.Utilities.Test;

namespace bban_validator
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountNumber bbanNumber = new AccountNumber();
            bbanNumber.SetBBANNumber();

            while (!bbanNumber.CheckBBANValidity())
            {
                bbanNumber.SetBBANNumber();
            }

            bbanNumber.ConvertBBANToIBAN("FI");
            bbanNumber.ShowBICCode();
            bbanNumber.ShowAccountNumberData();
        }
    }
}
