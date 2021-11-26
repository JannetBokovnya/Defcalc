using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using DEFCALC.DataModel;
using DEFCALC.Localization;
using DrawPipe.DataModel;
using Telerik.Windows.Controls;

namespace DEFCALC
{

    public partial class SelectionRegion : Window
    {
        
        public SelectionRegion()
        {
            LocalizationManager.DefaultResourceManager = ControlResources.ResourceManager;
            InitializeComponent();
            //передаем моделвью
            DataContext = App.Model;
        }

        MainViewModel Model { get { return App.Model; } }

        //запоминаем назване формы с которой пришли
        public Window PrevWindow;

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (Model.SelectedMG == null)
               LoadDataddlMG();
            if (Model.VisibleReport == 0)
                grdSelectRegion.RowDefinitions[4].Height = new GridLength(0);

            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;

        }

        /// <summary>
        /// загружаем комбобокс магистральный газопровод данными из базы загрузка в классе MainViewModel
        /// </summary>
         public void LoadDataddlMG()
        {
            Model.SelectRegion = "";
            Model.CountRegions = "";
            Model.SelectKilometr = "";
            Model.MGListFill();
           // btnSelectPipe.IsEnabled = false;
        }

         void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
         {
             //пока это делать не надо!
             //if ("SelectedgridRegion".Equals(e.PropertyName))
             //{
             //    if (Model.SelectedgridRegion != null)
             //    {
             //        btnSelectPipe.IsEnabled = true;    
             //    }
             //    else
             //    {
             //        btnSelectPipe.IsEnabled = false;  
             //    }

             //}
         }
 
        /// <summary>
        /// Клик на кнопку - выбрать трубу - переходим на форму - выбор трубы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectPipe_Click(object sender, RoutedEventArgs e)
        {
            if (Model.SelectedgridRegion != null)
            {
                Model.NameFormWindow = MainViewModel.NameFormWindows.SelectionRegion;
                SelectPipe selectPipe = new SelectPipe();
                selectPipe.Show();
                Application.Current.MainWindow = selectPipe;
                this.Close();    
            }
            else
            {
                MessageBox.Show("Не выбран участок.");
            }
            
        }

        /// <summary>
        /// Клик на кнопку выбрать акт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAkt_Click(object sender, RoutedEventArgs e)
        {
        }
        /// <summary>
        /// Клик на кнопке "Отменить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Model.SelectedNit = null;
            Model.GridRegionList.Clear();
            Model.SelectedMG = Model.MGList[0];
            Model.SelectRegion = "";
            Model.CountRegions = "";
            Model.SelectKilometr = "";


            switch (Model.NameFormWindow)
            {
                case MainViewModel.NameFormWindows.MainWindow:
                    {
                        //Переходим на главную страницу проекта
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Application.Current.MainWindow = mainWindow;
                        this.Close();
                    }
                    break;

                case MainViewModel.NameFormWindows.CreateActShurfovaniya:
                    {
                        //переходим на страницу создания акта шурфования
                        CreateActShurfovaniya createActShurfovaniya = new CreateActShurfovaniya();
                        createActShurfovaniya.ActwihtRegion = true;
                        createActShurfovaniya.Show();
                        Application.Current.MainWindow = createActShurfovaniya;
                        this.Close();
                    }
                    break;

            }

            
        }
    }
}

 ///// <summary>
 //        /// селект комбобкса мг - по мг должны выбираться все нити
 //        /// </summary>
 //        /// <param name="sender"></param>
 //        /// <param name="e"></param>
 //        private void ddlMG_SelectionChanged(object sender, SelectionChangedEventArgs e)
 //        {
 //            //if (Model.SelectedMG!=null)
 //            //{
                 
 //            //}

 //            //if (ddlMG.SelectedItem != null)
 //            //{
 //            //    _keyMg = "";
 //            //    DataTable dt = CBLL.GetNitOnMg(_keyMg);
 //            //    //наполняем комбобокс нитями
 //            //}
 //        }

        ///// <summary>
        // /// по клику на аомбобокс нити должны появиться в таблице все участки нити
        // /// </summary>
        // /// <param name="sender"></param>
        // /// <param name="e"></param>
        // private void ddlNitka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // {
        //     if (ddlNitka.SelectedItem != null)
        //     {
        //         _keyNit = "";
        //         DataTable dt = CBLL.GetAllregionsOnNit(_keyNit);
        //         //наполняем таблицу участками
        //     }
        // }

 ///// <summary>
 //       /// по клику на строку грида выбираем ключ участка и по нему загружаем все трубы на этом участке
 //       /// </summary>
 //       /// <param name="sender"></param>
 //       /// <param name="e"></param>
 //       private void rgrvRegion_SelectionChanged(object sender, SelectionChangeEventArgs e)
 //       {
 //          // _keyRegion = ((DataListRelationObjItems)grid.SelectedItem).KeyObj;
 //       }