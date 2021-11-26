using System.Windows;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string userKey = string.Empty;

        private static MainViewModel _mainViewModel = null;

        public static MainViewModel Model
        {
            get
            {
                if (_mainViewModel==null)
                {
                    _mainViewModel = new MainViewModel();
                }
                return _mainViewModel;
            }
        }
    }
}
