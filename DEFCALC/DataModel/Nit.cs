using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class Nit
    {
       public string NameNit { get; set; }

        public string KeyNit { get; set; }

        public Nit(string keyNit, string nameNit)
        {
            NameNit = nameNit;
            KeyNit = keyNit;
        }
    }
}
