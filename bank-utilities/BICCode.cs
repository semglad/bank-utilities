using System;
using System.Collections.Generic;
using System.Text;

namespace Ekoodi.Utilities.Finance
{
    class BicCode
    {
        public string ID { get; set; }
        public string Code { get; set; }

        public BicCode (string id, string name)
        {
            ID = id;
            Code = name;
        }
    }
}
