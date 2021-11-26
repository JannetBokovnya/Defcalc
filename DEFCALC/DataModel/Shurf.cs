using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class Shurf
    {
        public string KeyShurf { get; set; }
        public string NumberAkt { get; set; }
        public string HarakterRel { get; set; }

        public Shurf(string keyShurf, string numberAkt, string harakterRel)
        {
            KeyShurf = keyShurf;
            NumberAkt = numberAkt;
            HarakterRel = harakterRel;
        }

    }
}
