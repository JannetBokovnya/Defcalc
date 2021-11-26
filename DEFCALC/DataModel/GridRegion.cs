using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class GridRegion
    {
       public string KEYREGION { get; set; } //ключ участка
       public string NAMEREGION { get; set; } //название участка
       public string BEGINKM { get; set; }//начальный км
       public string ENDKM { get; set; }//конечный км
       public string LENGHT { get; set; }//длина

       public GridRegion (string keyRegion, string nameRegion, string beginKm, string endKm, string lenght)
       {
           KEYREGION = keyRegion;
           NAMEREGION = nameRegion;
           BEGINKM = beginKm;
           ENDKM = endKm;
           LENGHT = lenght;

       }

       
    }
}
