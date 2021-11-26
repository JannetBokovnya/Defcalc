using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class Dict
    {
       public string Name { get; set; }

        public string Key { get; set; }

        public Dict(string key, string name)
        {
            Name = name;
            Key = key;
        }
    }
}
