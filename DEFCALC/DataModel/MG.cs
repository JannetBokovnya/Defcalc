using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class MG
    {
        public string NameMG { get; set; }

        public string KeyMG { get; set; }

        public MG(string nameMG, string keyMG)
        {
            NameMG = nameMG;
            KeyMG = keyMG;
        }
    }
}
