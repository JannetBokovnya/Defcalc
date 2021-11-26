using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawPipe.ViewModel;

namespace DrawPipe.View.Control
{
    public partial class PipeControl : UserControl
    {
        public PipeControl()
        {
            InitializeComponent();
        }
        public PipeViewModel Model { get { return DataContext as PipeViewModel; } }

        public double HollowWidth { get { return double.IsNaN(holder.Width) ? holder.ActualWidth : holder.Width; } }

        public double HollowHeight { get { return double.IsNaN(holder.Height) ? holder.ActualHeight : holder.Height; } }

        public PipeSegmentControl SingleSegment { get { return singleSegment; } }

        public void Init()
        {
            //убрала слайдер 
            //slider.Minimum = 0;
            //slider.Maximum = Model.Pipe.SegmentList.Count;
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }
        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (("TypeShov".Equals(e.PropertyName)))
            {
                Draw();
            }
            else if (("NeedGoKey".Equals(e.PropertyName)))
            {
                int index = Model.GetIndexByKey(Model.NeedGoKey);
                if (index!=-1)
                {
                    Model.CentralIndex = index;
                }
            }
            else if (("CentralIndex".Equals(e.PropertyName)))
            {
                Draw();
            }
            else if (("IsVisibleButton".Equals(e.PropertyName)))
            {
               if (Model.IsVisibleButton != true)
               {
                   leftButtom.Visibility = Visibility.Collapsed;
                   rightButtom.Visibility = Visibility.Collapsed;
               }
               else
               {
                   leftButtom.Visibility = Visibility.Visible;
                   rightButtom.Visibility = Visibility.Visible;
               }

               

            }
        }
        //рисуем на канвасе pipeControla сегменты труб с уже загруженными данными
        public void Draw()
        {
            if (Model.IsSingleSegment)
            {
                if (Model.Pipe.SegmentList.Count > 0 && Model.CentralIndex < Model.Pipe.SegmentList.Count)
                {
                    singleSegment.Init(Model.Pipe.SegmentList[Model.CentralIndex]);
                }
            }
            else
            {
                if (Model.Pipe.SegmentList.Count > 0 && Model.CentralIndex < Model.Pipe.SegmentList.Count +1)
                {
                    leftSegment.Init(Model.Pipe.SegmentList[Model.CentralIndex - 1]);
                    centreSegment.Init(Model.Pipe.SegmentList[Model.CentralIndex]);
                    righSegment.Init(Model.Pipe.SegmentList[Model.CentralIndex + 1]);
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //на основном контроле рисуем пунктирные линии часовой развертки трубы
            DrawDashLines();

            if (Model != null)
            {
                Model.FillSegments(0);
            }
        }

        private void DrawDashLines()
        {
            dashCanvas.Children.Clear();
            double dy = (HollowHeight - 40.0) / 4.0;
            for (int i = 0; i < 5; i++)
            {
                Line l = new Line();
                l.Stroke = new SolidColorBrush(Colors.Gray);
                l.StrokeDashArray = new DoubleCollection() { 4, 4 };
                l.StrokeThickness = 0.5;
                l.X1 = 0;
                l.X2 = HollowWidth;
                l.Y1 = l.Y2 = dy * i + 40.0;
                dashCanvas.Children.Add(l);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.IsSingleSegment = !Model.IsSingleSegment;
            if (!Model.IsSingleSegment && Model.CentralIndex == 0)
            {
                Model.CentralIndex = 1;
            }
        }


        private void leftButtom_Click(object sender, RoutedEventArgs e)
        {
            GoLeft();
        }

        private void rightButtom_Click(object sender, RoutedEventArgs e)
        {
            GoRight();
        }

        private void GoLeft()
        {
            Model.CentralIndex--;
        }

        private void GoRight()
        {
            Model.CentralIndex++;
        }

        public void HiglightDefect(string keyDefect)
        {
            singleSegment.HiglightDefect(keyDefect);
        }

        public void  SefDefectColor(string keyDefect, Color color)
        {
            singleSegment.SefDefectColor(keyDefect, color);
        }

        private void PipeControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
        }
    }
}
