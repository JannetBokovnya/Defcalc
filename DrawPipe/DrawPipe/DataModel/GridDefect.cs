using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DrawPipe.DataModel
{
    public class GridDefect 
    {
        public double Angle { get; private set; } // Radian угловое положение дефекта
        public double W { get; private set; }//длина дефекта
        public double H { get; private set; }//высота(ширина) дефекта
        public double ShiftX { get; private set; }// расстояние от начала трубы
        public string KeySegmentOnDefect { get; private set; } // ключ сегмента трубы на котором находится дефект
        public string TypeDefect { get; private set; } // тип дефекта(ключ)
        public string PercentDepth { get; private set; } // процент глубины дефекта(нужно для определения цвета)
        public string HintDefect { get; private set; } // хинт дефекта
        public string NumberDefect { get; private set; } //номер дефекта
        public string Poterimetal { get; private set; } //потери металла
        public string KeyDefect { get; private set; } //ключ дефекта
        public string Depth { get; private set; } //глубина дефекта
        public string ASME { get; private set; }//расчет дефекта по ASME
        public string DNV { get; private set; }//расчет дефекта по DNV 
        public string RSTRENG { get; private set; } //расчет дефекта по RSTRENG
        public string StatusPassport { get; private set; }
        

        

        public GridDefect(string keyDefect, double angleRadian, double w, double h,
            double shiftX, string keySegmentOnDefect, string typeDefect, string depth,
            string hintDefect, string numberDefect, string poterimetal, string asme,
            string dnv, string rstreng,  string statusPassport)
        {
            Angle = angleRadian;
            W = w;
            H = h;
            ShiftX = shiftX;
            // Scale = scale;
            KeySegmentOnDefect = keySegmentOnDefect;
            TypeDefect = typeDefect;
            Depth = depth;
            HintDefect = hintDefect;
            NumberDefect = numberDefect;
            Poterimetal = poterimetal;
            KeyDefect = keyDefect;
            ASME = asme;
            DNV = dnv;
            RSTRENG = rstreng;
            StatusPassport = statusPassport;
            
        }
    }
}
