using System.Windows;
using DrawPipe.ViewModel;

namespace DrawPipe
{
    public partial class MainWindow : Window
    {
      //  private PipeViewModel _model;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          //  _model = new PipeViewModel();
           
         //   pipeControl.DataContext = _model;
            pipeControl.Init();
            
            pipeControl.Draw();
        }
    }
}
