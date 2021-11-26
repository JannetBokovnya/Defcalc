using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DEFCALC.DataModel
{
   public class CalculateDefect
    {
      
           public string KeyDefect { get; set; }//ключ дефекта
           public string DopDefect { get; set; } //допкстимость дефекта
           public string MaxPressureDopASME { get; set; }//макс допустимое давление ASME
           public string DestroyPressureASME { get; set; }//разрушающее давление ASME
           public string MaxDopLenghtDefASME { get; set; }//макс допустимая длина дефекта ASME
           public string MaxDopDepthASME { get; set; }//макс допустимая глубина деф
           public string RecomResponsASME { get; set; }//рекомендация по реагированию ASME
           public string DestroyPressureDNV { get; set; }//разрушающее давление по DNV
           public string DezPressureDNV { get; set; }//безопасное давление DNV
           public string KoefDezPressureDNV { get; set; }//коэффициент безопасного давления
           public string DestroyPressureRestreng { get; set; }//разрушающее давление Restreng
           public string CoefZapRestreng { get; set; }//коэффициент запаса Restreng
           public string RecomResponsRestreng { get; set; }//рекомендации по реагированию Restreng
           public Brush ColorASME { get; set; } // цвет заливки дефекта по ASME
           public Brush ColorDNV { get; set; } // цвет заливки дефекта по ASME
           public Brush ColorRestreng { get; set; } // цвет заливки дефекта по ASME
           public string RecomResponsDNV{ get; set; }//рекомендация по реагированию DNV !!!! добавить в базу!!!!
           


           public CalculateDefect(string keyDefect, string dopDefect, string maxPressureDopASME, string destroyPressureASME,
                                  string maxDopLenghtDefASME, string maxDopDepthASME, string recomResponsASME,
                                  string destroyPressureDNV, string dezPressureDNV, string koefDezPressureDNV,
                                  string destroyPressureRestreng, string coefZapRestreng, string recomResponsRestreng,
                                  Brush colorASME, Brush colorDNV, Brush colorRestreng, string recomResponsDNV)
           {
               KeyDefect = keyDefect;
               DopDefect = dopDefect;
               MaxPressureDopASME = maxPressureDopASME;
               DestroyPressureASME = destroyPressureASME;
               MaxDopLenghtDefASME = maxDopLenghtDefASME;
               MaxDopDepthASME = maxDopDepthASME;
               RecomResponsASME = recomResponsASME;
               DestroyPressureDNV = destroyPressureDNV;
               DezPressureDNV = dezPressureDNV;
               KoefDezPressureDNV = koefDezPressureDNV;
               DestroyPressureRestreng = destroyPressureRestreng;
               CoefZapRestreng = coefZapRestreng;
               RecomResponsRestreng = recomResponsRestreng;
               ColorASME = colorASME;
               ColorDNV = colorDNV;
               ColorRestreng = colorRestreng;
               RecomResponsDNV = recomResponsDNV;


           }

           
    }
}
