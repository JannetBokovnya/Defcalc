using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DEFCALC.DataModel;
using DEFCALC.Localization;
using Telerik.Windows.Controls;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for ListOfActs.xaml
    /// </summary>
    public partial class ListOfActs : Window
    {
        public ListOfActs()
        {
            LocalizationManager.DefaultResourceManager = ControlResources.ResourceManager;
            InitializeComponent();
            //передаем моделвью
            DataContext = App.Model;
        }

        MainViewModel Model { get { return App.Model; } }

        /// <summary>
        /// Клик на кнопке "Открыть акт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenActShurfovaniya_Click(object sender, RoutedEventArgs e)
        {
            //if (rgrvListAct.SelectedItems.Count != 0)
            //{

            //    Model.KeyAkt = Model.SelectedgridAct.KEYACT;
            //    //наполняем данными трубу на которой находится шурф
            //    Model.GetSelectPipeOnkeyAct();
            //    //сохраняем в моделе ключ акта - в нем вызываются все дефекты
            //    Model.GetKeyAkt = Model.KeyAkt;

            //    Model.NumberAct = Model.SelectedgridAct.NUMBERACT;
            //    Model.DataAct = Model.SelectedgridAct.DATAACT;
            //    //сохраняем в xml данные по акту и с выбором мг, нитки и т.д.
            //    SavePipe sp = new SavePipe();
            //    sp.KeyMg = Model.SelectMgKey;
            //    sp.NameMg = Model.SelectMgName;
            //    sp.KeyNit = Model.SelectNitKey;
            //    sp.NameNit = Model.SelectNitName;
            //    sp.KmBegin = Model.SelectedKmBegin;
            //    sp.KmEnd = Model.SelectedKmEnd;
            //    sp.NameRegion = Model.SelectRegionName;
            //    sp.KeyRegion = Model.SelectKeyRegion;
            //    sp.KeyAkt = Model.KeyAkt;
            //    sp.NumberAkt = Model.NumberAct;
            //    sp.DataAkt = Model.DataAct;
            //    sp.KmAkt = Model.SelectPipeKm;
            //    sp.NumberPipe = Model.SelectPipeNumberPipe;

            //    FileStream fs = new FileStream(@"SavePipe.xml", FileMode.Create, FileAccess.Write);
            //    Helper.Serialize(fs, sp);
            //    fs.Close();


            //    EditAct editAct = new EditAct();
            //    editAct.Show();
            //    Application.Current.MainWindow = editAct;
            //    this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Не выбран акт шурфования!");
            //}
        }

        /// <summary>
        /// Клик на кнопке "Отменить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Переходим на главную страницу проекта
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.MainWindow = mainWindow;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadListAct();
            

            lblSelectedSite.Text = "МГ " + Model.SelectMgName +
                                                       ", " + Model.SelectRegionName +
                                                       ", " + Model.SelectedKmEnd + " км " +
                                                       " - " + Model.SelectedKmBegin + " км ";



        }

        public void LoadListAct()
        {
            Model.GridListAct();
        }
    }
}
