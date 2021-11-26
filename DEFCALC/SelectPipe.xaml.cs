using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using DEFCALC.DataModel;
using DrawPipe.DataModel;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for SelectPipe.xaml
    /// </summary>
    public partial class SelectPipe : Window
    {

        public SelectPipe()
        {
            InitializeComponent();
            //передаем моделвью
            DataContext = App.Model;
        }

        MainViewModel Model { get { return App.Model; } }
        

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

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
                        Model.NameFormWindow = MainViewModel.NameFormWindows.CreateActShurfovaniya;
                        createActShurfovaniya.ActwihtRegion = true;
                        createActShurfovaniya.Show();
                        Application.Current.MainWindow = createActShurfovaniya;
                        this.Close();
                    }
                    break;

                case MainViewModel.NameFormWindows.SelectionRegion:
                    {
                        //переходим на страницу выбор трубы
                        Model.SelectedgridRegion = null;
                        SelectionRegion selectionRegion = new SelectionRegion();
                        Model.NameFormWindow = MainViewModel.NameFormWindows.MainWindow;
                        selectionRegion.Show();
                        Application.Current.MainWindow = selectionRegion;
                        this.Close();
                    }
                    break;

            }
            
 
            
        }
 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model.VisibleReport == 0)
                grdSelectPipe.RowDefinitions[5].Height = new GridLength(0);
            LoadDataGridSelectPipe();
            Model.PipeModel.PropertyChanged += PipeModel_PropertyChanged;
        }

        private bool _isIgnoreGridSelection;

        private void PipeModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("SegmentSelected".Equals(e.PropertyName))
            {

                foreach (var item in rgrvSelectPipe.Items)
                {
                    if(item is GridPipe)
                    {
                        var gp = item as GridPipe;
                        if (string.Equals(gp.KEYPIPE, Model.PipeModel.SegmentSelected))
                        {
                            rgrvSelectPipe.SelectedItem = item;
                            rgrvSelectPipe.ScrollIntoView(item);
                        }

                    }
                }
                //string key = Model.PipeModel.SegmentSelected;
                //_isIgnoreGridSelection = true;

                //// rgrvSelectPipe.SelectedItems.Add(rgrvSelectPipe.Items[4]);

                //_isIgnoreGridSelection = false;

            }

            
        }
        
        private void Window_UnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PipeModel.PropertyChanged -= PipeModel_PropertyChanged;
            if (Model.VisibleReport == 0)
                grdSelectPipe.RowDefinitions[5].Height = new GridLength(0);
        }

        /// <summary>
        /// загружаем данными грид и основными данные выбранный участок в лабл
        /// </summary>
        public void LoadDataGridSelectPipe()
        {
            try
            {

           
           if (Model.SelectKeyRegion !=null)
            {

                Model.GridPipeListFill();
                //PipeViewModel PipeModel = new PipeViewModel(); //модель для встроенного контрола рисования трубы
                //Model.PipeModel = PipeModel; GridPipeListOnSelectPipeList
                Model.PipeModel.Init(false, Model.GridPipeList,null, true);
              
                pipeControl.Init();
                pipeControl.Draw();
            }
            else
            {
                Model.Report("LoadDataGridSelectPipe   Нет данных!!!");
               
            }
            }
            catch (Exception e)
            {
                Model.Report("LoadDataGridSelectPipe" + e);
                throw;
            }
            
        }

        private void rgrvSelectPipe_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            string km = "";
            string numberPipe = "";

            try
            {

           
            if (!_isIgnoreGridSelection && rgrvSelectPipe.SelectedItems != null && rgrvSelectPipe.SelectedItems.Count > 0)
            {
                object item = rgrvSelectPipe.SelectedItems[rgrvSelectPipe.SelectedItems.Count - 1];
                if (item is GridPipe)
                {
                    GridPipe gp = item as GridPipe;
                    int index = Model.PipeModel.GetIndexByKey(gp.KEYPIPE);

                    
                    //Debug.WriteLine(gp.KEYPIPE + " " + gp.NUMBERPIPE);
                    if (index != -1)
                    {
                        Model.PipeModel.CentralIndex = index;
                    }
                }

                foreach (GridPipe itemgp in rgrvSelectPipe.SelectedItems)
                {
                    km = km + itemgp.KM + ", ";
                    numberPipe = numberPipe + itemgp.NUMBERPIPE + ", ";
                }

                km = km.Substring(0, km.Length - 2);
                numberPipe = numberPipe.Substring(0, numberPipe.Length - 2);
            }
            }
            catch (Exception ee)
            {
                Model.Report("rgrvSelectPipe_SelectionChanged " + ee);
                throw;
            }
        }


        /// <summary>
        /// Переходим на создание акта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateAct_Click(object sender, RoutedEventArgs e)
        {
           
            string km = "";
            string numberPipe = "";

            try
            {

         
            if (rgrvSelectPipe.SelectedItems.Count != 0)
            {
                
                Model.SelectPipeList.Clear();

                foreach (GridPipe item in rgrvSelectPipe.SelectedItems)
                {
                    Model.SelectPipeList.Add(new SelectPipeOnGrid(item.KEYPIPE, item.KM,
                                                                  item.NUMBERPIPE, item.ANGLESHOV,
                                                                  item.LENGHT, item.DEPTHPIPE,
                                                                  item.NUMBERDEFECT, item.NUMBERAKT, item.TUBERADIUS, "",""));

                }


                for (int i = 0; i < Model.SelectPipeList.Count; i++)
                {
                    km = km + Model.SelectPipeList[i].KmPipe + ", ";
                    numberPipe = numberPipe + Model.SelectPipeList[i].NumberPipe + ", ";

                }

                km = km.Substring(0, km.Length - 2);
                numberPipe = numberPipe.Substring(0, numberPipe.Length - 2);

                Model.SelectPipeKm = km;
                Model.SelectPipeNumberPipe = numberPipe;



                //наполнение данными для выбранных труб
                Model.GridPipeListOnSelectPipeFill();
                CreateActShurfovaniya createActShurfovaniya = new CreateActShurfovaniya();
                Model.NameFormWindow = MainViewModel.NameFormWindows.SelectPipe;
                createActShurfovaniya.ActwihtRegion = true;
                createActShurfovaniya.Show();
                Application.Current.MainWindow = createActShurfovaniya;
                this.Close();
            }
            else
            {
                MessageBox.Show("Не выбрана труба для шурфования!");
            }
            }
            catch (Exception ee)
            {
                Model.Report("btnCreateAct_Click " + ee);
                throw;
            }
        }

        private void BtnFindPipe_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {

            if (tbNumberPipe.Text != "")
            {
                foreach (var item in rgrvSelectPipe.Items)
                {
                    if (item is GridPipe)
                    {
                        var gp = item as GridPipe;
                        if (string.Equals(gp.NUMBERPIPE, tbNumberPipe.Text))
                        {
                            rgrvSelectPipe.SelectedItem = item;
                            rgrvSelectPipe.ScrollIntoView(item);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (tbKmPipe.Text != "")
                {
                    foreach (var item in rgrvSelectPipe.Items)
                    {
                        if (item is GridPipe)
                        {
                            var gp = item as GridPipe;
                            if (string.Equals(gp.KM, tbKmPipe.Text.Replace(",", ".")))
                            {
                                rgrvSelectPipe.SelectedItem = item;
                                rgrvSelectPipe.ScrollIntoView(item);
                                return;
                            }
                        }
                    }
                }
            }

            }
            catch (Exception ee)
            {
                Model.Report("BtnFindPipe_OnClick " + ee);
                throw;
            }

        }

    }
}

//if (!string.IsNullOrEmpty(KeyRegion))
//{
//    //наполняем таблицу данными(трубы)
//    DataTable dt = CBLL.GetPipeOnRegion(KeyRegion);
//    //напоняем данными встроенный контрол отображения труб - данные одни и те же
//}