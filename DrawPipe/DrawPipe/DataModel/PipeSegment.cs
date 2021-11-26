using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrawPipe.DataModel;
using DrawPipe.ViewModel;

namespace DrawPipe.DataModel
{

    public class PipeSegment
    {
        public double StartX { get; set; }
        public double EndX { get; set; }
        public string NumberSegment { get; set; }
        public string Km { get; set; }
        public double Angle { get; set; }
        public double TubeRadius { get; set; }
        public string KeySegment { get; set; }
        public string KeyTypePipe { get; set; }
        public string TypePipe { get; set; }
        public bool VisibleCanvasDefect { get; set; }
        public bool VisibleLineShov { get; set; }
      

        public PipeSegment(double startX, double endX, string km, double angle, string numberSegment, string keySegment, string keytypePipe, string typePipe, double tubeRadius)
        {
            StartX = startX;
            EndX = endX;
            NumberSegment = numberSegment;
            Km = km;
            Angle = angle;
           // TubeRadius = 2.0;// в метрах tubeRadius;

            TubeRadius = tubeRadius;
            KeySegment = keySegment;
            KeyTypePipe = keytypePipe;
            TypePipe = typePipe;
            //VisibleCanvasDefect = visibleCanvasDefect;
            //VisibleLineShov = visibleLineShov;
            DefectList = new List<Defect>();
            GroupDefectList = new List<GroupDefect>();
            ColoniGroupDefectList = new List<ColoniGroupDefect>();
        }
        public List<GroupDefect> GroupDefectList { get; private set; }
        public List<ColoniGroupDefect> ColoniGroupDefectList { get; private set; }

        public List<Defect> DefectList { get; private set; }


        public double Length
        {
            get
            {
                return Math.Abs(EndX - StartX);
            }
        }
    }
}
