using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DrawPipe.ViewModel;

namespace DrawPipe.View.Control
{
    /// <summary>
    /// Interaction logic for PipeLineSegmentControl.xaml
    /// </summary>
    public partial class PipeLineSegmentControl : UserControl
    {
        private string[] _sections;

        public PipeLineSegmentControl()
        {
            InitializeComponent();
        }

        public double HollowWidth { get { return double.IsNaN(border.Width) ? border.ActualWidth : border.Width; } }

        public double HollowHeight { get { return double.IsNaN(border.Height) ? border.ActualHeight : border.Height; } }


        public PipeViewModel Model { get { return DataContext as PipeViewModel; } }

        //рисование нижнего скролла прямоугольниками
        public void Draw(string[] sections)
        {
            _sections = sections;
            canvas.Children.Clear();
            //длина одного элемента прямоугольника
            double d = (HollowWidth) / sections.Length;
            double shift = 0.0;

            //int centrIndex = sections.Length / 2;//определяем центр длины массива (изминился алгоритм - это не важно!!!)
            //рисуем прямоугольники
            
            for (int i = 0; i < sections.Length; i++)
            {
                if (!string.IsNullOrEmpty(sections[i]))
                {
                    Rectangle r = new Rectangle();
                    r.Width = d;
                    r.Height = 20;
                    r.Stroke = new SolidColorBrush(Colors.Gray);
                    r.StrokeThickness = 1;

                    //раскраска нижнего скрола (помечаем трубы которые выбрали (3шт)) - тоже пока не нужно!!!
                    //if (i == centrIndex|| i == centrIndex-1 ||i == centrIndex+1)
                    //{
                    //    r.Fill = new SolidColorBrush(Colors.SeaGreen);

                    //}
                    //else
                    //{
                    //    r.Fill = new SolidColorBrush(Colors.CadetBlue);
                    //}

                    Canvas.SetLeft(r, shift);
                    canvas.Children.Add(r);
                }
                shift += d;
            }
           
        }

        private void border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(border);

            //длина одного элемента прямоугольника
            double d = (HollowWidth) / _sections.Length;

            //в этом индексе лежит нужный ключ(расчет положения нужной секции)
            int indexSection = (int) (point.X/d);

            if (indexSection >= 0 && indexSection <_sections.Length && !string.IsNullOrEmpty(_sections[indexSection]))
            {
                string key = _sections[indexSection];
                Model.NeedGoKey = key;
            }
          

        }
    }
}
