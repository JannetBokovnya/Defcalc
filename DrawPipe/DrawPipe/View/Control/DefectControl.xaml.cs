using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using DrawPipe.DataModel;
using DrawPipe.ViewModel;
using System;

namespace DrawPipe.View.Control
{
    /// <summary>
    /// Interaction logic for DefectControl.xaml
    /// </summary>
    public partial class DefectControl : UserControl
    {
        private Color _color;
        private bool _isFilled;

        public DefectControl()
        {
            InitializeComponent();
        }
        public void Init(string defectKey, double x, double y, double width, double height, DefectSpecies defectSpecy,  Color color)
        {
            DefectKey = defectKey;
            DefectSpecy = defectSpecy;
            _isFilled = false;
            _color = color;
            
            this.Height = Math.Max(height, 10);

         //   Debug.Assert(width > 0.0);

            this.Width = Math.Max(width, 8);
            Canvas.SetTop(this, y);
            Canvas.SetLeft(this, x);
            //border.BorderBrush = new SolidColorBrush(_color);
            
            if (defectSpecy == DefectSpecies.Group)
            {
                //border.Background = new SolidColorBrush(Color.FromArgb(50, 120,0,180));
                border.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 255, 215, 0));
                border.Background = new SolidColorBrush(Color.FromArgb(50, 255, 215, 0));
            }

            if (defectSpecy == DefectSpecies.Colonies)
            {
                //border.Background = new SolidColorBrush(Color.FromArgb(50, 120,0,180));
                border.BorderBrush = new SolidColorBrush(Color.FromArgb(150, 138, 43, 226));
               // border.Background = new SolidColorBrush(Color.FromArgb(0, 138, 43, 226));
                border.Background = new SolidColorBrush(Colors.Transparent);
                
            }

            if (defectSpecy == DefectSpecies.Single)
            {
                border.BorderBrush = new SolidColorBrush(Colors.DarkSlateGray);
                border.Background = new SolidColorBrush(Colors.Transparent);
                border.BorderBrush = new SolidColorBrush(_color);
            }
           

        }

        public string DefectKey { get; private set; }

        public DefectSpecies DefectSpecy { get; private set; }

        public PipeViewModel Model { get { return DataContext as PipeViewModel; } }

        public void Fill(bool isFilled)
        {
            _isFilled = isFilled;

            if (isFilled)
            {
                border.Background = new SolidColorBrush(_color);
            }
            else
            {
                border.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        public void SetColor(Color color)
        {
            _color = color;
            border.BorderBrush = new SolidColorBrush(_color);
            Fill(_isFilled);
        }

        private void border_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DefectSpecy == DefectSpecies.Single)
            {
                e.Handled = true;
                Model.DefectSelected = DefectKey;
            }
        }

    }
}
