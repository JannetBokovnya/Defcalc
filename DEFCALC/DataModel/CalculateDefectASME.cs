using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DEFCALC.DataModel
{
   public class CalculateDefectASME
    {

        public string KeyDefect { get; set; }//ключ дефекта
           public string DopDefect { get; set; } //допктимость дефекта
           public string MaxPressureDopASME { get; set; }//макс допустимое давление ASME
           public string DestroyPressureASME { get; set; }//разрушающее давление ASME
           public string MaxDopLenghtDefASME { get; set; }//макс допустимая длина дефекта ASME
           public string MaxDopDepthASME { get; set; }//макс допустимая глубина деф
           public string RecomResponsASME { get; set; }//рекомендация по реагированию ASME
           public Brush ColorASME { get; set; } // цвет заливки дефекта по ASME


           public CalculateDefectASME(string keyDefect, string dopDefect, string maxPressureDopASME, string destroyPressureASME,
                                  string maxDopLenghtDefASME, string maxDopDepthASME, string recomResponsASME,
                                 Brush colorASME)
           {
               KeyDefect = keyDefect;
               DopDefect = dopDefect;
               MaxPressureDopASME = maxPressureDopASME;
               DestroyPressureASME = destroyPressureASME;
               MaxDopLenghtDefASME = maxDopLenghtDefASME;
               MaxDopDepthASME = maxDopDepthASME;
               RecomResponsASME = recomResponsASME;
               ColorASME = colorASME;

           }
    }
}
