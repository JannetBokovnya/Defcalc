using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
  public  class GridPipe
    {
       public string KEYPIPE { get; set; } //ключ трубы
       public string NUMBERPIPE { get; set; } //номер трубы
       public string KM { get; set; }//километраж
       public string ANGLESHOV { get; set; }//угол шва
       public string LENGHT { get; set; }//длина
       public string DEPTHPIPE { get; set; }//толщина стенки
       public string NUMBERDEFECT { get; set; }//количество дефектов
       public string NUMBERAKT { get; set; }//номер акта

       public GridPipe(string keypipe, string numberpipe, string km, string angleshov, string lenght, string depthpipe, string numberdefect, string numberakt)
       {
           KEYPIPE = keypipe;
           NUMBERPIPE = numberpipe;
           KM = km;
           ANGLESHOV = angleshov;
           LENGHT = lenght;
           DEPTHPIPE = depthpipe;
           NUMBERDEFECT = numberdefect;
           NUMBERAKT = numberakt;

       }
    }
}
