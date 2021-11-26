using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
    public class GridAct : StatusAnswer
    {
       public string KEYACT { get; set; } //ключ акта
       public string NUMBERACT { get; set; } //номер акта
       public string DATAACT { get; set; }//дата акта
       public DateTime DataActDT { get; set; }//дата акта
       public string KMAKT { get; set; }//километраж
       public string NUMBERPIPE { get; set; }//номер трубы
       public string ANGLESHOV { get; set; } //УГОЛ ШВА
       public string STATUS { get; set; } //статус
       public string PLACEATATE { get; set; }//МЕСТОПОЛОЖЕНИЕ
       public string KEYREGION { get; set; } //ключ участка
       public string COUNTPIPE { get; set; } //количество труб на акте
       public string NUMBERPIPELIST { get; set; }//перечисление всех труб в акте
       public string KMPIPELIST { get; set; }//километражи всех труб в акте
       public string ISEDITED { get; set; } //подпись
       


       public GridAct(string keyAact, string dataAct, DateTime dataActDT, string numberAct, string kmAct, string numberPipe, 
                      string angleshov, string status, string placestate, string keyRegion, string countPipe,
                      string numberpipelist, string kmpipelist, string isEdited)
       {
           KEYACT = keyAact;
           NUMBERACT = numberAct;
           DATAACT = dataAct;
           DataActDT = dataActDT;
           KMAKT = kmAct;
           NUMBERPIPE = numberPipe;
           ANGLESHOV = angleshov;
           STATUS = status;
           PLACEATATE = placestate;
           KEYREGION = keyRegion;
           COUNTPIPE = countPipe;
           NUMBERPIPELIST = numberpipelist;
           KMPIPELIST = kmpipelist;
           ISEDITED = isEdited;
       }
    }
}
