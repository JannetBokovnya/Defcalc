using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class SelectPipeOnGrid
    {
        public string KeyPipe { get; set; }
        public string KmPipe { get; set; }
        public string NumberPipe { get; set; }
        public double ANGLESHOV { get; set; }//угол шва
        public string LENGHT { get; set; }//длина
        public string DEPTHPIPE { get; set; }//толщина стенки
        public string NUMBERDEFECT { get; set; }//количество дефектов
        public string NUMBERAKT { get; set; }//номер акта
        public double TUBERADIUS { get; set; } //радиус трубы
        public string MAXDEPTHDEF { get; set; } //МАКСИМАЛЬНАЯ ГЛУБИНА ДЕФЕКТА (для вывода в таблице)
        public string MAXSQRDEF { get; set; } //максимальная площпдь дефекта (для вывода в таблице)
       
        

        public SelectPipeOnGrid(string keyPipe, string kmPipe, string numberPipe, 
                                double angleshov, string lenght, string depthpipe, 
                                string numberdefect, string numberakt, double tubeRadius,
                                string maxdepthdef, string maxsqrdef)
        {
            KeyPipe = keyPipe;
            KmPipe = kmPipe;
            NumberPipe = numberPipe;
            ANGLESHOV = angleshov;
            LENGHT = lenght;
            DEPTHPIPE = depthpipe;
            NUMBERDEFECT = numberdefect;
            NUMBERAKT = numberakt;
            TUBERADIUS = tubeRadius;
            MAXDEPTHDEF = maxdepthdef;
            MAXSQRDEF = maxsqrdef;

        }
    }
}
