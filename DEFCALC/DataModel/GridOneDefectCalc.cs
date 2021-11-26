using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
  public  class GridOneDefectCalc
    {
        public string PressureOnDefect { get; private set; } // названия различных давлений расчета дефекта
        public string ASME { get; private set; }//расчет дефекта по ASME
        public string DNV { get; private set; }//расчет дефекта по DNV 
        public string DNVGRUP { get; private set; }//расчет группового дефекта по DNV 
        public string RSTRENG { get; private set; } //расчет дефекта по RSTRENG

        public GridOneDefectCalc(string pressureondefect, string asme, string dnv, string dnvgrup, string rstreng)
        {
            PressureOnDefect = pressureondefect;
            ASME = asme;
            DNV = dnv;
            DNVGRUP = dnvgrup;
            RSTRENG = rstreng;
        }
    }
}
