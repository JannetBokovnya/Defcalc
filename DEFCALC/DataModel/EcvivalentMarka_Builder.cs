using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
    public class EcvivalentMarka_Builder: BaseModel
    {

        public string KeFactory_builder { get; set; } //ключ завода изготовителя
        public string KeyFeel_grade { get; set; } // ключ марки стали
        public string Range_fluid { get; set; } //предел текучести
        public string Range_stranght { get; set; } //предел прочности
        public string ModuleUng { get; set; } // модуль юнга
        public string KoefPuanson { get; set; } //коэффициент пуансона
        public string KoefLinExpansion { get; set; } //коэффициент линейного расширения
        public string NameFeelGrade { get; set; }//название марки стали - нужно для комбобокса марки стали

        public EcvivalentMarka_Builder(string keFactory_builder, string keyFeel_grade, string range_fluid, string range_stranght,
                                       string moduleUng, string koefPuanson, string koefLinExpansion, string nameFeelGrade)
        {
            KeFactory_builder = keFactory_builder;
            KeyFeel_grade = keyFeel_grade;
            Range_fluid = range_fluid;
            Range_stranght = range_stranght;
            ModuleUng = moduleUng;
            KoefPuanson = koefPuanson;
            KoefLinExpansion = koefLinExpansion;
            NameFeelGrade = nameFeelGrade;

        }

        
       
    }
}
