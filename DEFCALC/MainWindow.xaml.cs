using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using DEFCALC.DataModel;
using DEFCALC.Localization;
using DrawPipe.DataModel;
using Microsoft.Win32;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker bw = new BackgroundWorker();//паралельный поток

        public MainWindow()
        {
            LocalizationManager.DefaultResourceManager = ControlResources.ResourceManager;
            InitializeComponent();
            DataContext = App.Model;
            

        }
        //вью модель
        MainViewModel Model { get { return App.Model; } }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;


            Model.GetVisibleReport();

            //загружаем из xml сохраненные данные

            FileStream fs = new FileStream(@"SavePipe.xml", FileMode.Open, FileAccess.Read);
            if (fs.Length != 0)
            {
                SavePipe sp = Helper.Deserialize(fs, typeof(SavePipe)) as SavePipe;
                fs.Close();

                // загружаем переменные

                Model.SelectMgKey = sp.KeyMg;
                Model.SelectMgName = sp.NameMg;
                Model.SelectNitKey = sp.KeyNit;
                Model.SelectNitName = sp.NameNit;
                Model.SelectedKmBegin = sp.KmBegin;
                Model.SelectedKmEnd = sp.KmEnd;
                Model.SelectKeyRegion = sp.KeyRegion;
                Model.SelectRegionName = sp.NameRegion;
                Model.KeyAkt = sp.KeyAkt;
                Model.NumberAct = sp.NumberAkt;
                Model.DataAct = sp.DataAkt;
                Model.SelectPipeKm = sp.KmAkt;
                Model.SelectPipeNumberPipe = sp.NumberPipe;
                Model.ActwihtRegion = sp.ActwihtRegion;

                
                if (!string.IsNullOrEmpty(Model.KeyAkt))
                {
                   
                }

                if (sp.KeyNit == "")
                {
                    // btnSelectPipe.IsEnabled = false;
                }

                if (string.IsNullOrEmpty(sp.KeyAkt))
                {
                    //btnCreateActShurfovaniyaPageOper.IsEnabled = false;
                    //btnListOfActs.IsEnabled = false;
                    //btnCreateActShurfovaniyaPageOperRegion.IsEnabled = false;

                }
            }
            else
            {
                fs.Close();
                //btnSelectPipe.IsEnabled = false;
                //btnCreateActShurfovaniyaPageOper.IsEnabled = false;
            }

            LoadListAct();

        }


        /// <summary>
        /// открывается форма - выбор участка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectRegion_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Сообщение: " + CBLL.TestSqlLiteBbConnection(string.Format(@"{0}\{1}", (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)), ConfigurationManager.AppSettings["SqlLiteDataBaseName"])));

            SelectionRegion selectRegion = new SelectionRegion();
            //selectRegion.PrevWindow = this;
            Model.NameFormWindow = MainViewModel.NameFormWindows.MainWindow;

            selectRegion.Show();
            Application.Current.MainWindow = selectRegion;
            this.Close();

        }

        /// <summary>
        /// открывается форма выбора трубы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectPipe_Click(object sender, RoutedEventArgs e)
        {
           if (Model.SelectKeyRegion =="")
           {
               MessageBox.Show("Нет данных по участку. Выберите участок!");
           }
           else
           {
               SelectPipe selectPipe = new SelectPipe();
               Model.NameFormWindow = MainViewModel.NameFormWindows.MainWindow;
               selectPipe.Show();
               Application.Current.MainWindow = selectPipe;
               this.Close();   
           }
            
        }


        /// <summary>
        /// создать новый акт (пустой)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateActShurfovaniyaPageOperRegion_Click(object sender, RoutedEventArgs e)
        {
            Model.SelectRegion = null;
            Model.SelectPipeKm = null;
            Model.SelectPipeNumberPipe = null;
            Model.SelectPipeList.Clear();
            CreateActShurfovaniya createActShurfovaniya = new CreateActShurfovaniya();
            createActShurfovaniya.ActwihtRegion = false;
            createActShurfovaniya.Show();
            Application.Current.MainWindow = createActShurfovaniya;
            this.Close();
        }
       
        public void LoadListAct()
        {
            try
            {
                Model.GridListAct();
               
            }
            catch (Exception e)
            {

                Model.Report("Model.GridListAct()" + e.ToString());
            }


            if (rgrvListAct.Items.Count == 0)
            {
                btnSelectPipe.IsEnabled = false;
                btnOpenActShurfovaniya.IsEnabled = false;
                Model.SelectRegionName = "";
                Model.SelectPipeNumberPipe = "";
                Model.NumberAct = "";

                //очищаем переменные
               
                SavePipe sp = new SavePipe();

                sp.NameMg = "";
                sp.KeyNit = "";
                sp.NameNit = "";
                sp.KmBegin = "";
                sp.KmEnd = "";
                sp.NameRegion = Model.SelectedgridAct.PLACEATATE = "";
                sp.KeyRegion = Model.SelectedgridAct.KEYREGION ="";
                sp.KeyAkt = Model.SelectedgridAct.KEYACT="";
                sp.NumberAkt = Model.SelectedgridAct.NUMBERACT="";
                sp.DataAkt = Model.SelectedgridAct.DATAACT="";
                sp.KmAkt = Model.SelectedgridAct.KMPIPELIST="";
                sp.NumberPipe = Model.SelectedgridAct.NUMBERPIPELIST="";



                FileStream fs = new FileStream(@"SavePipe.xml", FileMode.Create, FileAccess.Write);
                Helper.Serialize(fs, sp);
                fs.Close();

            }
            else
            {
                btnOpenActShurfovaniya.IsEnabled = true;
            }

            foreach (var item in rgrvListAct.Items)
            {
                if (item is GridAct)
                {
                    var gd = item as GridAct;
                    if (string.Equals(gd.KEYACT, Model.KeyAkt))
                    {
                      
                       // rgrvListAct.SelectedItem = item;
                       // rgrvListAct.CurrentItem = item;
                        rgrvListAct.ScrollIntoView(item);

                        break;

                    }
                }
            }
          
            
        }

        private void rgrvListAct_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {

            //выбираем помеченные акты дя импорта в иас
          

        }

        private void RgrvListAct_OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            //при загрузке данных раскращиваем редактируемый акт в зеленый цвет!
            var gr = e.Row as GridViewRow;

            if (gr != null)
            {
                var gr_item = ((((RadRowItem)(gr)).Item));
                dynamic gr_d = gr_item;


                if (gr_d.GetType().GetProperty("ISEDITED") != null)
                {
                    string result = gr_d.GetType().GetProperty("ISEDITED").GetValue(gr_d, null).ToString();
                    if ((result == "1") || (result == "2"))
                    {
                        gr.Foreground = new SolidColorBrush(Color.FromArgb(240, 128, 128, 128));
                        // gr.Foreground = new  SolidColorBrush(Color.FromArgb(150, 143, 188, 143));
                    }
                }


                if (gr_d.GetType().GetProperty("KEYACT") != null )
                {
                    
                    string result = gr_d.GetType().GetProperty("KEYACT").GetValue(gr_d, null).ToString();
                    if (result == Model.KeyAkt)
                    {

                        //Binding colorBinding = new Binding("Price") { Converter = new PriceToColorConverter() };
                        //e.Row.Cells[1].SetBinding(GridViewCell.ForegroundProperty, colorBinding);
                        //e.Row.Cells[5].Foreground = new SolidColorBrush(Colors.GreenYellow);
                        
                        gr.Foreground = new SolidColorBrush(Colors.Green);      
                    }
                    else
                    {
                       // gr.Foreground = new SolidColorBrush(Colors.Black);      
                    }
                }

            }


            

        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           
            //если готова строка експорта то...
            if ("CreateExportDefectAkt".Equals(e.PropertyName))
            {
                try
                {

                    string jsonExport = JsonHelper.JsonSerializer<ExportDefectAkt.Export>(Model.ExportDefectAktModel);
                    jsonExport = "{\"DATA\":[" + jsonExport + "]}";


                    SaveFileDialog dialog = new SaveFileDialog();
                    DateTime d1 = DateTime.Now;
                    string sdt = d1.ToString("dd.MM.yyyy");
                    dialog.FileName = "offline_" + sdt + ".ldb";
                   // dialog.DefaultExt = "offline_"+sdt+".json";
                    dialog.Filter = ".ldb Files|*.ldb";

                    dialog.FilterIndex = 1;


                    bool? saveClicked = dialog.ShowDialog();

                    if (saveClicked == true)
                    {

                        using (FileStream stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(stream))
                            {
                                writer.Write(jsonExport);
                                writer.Close();
                            }
                        }
                    }
                    indicator.IsBusy = false;
                    MessageBox.Show("Данные выгружены в файл.");
                }
                catch (Exception ee)
                {
                    Model.Report("CreateExportDefectAkt " + ee);
                    MessageBox.Show("Ошибка выгрузки данных в файл.");
                }
            }
            if ("VisibleReport".Equals(e.PropertyName))
            {
                if (Model.VisibleReport == 0)
                    gMainFormGrid.RowDefinitions[5].Height = new GridLength(0);
            }
        }

 
        /// <summary>
        /// кнопка экспорт в json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExportData_OnClick(object sender, RoutedEventArgs e)
        {
            string selectAkt = "";
          
               if (rgrvListAct.SelectedItems.Count != 0)
               {
                   indicator.IsBusy = true;
                   foreach (GridAct item in rgrvListAct.SelectedItems)
                   {
                       if (item.ISEDITED != "2")
                       {
                           selectAkt = selectAkt + item.KEYACT + ", ";    
                       }
                       
                   }

                   selectAkt = selectAkt.Substring(0, selectAkt.Length - 2);
                   Model.FileExportToIAS(selectAkt);    
               }
               else
               {
                   MessageBox.Show("Акты для экспорта не выбраны ");  
               }

            
        }

        /// <summary>
        /// удаление одного или несколько выбранных актов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteAkt_OnClick(object sender, RoutedEventArgs e)
        {
            string selectAkt = "";
            try
            {
                
                if (rgrvListAct.SelectedItems.Count != 0)
                {
                    if (Model.ShowConfirmDialog("Удалить выбранные акты?  "))
                    {
                        foreach (GridAct item in rgrvListAct.SelectedItems)
                        {
                            selectAkt = selectAkt + item.KEYACT + ", ";
                        }

                        selectAkt = selectAkt.Substring(0, selectAkt.Length - 2);
                        string resuleDelete = CBLL.DeleteOnSelectkeyAkt(selectAkt);
                        LoadListAct();
                    }
                   
                }
                else
                {
                    MessageBox.Show("Акты для экспорта не выбраны ");
                }


            }
            catch (Exception ee)
            {
                
                Model.Report("Ошибка удаление акта " + ee);
                MessageBox.Show("Ошибка удаления акта "+ee);
            }
        }


        /// <summary>
        /// Клик на кнопке "Открыть акт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenActShurfovaniya_Click(object sender, RoutedEventArgs e)
        {
            if (rgrvListAct.SelectedItems.Count != 0)
            {
                OpenAkt(Model);
            }
            else
            {
                MessageBox.Show("Не выбран акт шурфования!");
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //расчет дефектов на трубах этого акта
            //расчет дефектов

            CalcDefect cd = new CalcDefect();
            cd.InitCalc();
            cd.CalcColony_Group();

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                indicator.IsBusy = false;

            }
            else
            {
                indicator.IsBusy = false;
                MessageBox.Show("Ошибка расчета дефектов");
            }

            EditAct editAct = new EditAct();
            editAct.Show();
            Application.Current.MainWindow = editAct;
            this.Close();



            ////http://msdn.microsoft.com/ru-ru/library/ms229675.aspx
            //// http://social.msdn.microsoft.com/Forums/ru-RU/aspnetru/thread/ad892d3e-0db7-44d5-ad06-59a55750be2c
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

        }



        private void BtnManagerBD_OnClick(object sender, RoutedEventArgs e)
        {
            ManagerBase managerBD = new ManagerBase();
            
            //Model.NameFormWindow = MainViewModel.NameFormWindows.MainWindow;

            managerBD.Show();
            Application.Current.MainWindow = managerBD;
            this.Close();
        }


        private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
        }

        private void rgrvListAct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (rgrvListAct.SelectedItems.Count != 0)
            {
                OpenAkt(Model);

            }
        }

        private void OpenAkt(MainViewModel Model)
        {
            MainViewModel model = Model;

            model.SignatureAkt = model.SelectedgridAct.ISEDITED;
            model.KeyAkt = model.SelectedgridAct.KEYACT;
            //наполняем данными трубу на которой находится шурф
            model.GetSelectPipeOnkeyAct();
            //сохраняем в моделе ключ акта - в нем вызываются все дефекты
            model.GetKeyAkt = model.KeyAkt;

            model.NumberAct = model.SelectedgridAct.NUMBERACT;
            model.DataAct = model.SelectedgridAct.DATAACT;
            //сохраняем в xml данные по акту и с выбором мг, нитки и т.д.
            SavePipe sp = new SavePipe();

            sp.NameMg = "";
            sp.KeyNit = "";
            sp.NameNit = "";
            sp.KmBegin = "";
            sp.KmEnd = "";
            sp.NameRegion = model.SelectedgridAct.PLACEATATE;
            sp.KeyRegion = model.SelectedgridAct.KEYREGION;
            sp.KeyAkt = model.SelectedgridAct.KEYACT;
            sp.NumberAkt = model.SelectedgridAct.NUMBERACT;
            sp.DataAkt = model.SelectedgridAct.DATAACT;
            sp.KmAkt = model.SelectedgridAct.KMPIPELIST;
            sp.NumberPipe = model.SelectedgridAct.NUMBERPIPELIST;



            FileStream fs = new FileStream(@"SavePipe.xml", FileMode.Create, FileAccess.Write);
            Helper.Serialize(fs, sp);
            fs.Close();


            //если акт загружен из базы - расчитываем дефекты и группировки (один разБ если не расчитаны)
            if (model.SignatureAkt == "2")
            {
                indicator.BusyContent = "Расчет дефектов";
                indicator.IsBusy = true;

                DataTable dt = CBLL.GetDefectPipe(model.SelectPipeList);
                //выбираем все дефекты(ключи)
                string keyDefect = "";

                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in dt.AsEnumerable()
                        select t;
                    foreach (DataRow p in query)
                    {
                        keyDefect = keyDefect + (p.Field<object>("nDefect_key") != null
                                                     ? p.Field<object>("nDefect_key").ToString()
                                                     : "<не указано>") + ", ";
                    }
                }

                keyDefect = keyDefect.Substring(0, keyDefect.Length - 2);
                string countCalcDefect = CBLL.GetountCalcDrfrct(keyDefect);

                //расчитываем дефекты только 1 раз
                if (countCalcDefect != "0")
                {
                    bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                    bw.RunWorkerCompleted += (bw_RunWorkerCompleted);
                    bw.ProgressChanged += (bw_ProgressChanged);

                    bw.WorkerReportsProgress = true;
                    bw.WorkerSupportsCancellation = true;
                    bw.RunWorkerAsync();
                }
                else
                {
                    EditAct editAct = new EditAct();
                    editAct.Show();
                    Application.Current.MainWindow = editAct;
                    this.Close();
                }

            }
            else
            {
                EditAct editAct = new EditAct();
                editAct.Show();
                Application.Current.MainWindow = editAct;
                this.Close();
            }
        }
    }
}

//http://stackoverflow.com/questions/14103539/radgridview-change-text-color
//http://www.telerik.com/community/forums/aspnet-ajax/grid/radgrid-rows-color.aspx
//http://www.telerik.com/community/forums/wpf/gridview/foreground-color-of-radgridview-not-changing.aspx
//http://www.telerik.com/community/forums/wpf/gridview/setting-radgridview-row-background-color-based-on-the-value-of-a-column-in-the-row.aspx

