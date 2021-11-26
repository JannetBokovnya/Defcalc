using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DEFCALC.DataModel
{
   public class CalculateDefectRSTRENG
    {
        public string KeyDefect { get; set; }//ключ дефекта
           public string DopDefect { get; set; } //допстимость дефекта
           public string DestroyPressureRestreng { get; set; }//разрушающее давление Restreng
           public string CoefZapRestreng { get; set; }//коэффициент запаса Restreng
           public string RecomResponsRestreng { get; set; }//рекомендации по реагированию Restreng
           public Brush ColorRestreng { get; set; } // цвет заливки дефекта по ASME


           public CalculateDefectRSTRENG(string keyDefect, string dopDefect, 
                                  string destroyPressureRestreng, string coefZapRestreng, string recomResponsRestreng,
                                  Brush colorRestreng)
           {
               KeyDefect = keyDefect;
               DopDefect = dopDefect;
               DestroyPressureRestreng = destroyPressureRestreng;
               CoefZapRestreng = coefZapRestreng;
               RecomResponsRestreng = recomResponsRestreng;
               ColorRestreng = colorRestreng;

           }
    }
}
