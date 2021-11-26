using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class OutDefectCalc
    {
           public string RankDanger { get; set; } //ранг опасности
           public string DestroyPres { get; set; } //Максимальное (Разрушающее) давление (МПа)
           public string MaxDepthForCorPipe { get; set; }//с заданной длиной коррозии максимальная глубина будет равна
           public string MaxLengthForPipe { get; set; } //с заданной глубиной коррозии максимальная длина будет равна
           public string SafeWorkPres { get; set; }//Допустимое давление (МПа)
           public string Recom { get; set; }//Рекомендации
           public string StrengthCondition { get; set; }//допустимость деффекта
           public string StrengthKey { get; set; } //допустимость дефекта
           public string Kzap { get; set; } // Реальный коэффициент запаса
           public string Time { get; set; } // Время безопасной експлуатации (остаточный ресурс) (лет)


       //  {"RankDanger":"0.4992523122621505","DestroyPres":"13.289991568451473",
       //"MaxDepthForCorPipe":"","MaxLengthForPipe":"14.4","SafeWorkPres":"7.614",
       //"Recom":"?????? ?? ???????? ??????? ??? ??????? ??????? ???????????? ????????????",
       //"StrengthCondition":"??????????","StrengthKey":"0"}

       public Dictionary<string, string> GetFieldsAndValues()
       {
           Dictionary<string, string> d = new Dictionary<string, string>();

           if (!string.IsNullOrWhiteSpace(DestroyPres))
           {
               d["nDestroyPressure"] = "'" + DestroyPres + "'";
           }
           if (!string.IsNullOrWhiteSpace(MaxLengthForPipe))
           {
               d["nMaxDopLenghtDefASME"] = "'" + MaxLengthForPipe + "'";
           }
           if (!string.IsNullOrWhiteSpace(MaxDepthForCorPipe))
           {
               d["nMaxDopDepthASME"] = "'" + MaxDepthForCorPipe + "'";
           }
           if (!string.IsNullOrWhiteSpace(Recom))
           {
               d["cRecomRespons"] = "'" + Recom + "'";
           }
           if (!string.IsNullOrWhiteSpace(SafeWorkPres))
           {
               d["nMaxPressureDop"] = "'" + SafeWorkPres + "'";
           }
           if (!string.IsNullOrWhiteSpace(StrengthKey))
           {
               d["nDict_Dip_Defect"] = "'" + StrengthKey + "'";
           }

           if (!string.IsNullOrWhiteSpace(Kzap))
           {
               d["nCoefZap"] = "'" + Kzap + "'";
           }

           if (!string.IsNullOrWhiteSpace(Time))
           {
               d["dDateEdit"] = "'" + Time + "'";
           }
          
           return d;
       }
    }
}
