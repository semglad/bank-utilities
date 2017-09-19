using System;
using Ekoodi.Utilities.Test;

namespace bban_validator
{
    class Program
    {
        static void Main(string[] args)
        {
            BBAN bbanNumber = new BBAN();
            bbanNumber.SetBBANNumber();
            bbanNumber.CheckBBANValidity();
        }
    }
}
