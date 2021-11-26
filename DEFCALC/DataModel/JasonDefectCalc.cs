using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class JasonDefectCalc
    {
       
       public class Tube
       {
           /// <summary>
           /// толщина трубы
           /// </summary>
           public string TubeWidth { get; set; }
           /// <summary>
           /// диаметр трубы
           /// </summary>
           public string Diam { get; set; } 
           /// <summary>
           /// максимально допустимое рабочее давление
           /// </summary>
           public string WorkPress{ get; set; }
           /// <summary>
           /// предел текучести
           /// </summary>
           public string PredTek{ get; set; }
           /// <summary>
           /// реальное рабочее давление
           /// </summary>
           public string RealWorkPress{ get; set; }
           /// <summary>
           /// номинальное продольное напряжение
           /// </summary>
           public string SigmaL{ get; set; }
           /// <summary>
           /// предел прочности
           /// </summary>
           public string PredProch{ get; set; }
           /// <summary>
           /// предел прочности(предельное напряжение материала )
           /// </summary>
           public string PredNapr{ get; set; }
           /// <summary>
           /// коэффициент Пуансона
           /// </summary>
           public string Puas{ get; set; }
           /// <summary>
           /// длина трубы
           /// </summary>
           public string LengthReg{ get; set; }
           /// <summary>
           /// категория участка трубопроводв
           /// </summary>
           public string PipeCat{ get; set; }
           /// <summary>
           /// коэффициент линейного расширения
           /// </summary>
           public string LinRas{ get; set; }
           /// <summary>
           /// модуль Юнга
           /// </summary>
           public string Jung{ get; set; }
           /// <summary>
           /// время ремонта трубы(годы)
           /// </summary>
           public string Tp{ get; set; }
           /// <summary>
           /// время работы отрем трубы с момента начало до конца экспл(годы)
           /// </summary>
           public string Teks{ get; set; }
           /// <summary>
           /// проектное давление трубы (МПа)
           /// </summary>
           public string Pproject{ get; set; }

           public List<Defect> Defects { get; set; }

           public Tube()
           {
               Defects = new List<Defect>();
           }
       }

       public class Defect
       {
           /// <summary>
           /// максимальная глубина коррозии
           /// </summary>
           public string CorDepth { get; set; }
           /// <summary>
           /// максимальная длина коррозии
           /// </summary>
           public string MaxCorLength { get; set; }
           /// <summary>
           /// ширина области коррозии
           /// </summary>
           public string CorWidth { get; set; }
           /// <summary>
           /// угол
           /// </summary>
           public string Angle { get; set; }
           /// <summary>
           /// километраж дефекта
           /// </summary>
           public string Kilometer { get; set; }
           /// <summary>
           /// тип дефекта
           /// </summary>
           public string Type { get; set; }
           /// <summary>
           /// идентификатор дефекта
           /// </summary>
           public string ID { get; set; }
       }

       public class Defectoscope
       {
           public string DefectoscopeJson { get; set; }
       }

       public class Coeffs
       {
           /// <summary>
           /// коэффициент дизайна
           /// </summary>
           public string DesignCoef {get; set; }
           /// <summary>
           /// модельный коэффициент
           /// </summary>
           public string ModelCoef { get; set; }
           /// <summary>
           /// поправочный коэффициент учитывает влияние температуры
           /// </summary>
           public string KoefT { get; set; }
       }

      

       public class TubeJason
       {
           public Tube Tube { get; set; }
           public Defectoscope Defectoscope { get; set; }
           public Coeffs Coeffs { get; set; }
           public string DefType { get; set; }
       }

      
    }
}

