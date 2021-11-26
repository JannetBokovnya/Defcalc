using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DrawPipe.DataModel;
using DrawPipe.ViewModel;
using System.Windows.Shapes;

namespace DrawPipe.View.Control
{
    /// <summary>
    /// Interaction logic for PipeSegmentControl.xaml
    /// </summary>
    public partial class PipeSegmentControl : UserControl
    {
        public PipeSegmentControl()
        {
            InitializeComponent();
        }

        public double HollowWidth
        {
            get
            {
                //Debug.Assert(this.Visibility == Visibility.Visible);

                return double.IsNaN(border.Width) ? border.ActualWidth : border.Width;
            }
        }

        public double HollowHeight
        {
            get
            {
                //Debug.Assert(this.Visibility == Visibility.Visible);
                return double.IsNaN(border.Height) ? border.ActualHeight : border.Height;
            }
        }

        public PipeViewModel Model { get { return DataContext as PipeViewModel; } }

        public PipeSegment PipeSegment { get; private set; }

        public void Init(PipeSegment ps)
        {
          //  Debug.Assert(ps.Length > 0.0);

            PipeSegment = ps;
            //kmTxt.Text = ps.KeySegment.ToString();
            kmTxt.Text = "Номер трубы "+ps.NumberSegment.ToString();

            Draw();
        }

        public void HiglightDefect(string keyDefect)
        {
            foreach (var child in canvasDefect.Children)
            {
                if (child is DefectControl)
                {
                    var dc = child as DefectControl;
                    if (string.Equals(dc.DefectKey, keyDefect))
                    {
                        dc.Fill(true);
                    }
                    else
                    {
                        dc.Fill(false);
                    }
                }
            }
        }

        public void SefDefectColor(string keyDefect, Color color)
        {
            foreach (var child in canvasDefect.Children)
            {
                if (child is DefectControl)
                {
                    var dc = child as DefectControl;
                    if (string.Equals(dc.DefectKey, keyDefect))
                    {
                        dc.SetColor(color);
                    }
                }
            }
        }

        private void Draw()
        {
            canvas.Children.Clear();
            if (PipeSegment != null)
            {
              //  Debug.WriteLine("Draw " + PipeSegment.KeySegment);

                //расчет положения продольного шва
                DrowDefect();
                double y = PipeSegment.Angle * HollowHeight / 12.0;
                Line l = new Line
                  {
                      Stroke = new SolidColorBrush(Colors.Red),
                      X1 = 0,
                      X2 = HollowWidth,
                      Y1 = y,
                      Y2 = y
                  };

                canvas.Children.Add(l);
                if (PipeSegment.TypePipe == "двушовная")
                {
                    double angle = PipeSegment.Angle + 6;
                    if (angle > 12)
                    {
                        angle = angle - 12;
                    }
                    double yd = (angle) * HollowHeight / 12.0;
                    Line ld = new Line
                    {
                        Stroke = new SolidColorBrush(Colors.Red),
                        X1 = 0,
                        X2 = HollowWidth,
                        Y1 = yd,
                        Y2 = yd
                    };
                    canvas.Children.Add(ld);
                }
            }
        }

        private void DrowDefect()
        {
            const double ToMeter = 0.001;
            const double ToMeter1 = 1;

            canvasDefect.Children.Clear();

            for (int ii = 0; ii < PipeSegment.ColoniGroupDefectList.Count; ii++)
            {
                var cgd = PipeSegment.ColoniGroupDefectList[ii];

                double y = cgd.Angle * (HollowHeight / 12.0);
                //double h = (HollowHeight / (Math.PI * 2 * PipeSegment.TubeRadius)) * (cgd.H * ToMeter);
                double h = HollowHeight;
                double x = (HollowWidth / PipeSegment.Length) * (cgd.ShiftX * ToMeter1);
                double w = HollowWidth * (cgd.W * ToMeter) / PipeSegment.Length;

                DefectControl defectControl = new DefectControl();
               // Color.FromArgb(200, 0, 250, 250)
                //defectControl.Init(string.Empty, x, y, w, h, DefectSpecies.Colonies, Color.FromArgb(150,138,43,226));
                //делаем прозрачный
                defectControl.Init(string.Empty, x, y, w, h, DefectSpecies.Colonies, Colors.Transparent);
                canvasDefect.Children.Add(defectControl);
            }

            for (int i = 0; i < PipeSegment.GroupDefectList.Count; i++)
            {
                var gd = PipeSegment.GroupDefectList[i];

                double y = gd.Angle * (HollowHeight / 12.0);
                double h = (HollowHeight / (Math.PI * 2 * PipeSegment.TubeRadius)) * (gd.H * ToMeter);
                double x = (HollowWidth / PipeSegment.Length) * (gd.ShiftX * ToMeter1);
                double w = HollowWidth * (gd.W * ToMeter) / PipeSegment.Length;

                DefectControl defectControl = new DefectControl();
               // Color.FromArgb(200, 0, 250, 250)
                defectControl.Init(string.Empty, x, y, w, h, DefectSpecies.Group, Color.FromArgb(80, 255, 215, 0));
                canvasDefect.Children.Add(defectControl);
            }

          //  Debug.WriteLine("+++++++++++++++++++++");
          //  Debug.WriteLine("Defects: {1}, Key segment: {0} ", PipeSegment.KeySegment, PipeSegment.DefectList.Count);

            for (int i = 0; i < PipeSegment.DefectList.Count; i++)
            {
                double y = PipeSegment.DefectList[i].Angle * (HollowHeight / 12.0);
                double h = (HollowHeight / (Math.PI * 2 * PipeSegment.TubeRadius)) * (PipeSegment.DefectList[i].H * ToMeter);
                double x = (HollowWidth/PipeSegment.Length) *(PipeSegment.DefectList[i].ShiftX * ToMeter1);
                double w = HollowWidth * (PipeSegment.DefectList[i].W * ToMeter) / PipeSegment.Length;

                if (HollowWidth <= 0.0)
                {
                    Debugger.Break();
                    string err = "";
                }

                DefectControl defectControl = new DefectControl(); // ColorDefectDop
                //так было
                //defectControl.Init(PipeSegment.DefectList[i].KeyDefect, x, y, w, h, DefectSpecies.Single, Colors.DarkGray);
                //новое
                defectControl.Init(PipeSegment.DefectList[i].KeyDefect, x, y, w, h, DefectSpecies.Single, 
                    ColorDefectDop(PipeSegment.DefectList[i].ASME, PipeSegment.DefectList[i].DNV, PipeSegment.DefectList[i].RSTRENG));

               // Debug.WriteLine("Key defect: {0}, x {1}, y {2}, w {3}, h {4}, pipeSLen {5}", PipeSegment.DefectList[i].KeyDefect, x, y, w, h, PipeSegment.Length);
                canvasDefect.Children.Add(defectControl);
            }

          //  Debug.WriteLine("--------------------------");

        }

        private Color ColorDefectDop(string aSME, string dNV, string rSTR)
        {
            Color colorDefect = Colors.Gray;
            string ASME = aSME;
            string DNV = dNV;
            string RSTR = rSTR;

            if ((ASME == "White") || (DNV == "White") || (RSTR == "White"))
            {
                colorDefect = Colors.Gray;
            }
            else
            {
                int aa = 0;


                if ("Orange".Equals(ASME, StringComparison.OrdinalIgnoreCase) ||
                    "Orange".Equals(DNV, StringComparison.OrdinalIgnoreCase) ||
                    "Orange".Equals(RSTR, StringComparison.OrdinalIgnoreCase))
                {
                    aa = 1;
                }

                if ("red".Equals(ASME, StringComparison.OrdinalIgnoreCase) ||
                    "red".Equals(DNV, StringComparison.OrdinalIgnoreCase) ||
                    "red".Equals(RSTR, StringComparison.OrdinalIgnoreCase))
                {
                    aa = 2;
                }

                if (aa == 0)
                {
                    colorDefect = Colors.Green;
                }

                if (aa == 1)
                {
                    colorDefect = Colors.Orange;
                }
                if (aa == 2)
                {
                    colorDefect = Colors.Red;
                }
            }
            return colorDefect;
        }


        private void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            Draw();
        }

        private void CanvasDefect_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Model.SegmentSelected = PipeSegment.KeySegment;
        }


    }
}
