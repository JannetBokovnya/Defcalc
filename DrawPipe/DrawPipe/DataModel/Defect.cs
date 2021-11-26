using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawPipe.DataModel
{
    public class Defect
    {
        /// <summary>
        /// Угловое положение дефекта. В часах
        /// </summary>
        public double Angle { get; private set; } 
        public double W { get; private set; }//длина дефекта
        public double H { get; private set; }//высота(ширина) дефекта
        public double ShiftX { get; private set; }// расстояние от начала сегмента (m)
        public string KeySegmentOnDefect { get; private set; } // ключ сегмента трубы на котором находится дефект
        public string TypeDefect { get; private set; } // тип дефекта(ключ)
        public string PercentDepth { get; private set; } // процент глубины дефекта(нужно для определения цвета)
        public string HintDefect { get; private set; } // хинт дефекта
        public string KeyDefect { get; private set; }
        public string ASME { get; private set; }//расчет дефекта по ASME
        public string DNV { get; private set; }//расчет дефекта по DNV 
        public string RSTRENG { get; private set; } //расчет дефекта по RSTRENG


        public Defect(double angleHours, double w, double h, double shiftX, string keySegmentOnDefect, 
            string typeDefect, string percentDepth, string hintDefect, string keyDefect, string asme,
            string dnv, string rstreng)
        {
            Angle = angleHours;
            W = w;
            H = h;
            ShiftX = shiftX;
            // Scale = scale;
            KeySegmentOnDefect = keySegmentOnDefect;
            TypeDefect = typeDefect;
            PercentDepth = percentDepth;
            HintDefect = hintDefect;
            KeyDefect = keyDefect;
            ASME = asme;
            DNV = dnv;
            RSTRENG = rstreng;
        }
    }

    public class GroupDefect
    {
        public double Angle { get; private set; } // угловое положение дефекта часах
        public double W { get; private set; }//длина дефекта
        public double H { get; private set; }//высота(ширина) дефекта
        public double ShiftX { get; private set; }// расстояние от начала сегмента
        public string KeySegmentOnDefect { get; private set; } // ключ сегмента трубы на котором находится дефект

        public GroupDefect(double angleHaurs, double w, double h, double shiftX, string keySegmentOnDefect)
        {
            Angle = angleHaurs;
            W = w;
            H = h;
            ShiftX = shiftX;
            KeySegmentOnDefect = keySegmentOnDefect;
        }
    }

    public  class ColoniGroupDefect
    {
        public double Angle { get; private set; } // угловое положение дефекта часах
        public double W { get; private set; }//длина дефекта
        public double H { get; private set; }//высота(ширина) дефекта
        public double ShiftX { get; private set; }// расстояние от начала сегмента
        public string KeySegmentOnDefect { get; private set; } // ключ сегмента трубы на котором находится дефект

        public ColoniGroupDefect(double angleHaurs, double w, double h, double shiftX, string keySegmentOnDefect)
        {
            Angle = angleHaurs;
            W = w;
            H = h;
            ShiftX = shiftX;
            KeySegmentOnDefect = keySegmentOnDefect;
        }
    }
}
