using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DEFCALC.DataModel
{
  public  class CalculateDefectDNV
    {
         public string KeyDefect { get; set; }//ключ дефекта
           public string DopDefect { get; set; } //допкстимость дефекта
           public string DestroyPressureDNV { get; set; }//разрушающее давление по DNV
           public string DezPressureDNV { get; set; }//безопасное давление DNV
           public string KoefDezPressureDNV { get; set; }//коэффициент безопасного давления
           public Brush ColorDNV { get; set; } // цвет заливки дефекта по ASME
           public string RecomResponsDNV{ get; set; }//рекомендация по реагированию DNV !!!! добавить в базу!!!!
           


           public CalculateDefectDNV(string keyDefect, string dopDefect, string destroyPressureDNV, 
                                     string dezPressureDNV, string koefDezPressureDNV,
                                     Brush colorDNV, string recomResponsDNV)
           {
               KeyDefect = keyDefect;
               DopDefect = dopDefect;
               DestroyPressureDNV = destroyPressureDNV;
               DezPressureDNV = dezPressureDNV;
               KoefDezPressureDNV = koefDezPressureDNV;
               ColorDNV = colorDNV;
               RecomResponsDNV = recomResponsDNV;
           }

    }
}
