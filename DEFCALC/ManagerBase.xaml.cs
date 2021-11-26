using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DEFCALC.DataModel;
using Microsoft.Win32;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for ManagerBase.xaml
    /// </summary>
    public partial class ManagerBase : Window
    {

        private CreateTree createTree = new CreateTree();
        private RadTreeViewItem _currentItem;
        private CreateTree _createTreeCurrentItem;
        private string lineReadFile = "";
        private ImportAllBaza.ImportIas importiasAnaliz = new ImportAllBaza.ImportIas();
        private RadTreeViewItem _selectedTree;
        private string selectKeyOnTree = "";
        private string nameFileRezervCopy; //путь к файлу резервного копирования
        private string name; //сам файл резервного копирования

        private BackgroundWorker bw = new BackgroundWorker();//паралельный поток

        public ManagerBase()
        {
            InitializeComponent();

        }

        MainViewModel Model { get { return App.Model; } }


        private void ManagerBase_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string txt = "После проведения импорта все текущие данные из базы модуля будут удалены. Во избежании потерь данных экспортируйте все заполненные акты шурфования. ";
                tbtxt.Text = txt;
                tbtxtRezerv.Text = txt;

                string line = "";
                if (File.Exists(@"dataCopyBaza.txt"))
                {

                    using (StreamReader reader = new StreamReader(@"dataCopyBaza.txt"))
                    {
                        line = reader.ReadLine();
                    }

                }
                if (!String.IsNullOrEmpty(line))
                {
                    txtDataCopy.Text = line;
                }
                else
                {
                    txtDataCopy.Text = "Резервное копирование системы не проводилось";
                }

            }
            catch (Exception ee)
            {
                txtDataCopy.Text = "Резервное копирование системы не проводилось";

            }

            DataContext = App.Model;
            CreateOneitemsTree();

        }


        //построение первой ветки дерева
        private void CreateOneitemsTree()
        {
            List<CreateTree> list = new List<CreateTree>();
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("0"), Byte.Parse("0"), Byte.Parse("0")));
            FontWeight fw = FontWeights.Bold;


            createTree = new CreateTree
            {
                KEY = "#1",
                NAME = "Магистральный газопровод",
                Brush = brush,
                Weight = fw,
                KeyParent = "0"

            };
            list.Add(createTree);
            //createTree = new CreateTree
            //{
            //    KEY = "#10",
            //    NAME = "Трубы, не привязанные к МТ",
            //    Brush = brush,
            //    Weight = fw,
            //    KeyParent = "0"

            //};


           // list.Add(createTree);
            radTreeView.ItemsSource = null;
            radTreeView.ItemsSource = list;
        }

        //динамическое построение ветки дерева
        private void RadTreeView_OnLoadOnDemand(object sender, RadRoutedEventArgs e)
        {
            bool isLoad = true;
            _currentItem = e.OriginalSource as RadTreeViewItem;
            _createTreeCurrentItem = _currentItem.DataContext as CreateTree;
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("0"), Byte.Parse("0"), Byte.Parse("0")));
            FontWeight fw = FontWeights.Normal;

            FontWeight fw1 = FontWeights.Bold;



            CreateTree ct = _currentItem.Item as CreateTree;

            if (ct != null)
            {
                //определяем какой символ и сколько символов
                string[] arr = ct.KEY.Split('#');

                switch (arr.Length)
                {
                    case 2:
                        {
                            //открываем ветку магистральный газопровод
                            if (arr[1] == "1")
                            {

                                //выбираем все газопроводы
                                DataTable dt = CBLL.GetAllMG();
                                if (dt.Rows.Count != 0)
                                {
                                    if (_currentItem.Items.Count > 0)
                                    {
                                        _currentItem.Items.Clear();
                                        isLoad = true;
                                    }


                                    IEnumerable<DataRow> query =
                           from t in dt.AsEnumerable()
                           select t;

                                    foreach (DataRow p in query)
                                    {

                                        string key = (p.Field<object>("Key_MG") != null
                                                         ? p.Field<object>("Key_MG").ToString()
                                                         : "<не указано>");
                                        key = "#1#" + key;
                                        CreateTree child = new CreateTree
                                        {
                                            KEY = key,
                                            NAME =
                                                (p.Field<object>("Name") != null
                                                     ? p.Field<object>("Name").ToString()
                                                     : "<не указано>"),
                                            Brush = brush,
                                            Weight = fw,
                                            KeyParent = "1"

                                        };


                                        _createTreeCurrentItem.Children.Add(child);
                                    }
                                }
                                else
                                {
                                    isLoad = false;
                                }

                                _currentItem.IsLoadOnDemandEnabled = isLoad;
                                _currentItem.IsExpanded = isLoad;


                            }
                            else
                            {
                                //открываем ветку нитка
                                if (arr[1] == "2")
                                {
                                    //выбираем все нити по ключу мг
                                    string keyMg = ct.KeyParent;
                                    DataTable dt = CBLL.GetNitOnMg(keyMg);
                                    if (dt.Rows.Count != 0)
                                    {
                                        isLoad = true;
                                        if (_currentItem.Items.Count > 0)
                                        {
                                            _currentItem.Items.Clear();
                                        }

                                        IEnumerable<DataRow> query =
                                            from t in dt.AsEnumerable()
                                            select t;

                                        foreach (DataRow p in query)
                                        {

                                            string key = (p.Field<object>("Key_thread") != null
                                                              ? p.Field<object>("Key_thread").ToString()
                                                              : "<не указано>");
                                            key = "#2#" + key;
                                            CreateTree child = new CreateTree
                                                {
                                                    KEY = key,
                                                    NAME =
                                                        (p.Field<object>("NameThread") != null
                                                             ? p.Field<object>("NameThread").ToString()
                                                             : "<не указано>"),
                                                    Brush = brush,
                                                    Weight = fw,
                                                    KeyParent = "2"

                                                };


                                            _createTreeCurrentItem.Children.Add(child);
                                        }
                                    }
                                    else
                                    {
                                        isLoad = false;
                                    }

                                    _currentItem.IsLoadOnDemandEnabled = isLoad;
                                    _currentItem.IsExpanded = isLoad;

                                }
                                else
                                {
                                    // открываем ветку участок
                                    if (arr[1] == "3")
                                    {
                                        //выбираем все участки по ключу нити 
                                        string keyNit = ct.KeyParent;
                                        DataTable dt = CBLL.GetAllregionsOnNit(keyNit);
                                        if (dt.Rows.Count != 0)
                                        {
                                            isLoad = false;
                                            if (_currentItem.Items.Count > 0)
                                            {
                                                _currentItem.Items.Clear();
                                            }

                                            IEnumerable<DataRow> query =
                                                from t in dt.AsEnumerable()
                                                select t;

                                            foreach (DataRow p in query)
                                            {

                                                string key = (p.Field<object>("nkey_region") != null
                                                                  ? p.Field<object>("nkey_region").ToString()
                                                                  : "<не указано>");
                                                key = "#3#" + key;
                                                CreateTree child = new CreateTree
                                                {
                                                    KEY = key,
                                                    NAME =
                                                        (p.Field<object>("cName") != null
                                                             ? p.Field<object>("cName").ToString()
                                                             : "<не указано>"),
                                                    Brush = brush,
                                                    Weight = fw,
                                                    KeyParent = "3"

                                                };


                                                _createTreeCurrentItem.Children.Add(child);
                                               
                                                
                                                
                                            }
                                        }
                                        else
                                        {
                                            isLoad = false;
                                        }
                                        //не получается последнюю ветку раскрыть и сделать без значков - т.к. isload вешается на родителя
                                        //- нужно все дерево создавать руками без лоадондеманд
                                        _currentItem.IsLoadingOnDemand = isLoad;
                                        _currentItem.IsLoadOnDemandEnabled = isLoad;
                                        _currentItem.IsExpanded = true;
                                       


                                    }
                                    else
                                    {
                                        string keyAkt = "";
                                        string numberPipe = "";
                                        string numberAkt = "";
                                        //открываем ветку трубы не привязанные к МТ
                                        if (arr[1] == "10")
                                        {

                                            DataTable dt = CBLL.GetAllAktOnRegionIsNull();
                                            if (dt.Rows.Count != 0)
                                            {
                                                isLoad = true;
                                                if (_currentItem.Items.Count > 0)
                                                {
                                                    _currentItem.Items.Clear();
                                                }

                                                IEnumerable<DataRow> query =
                                                    from t in dt.AsEnumerable()
                                                    select t;

                                                foreach (DataRow p in query)
                                                {
                                                    keyAkt = (p.Field<object>("nShurf_key") != null
                                                                  ? p.Field<object>("nShurf_key").ToString()
                                                                  : "");
                                                    numberAkt = (!String.IsNullOrEmpty(p.Field<object>("cNumberAkt").ToString()) ? p.Field<object>("cNumberAkt").ToString() : "<не определено>");
                                                    if (keyAkt != "")
                                                    {
                                                        DataTable dt1 = CBLL.GetAllPipe_KeyAktOnRegionIsNull(keyAkt);
                                                        if (dt1.Rows.Count != 0)
                                                        {
                                                            IEnumerable<DataRow> query1 =
                                                              from t in dt1.AsEnumerable()
                                                              select t;

                                                            foreach (DataRow p1 in query1)
                                                            {
                                                                try
                                                                {
                                                                    numberPipe = numberPipe + (!String.IsNullOrEmpty(p1.Field<object>("cNumber_pipe").ToString()) ? p1.Field<object>("cNumber_pipe").ToString()
                                                                 : "<не определено>") + ", ";
                                                                }
                                                                catch (Exception)
                                                                {

                                                                    numberPipe = numberPipe + "<не определено>" + ", ";
                                                                }

                                                            }

                                                            numberPipe = numberPipe.Substring(0, numberPipe.Length - 2);
                                                        }


                                                    }

                                                    keyAkt = "#11#" + keyAkt;
                                                    CreateTree child = new CreateTree
                                                    {
                                                        KEY = keyAkt,
                                                        NAME = "№ трубы: " + numberPipe + "       № акта: " + numberAkt,
                                                        Brush = brush,
                                                        Weight = fw,
                                                        KeyParent = "10"

                                                    };

                                                    numberPipe = "";
                                                    numberAkt = "";
                                                    keyAkt = "";
                                                    _createTreeCurrentItem.Children.Add(child);


                                                }
                                            }


                                        }
                                        _currentItem.IsLoadOnDemandEnabled = isLoad;
                                        _currentItem.IsExpanded = isLoad;
                                    }

                                }
                            }

                            break;
                        }
                    case 3:
                        {
                            if (arr[1] == "1")
                            {
                                CreateTree child = new CreateTree
                                    {
                                        KEY = "#2",
                                        NAME = "Нитка",
                                        Brush = brush,
                                        Weight = fw1,
                                        KeyParent = arr[2]
                                    };
                                _createTreeCurrentItem.Children.Add(child);
                                _currentItem.IsLoadOnDemandEnabled = true;
                                _currentItem.IsExpanded = true;
                            }
                            else
                            {
                                if (arr[1] == "2")
                                {
                                    CreateTree child = new CreateTree
                                    {
                                        KEY = "#3",
                                        NAME = "Участок ВТД",
                                        Brush = brush,
                                        Weight = fw1,
                                        KeyParent = arr[2]
                                    };
                                    _createTreeCurrentItem.Children.Add(child);
                                    _currentItem.IsLoadOnDemandEnabled = true;
                                    _currentItem.IsExpanded = true;
                                }
                                else
                                {
                                    if (arr[1] == "3")
                                    {
                                        _currentItem.IsLoadingOnDemand = false;
                                        _currentItem.IsLoadOnDemandEnabled = false;
                                        _currentItem.IsExpanded = false;
                                    }
                                }
                            }
                            break;
                        }
                }

                //_currentItem.IsLoadOnDemandEnabled = true;
                //_currentItem.IsExpanded = true;
                // _currentItem.IsLoadingOnDemand = false;
            }
            else
            {
                _currentItem.IsLoadingOnDemand = false;
                _currentItem.IsLoadOnDemandEnabled = false;
                _currentItem.IsExpanded = false;
            }
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            //Переходим на главную страницу проекта
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.MainWindow = mainWindow;
            this.Close();
        }

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            CreateOneitemsTree();

        }
        /// <summary>
        /// импорт данных из иаса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddFile_OnClick(object sender, RoutedEventArgs e)
        {
            string nameFile = "не определено";
            try
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = ".ldb",
                    Filter = "Документ (.ldb)|*.ldb"
                };
                Nullable<bool> result = dlg.ShowDialog();



                if (result == true)
                {
                    nameFile = dlg.FileName;
                    txtReport.Text = "Идет анализ файла...";
                    using (FileStream stream = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding(1251)))
                        {

                            while (reader.Peek() != -1)
                            {
                                var tt = reader.CurrentEncoding;
                                lineReadFile = lineReadFile + reader.ReadLine(); // Line = sr.ReadToEnd();

                            }

                            reader.Close();
                        }
                    }

                    //запись файла в структуру
                    importiasAnaliz = JsonHelper.JsonDeserialize<ImportAllBaza.ImportIas>(lineReadFile);
                    if ((importiasAnaliz.MG == null) || (importiasAnaliz.THREAD == null) || (importiasAnaliz.THREAD == null))
                    {
                        txtReport.Foreground = new SolidColorBrush(Colors.Red);
                        txtReport.Text = "Анализ файла завершен c ошибками! ";
                        txtNameFile.Text = dlg.FileName;
                    }
                    else
                    {
                        txtReport.Text = "Анализ файла завершен ";
                        txtNameFile.Text = dlg.FileName;
                        btnImportFile.IsEnabled = true;
                    }
                }

            }
            catch (Exception ee)
            {
                txtReport.Foreground = new SolidColorBrush(Colors.Red);
                txtReport.Text = " Анализ файла - Ошибка чтения ";
                txtNameFile.Text = nameFile;
            }


        }
        /// <summary>
        /// кнопка -загрузить в базу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImportFile_OnClick(object sender, RoutedEventArgs e)
        {

            indicator.IsBusy = true;
            indicator.BusyContent = "Импорт  данных...";

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += (bw_RunWorkerCompleted);
            bw.ProgressChanged += (bw_ProgressChanged);

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();
            //FileImportOnOfflineModule(lineReadFile);

            SavePipe sp = new SavePipe();

            sp = new SavePipe();
            sp.ActwihtRegion = true;
            sp.KeyMg = "";
            sp.NameMg = "";
            sp.KeyNit = "";
            sp.NameNit = "";
            sp.KmBegin = "";
            sp.KmEnd = "";
            sp.NameRegion = "";
            sp.KeyRegion = "";
            sp.KeyAkt = "";
            sp.NumberAkt = "";
            sp.DataAkt = "";
            sp.KmAkt = "";
            sp.NumberPipe = "";

            Model.SelectPipeKm = "";
            Model.SelectPipeNumberPipe = "";

            FileStream fs = new FileStream(@"SavePipe.xml", FileMode.Create, FileAccess.Write);
            Helper.Serialize(fs, sp);
            fs.Close();
           
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            FileImportOnOfflineModule(lineReadFile);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                indicator.IsBusy = false;
                CreateOneitemsTree();
            }
            else
            {
                indicator.IsBusy = false;
                MessageBox.Show("Ошибка импорта данных");
            }
            ////http://msdn.microsoft.com/ru-ru/library/ms229675.aspx
            //// http://social.msdn.microsoft.com/Forums/ru-RU/aspnetru/thread/ad892d3e-0db7-44d5-ad06-59a55750be2c
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
          
        }
        /// <summary>
        /// загрузка данных в базу sqlight
        /// </summary>

        public void FileImportOnOfflineModule(string lineReadFile)
        {
            List<string> allMgBaza = new List<string>();
            try
            {
                
               
                ////вначале чистим всю базу
                string resultClearDB = CBLL.ClearBasa();
                if (resultClearDB != "")
                {
                   
                    MessageBox.Show(resultClearDB);
                    return;
                }

                else
                {

                    
                   // MessageBox.Show("Очистка базы прошла успешно.");

                    
                    //  импортируем данные из прочитанного файла(json) во внутреннюю структуру
                    ImportAllBaza.ImportIas importias = new ImportAllBaza.ImportIas();
                    importias = JsonHelper.JsonDeserialize<ImportAllBaza.ImportIas>(lineReadFile);

                    //ИМПОРТ ДАННЫХ ИЗ СТРУКТУРЫ
                    //импорт словарей
                   
                    string insertnsi = AddImportNsi(importias);

                    //инсерт МГ
                   
                    string insertMg = AddImportMG(importias);
                    if (insertMg != "")
                    {
                        MessageBox.Show("Ошибка импорта МГ.");
                        return;
                    }
                    else
                    {
                        
                        string insertTreahead = AddImportTreahead(importias);
                        if (insertTreahead != "")
                        {
                            MessageBox.Show("Ошибка нити.");
                            return;
                        }
                        else
                        {
                           
                            string resultAdd = AddImportRegion(importias);
                            if (resultAdd == "")
                            {


                                MessageBox.Show("Успешно проимпортировались данные.");
                               
                            }

                           // indicator.IsBusy = false;
                        }

                    }

                }
            }
            catch (Exception ee)
            {

                MessageBox.Show("Ошибка импорта данных" + ee.ToString());
               
            }
            bw.CancelAsync();

        }
        //добавление нси
        private string AddImportNsi(ImportAllBaza.ImportIas importIas)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            string resultInsertNSI = "";
            //завод изготовитель
            for (int i = 0; i < importIas.NSI.FACTORYBUILDER.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(importIas.NSI.FACTORYBUILDER[i].NKEY))
                {
                    d["nFactory_builder_key"] = importIas.NSI.FACTORYBUILDER[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.FACTORYBUILDER[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.FACTORYBUILDER[i].CNAME + "'";

                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Factory_builder", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }
            //тип дефекта

            d = new Dictionary<string, string>();
            for (int i = 0; i < importIas.NSI.METALDEFECTTYPE.Count; i++)
            {

                if (!string.IsNullOrWhiteSpace(importIas.NSI.METALDEFECTTYPE[i].NKEY))
                {
                    d["nTypeDefect_Key"] = importIas.NSI.METALDEFECTTYPE[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.METALDEFECTTYPE[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.METALDEFECTTYPE[i].CNAME + "'";
                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_Type_Defect", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }

            //метод устранения дефекта

            //d = new Dictionary<string, string>();
            //for (int i = 0; i < importIas.NSI.METODUSTRDEFECT.Count; i++)
            //{

            //    if (!string.IsNullOrWhiteSpace(importIas.NSI.METODUSTRDEFECT[i].NKEY))
            //    {
            //        d["nMetod_Ustr_Def_Key"] = importIas.NSI.METODUSTRDEFECT[i].NKEY;

            //        if (!string.IsNullOrWhiteSpace(importIas.NSI.METODUSTRDEFECT[i].CNAME))
            //        {
            //            d["cName"] = "'" + importIas.NSI.METODUSTRDEFECT[i].CNAME + "'";
            //        }
            //    }

            //    if (d.Count != 0)
            //    {
            //        string resultAdd = CBLL.AddTable("Dict_Metod_ustr_def", d);
            //        if (resultAdd != "")
            //        {

            //            MessageBox.Show(resultAdd);
            //            resultInsertNSI = resultAdd;
            //            return resultInsertNSI;
            //        }
            //    }
            //}

            //Расчетный тип дефекта металла
            for (int i = 0; i < importIas.NSI.METALDEFECTCALCTYPE.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.METALDEFECTCALCTYPE[i].NKEY))
                {
                    d["nDict_Calc_Type_Key"] = importIas.NSI.METALDEFECTCALCTYPE[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.METALDEFECTCALCTYPE[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.METALDEFECTCALCTYPE[i].CNAME + "'";

                    }
                }
                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_Calc_Type", d);
                    if (resultAdd != "")
                    {
                      
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }
            //Расположение дефекта в металле
            for (int i = 0; i < importIas.NSI.LOCATIONDEFECT.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.LOCATIONDEFECT[i].NKEY))
                {
                    d["ndepth_pos_Key"] = importIas.NSI.LOCATIONDEFECT[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.LOCATIONDEFECT[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.LOCATIONDEFECT[i].CNAME + "'";

                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_Depth_pos", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }
            //Способ ремонта газопровода
            for (int i = 0; i < importIas.NSI.WAYREPAIRPIPELINE.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.WAYREPAIRPIPELINE[i].NKEY))
                {
                    d["nMetod_Ustr_Def_Key"] = importIas.NSI.WAYREPAIRPIPELINE[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.WAYREPAIRPIPELINE[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.WAYREPAIRPIPELINE[i].CNAME + "'";

                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_Metod_ustr_def", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }
            //Тип грунта
            for (int i = 0; i < importIas.NSI.TYPEOFSOIL.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.TYPEOFSOIL[i].NKEY))
                {
                    d["nDict_Character_grunt_key"] = importIas.NSI.TYPEOFSOIL[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.TYPEOFSOIL[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.TYPEOFSOIL[i].CNAME + "'";

                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_Character_grunt", d);
                    if (resultAdd != "")
                    {
                        
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }
            //Тип изоляции
            for (int i = 0; i < importIas.NSI.INSULATIONTYPE.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.INSULATIONTYPE[i].NKEY))
                {
                    d["ntype_izol_key"] = importIas.NSI.INSULATIONTYPE[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.INSULATIONTYPE[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.INSULATIONTYPE[i].CNAME + "'";

                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_type_izol", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }

            //Состояние защитного покрытия
            for (int i = 0; i < importIas.NSI.PROTECTCOATINGSTATE.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.PROTECTCOATINGSTATE[i].NKEY))
                {
                    d["nDict_Izol_state_key"] = importIas.NSI.PROTECTCOATINGSTATE[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.PROTECTCOATINGSTATE[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.PROTECTCOATINGSTATE[i].CNAME + "'";

                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_Izol_state", d);
                    if (resultAdd != "")
                    {
                        
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }
            //Наличие влаги под изоляцией
            for (int i = 0; i < importIas.NSI.MOISTUREUNDERINSUL.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.MOISTUREUNDERINSUL[i].NKEY))
                {
                    d["ndict_availabil_damp_key"] = importIas.NSI.MOISTUREUNDERINSUL[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.MOISTUREUNDERINSUL[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.MOISTUREUNDERINSUL[i].CNAME + "'";

                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_availabil_damp", d);
                    if (resultAdd != "")
                    {
                        
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }
            // Словарь марка стали 
            for (int i = 0; i < importIas.NSI.FEELGRAD.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.FEELGRAD[i].NKEY))
                {
                    d["nDict_feel_grade_key"] = importIas.NSI.FEELGRAD[i].NKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.FEELGRAD[i].CNAME))
                    {
                        d["cName"] = "'" + importIas.NSI.FEELGRAD[i].CNAME + "'";

                    }
                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Dict_feel_grade", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }
            // марка стали_завод
            for (int i = 0; i < importIas.NSI.EKVIVALENTMARKABUILDER.Count; i++)
            {
                d = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(importIas.NSI.EKVIVALENTMARKABUILDER[i].FACTORYBUILDERKEY))
                {
                    d["nFactory_builder_key"] = importIas.NSI.EKVIVALENTMARKABUILDER[i].FACTORYBUILDERKEY;

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.EKVIVALENTMARKABUILDER[i].FEELGRADEKEY))
                    {
                        d["nDict_feel_grade_key"] = importIas.NSI.EKVIVALENTMARKABUILDER[i].FEELGRADEKEY;

                    }

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.EKVIVALENTMARKABUILDER[i].KOEFLINEXPANSION))
                    {
                        d["nKoefLinExpansion"] = (importIas.NSI.EKVIVALENTMARKABUILDER[i].KOEFLINEXPANSION.ToString()).Replace(",", ".");

                    }

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.EKVIVALENTMARKABUILDER[i].KOEFPUASON))
                    {
                        d["nKoefPuanson"] = (importIas.NSI.EKVIVALENTMARKABUILDER[i].KOEFPUASON.ToString()).Replace(",", ".");

                    }

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.EKVIVALENTMARKABUILDER[i].MODULEJUNG))
                    {
                        d["nModulUng"] = (importIas.NSI.EKVIVALENTMARKABUILDER[i].MODULEJUNG.ToString()).Replace(",", ".");

                    }

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.EKVIVALENTMARKABUILDER[i].RANGEFLUD))
                    {
                        d["nRange_flud"] = (importIas.NSI.EKVIVALENTMARKABUILDER[i].RANGEFLUD.ToString()).Replace(",", ".");

                    }

                    if (!string.IsNullOrWhiteSpace(importIas.NSI.EKVIVALENTMARKABUILDER[i].RANGESTRANGHT))
                    {
                        d["nRange_stranght"] = (importIas.NSI.EKVIVALENTMARKABUILDER[i].RANGESTRANGHT.ToString()).Replace(",", ".");

                    }


                }

                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Ecvivalent_marka_builder", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        resultInsertNSI = resultAdd;
                        return resultInsertNSI;
                    }
                }
            }


            return resultInsertNSI;
        }




        /// <summary>
        /// поле-значение для Расположение дефекта в металле
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldAndValuesDict(ImportAllBaza.LOCATIONDEFECT ld)
        {

            Dictionary<string, string> d = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(ld.NKEY))
            {
                d["ndepth_pos_Key"] = ld.NKEY;

                if (!string.IsNullOrWhiteSpace(ld.CNAME))
                {
                    d["cName"] = "'" + ld.CNAME + "'";

                }
            }
            return d;
        }





        /// <summary>
        /// инсерт мг
        /// </summary>
        private string AddImportMG(ImportAllBaza.ImportIas importias)
        {
            
            string insertMg = "";
            for (int i = 0; i < importias.MG.Count; i++)
            {
                var d = GetFieldsAndValuesMG(importias.MG[i]);
                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("MG", d);
                    if (resultAdd != "")
                    {
                        
                        MessageBox.Show(resultAdd);
                        insertMg = resultAdd;
                        return insertMg;
                    }
                }

            }
            return insertMg;
        }
        /// <summary>
        /// инсерт нитки
        /// </summary>
        private string AddImportTreahead(ImportAllBaza.ImportIas importias)
        {
           
            string insertTreahead = "";

            for (int i1 = 0; i1 < importias.THREAD.Count; i1++)
            {
                var d = GetFieldsAndValuesTHREAD(importias.THREAD[i1]);
                if (d.Count != 0)
                {
                    string resultAdd = CBLL.AddTable("Thread", d);
                    if (resultAdd != "")
                    {
                        MessageBox.Show(resultAdd);
                        insertTreahead = resultAdd;
                        return insertTreahead;
                    }
                }
            }
            return insertTreahead;
        }

        //инсерт всего остального
        private string AddImportRegion(ImportAllBaza.ImportIas importias)
        {
            string resultAdd = "";
            //инсерт участка
            for (int i2 = 0; i2 < importias.SECTION.Count; i2++)
            {
                var d = GetFieldsAndValuesRegions_thread(importias.SECTION[i2]);
                if (d.Count != 0)
                {
                    resultAdd = CBLL.AddTable("Regions_thread", d);
                    if (resultAdd != "")
                    {
                        
                        MessageBox.Show(resultAdd);
                        return resultAdd;
                    }
                }
            }
            //инсерт труб
           
            int countPipe;
            countPipe = importias.PIPES.Count;

            for (int i3 = 0; i3 < importias.PIPES.Count; i3++)
            {
                var d = GetFieldsAndValuesPipe(importias.PIPES[i3]);
                if (d.Count != 0)
                {
                    resultAdd = CBLL.AddTable("Pipe", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        return resultAdd;
                    }

                }
            }
            
            //инсерт дефектов
            for (int i4 = 0; i4 < importias.DEFECTS.Count; i4++)
            {
                var d = GetFieldsAndValuesDefect(importias.DEFECTS[i4]);
                if (d.Count != 0)
                {
                    resultAdd = CBLL.AddTable("Defect_metalla", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        return resultAdd;
                    }
                }
            }

           
            //инсерт шурфов
            for (int i5 = 0; i5 < importias.SHURFS.Count; i5++)
            {
                var d = GetFieldEndValueShurf(importias.SHURFS[i5]);
                if (d.Count != 0)
                {
                    resultAdd = CBLL.AddTable("Shurf", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        return resultAdd;
                    }
                }
            }
            //инсерт шурф-труба
             for (int i6 = 0; i6 < importias.SHURF_PIPE.Count; i6++)
             {
                 var d = GetFieldEndValueShurf_Pipe(importias.SHURF_PIPE[i6]);
                if (d.Count != 0)
                {
                    resultAdd = CBLL.AddTable("Shurf_Truba", d);
                    if (resultAdd != "")
                    {
                       
                        MessageBox.Show(resultAdd);
                        return resultAdd;
                    }
                }
             }
            


            return resultAdd;
        }

        /// <summary>
        /// для инсерта добавляем поле-значение (МГ)
        /// </summary>
        /// <param name="dataMG"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldsAndValuesMG(ImportAllBaza.MG dataMG)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(dataMG.KEY_MG))
            {
                d["Key_MG"] = dataMG.KEY_MG;

                if (!string.IsNullOrWhiteSpace(dataMG.KEY_MG))
                {
                    d["NAME"] = "'" + dataMG.NAME + "'";

                }
            }
            return d;
        }


        /// <summary>
        /// для инсерта добавляем поле-значение (Нить)
        /// </summary>
        /// <param name="dataTHREAD"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldsAndValuesTHREAD(ImportAllBaza.THREAD dataTHREAD)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(dataTHREAD.KEY_THREAD))
            {
                d["Key_thread"] = dataTHREAD.KEY_THREAD;

                if (!string.IsNullOrWhiteSpace(dataTHREAD.NAMETHREAD))
                {
                    d["NameThread"] = "'" + dataTHREAD.NAMETHREAD + "'";
                }

                if (!string.IsNullOrWhiteSpace(dataTHREAD.NKM_BEG))
                {
                    d["Nkm_beg"] = (dataTHREAD.NKM_BEG.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataTHREAD.NKM_END))
                {
                    d["Nkm_end"] = (dataTHREAD.NKM_END.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataTHREAD.KEY_MG))
                {
                    d["Key_MG"] = dataTHREAD.KEY_MG;
                }
            }
            return d;
        }
        /// <summary>
        /// для инсерта добавляем поле-значение (участок)
        /// </summary>
        /// <param name="dataRegions_thread"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldsAndValuesRegions_thread(ImportAllBaza.SECTION dataRegions_thread)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(dataRegions_thread.NKEY_REGION))
            {
                d["nkey_region"] = dataRegions_thread.NKEY_REGION;

                if (!string.IsNullOrWhiteSpace(dataRegions_thread.CNAME))
                {
                    d["cName"] = "'" + dataRegions_thread.CNAME + "'";
                }
                else
                {
                    d["cName"] = "'<не определено>'";
                }
                if (!string.IsNullOrWhiteSpace(dataRegions_thread.NKM_BEGIN))
                {
                    d["nkm_begin"] = (dataRegions_thread.NKM_BEGIN.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataRegions_thread.NKM_END))
                {
                    d["nkm_end"] = (dataRegions_thread.NKM_END.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataRegions_thread.KEY_THREAD))
                {
                    d["Key_thread"] = dataRegions_thread.KEY_THREAD;
                }
            }
            return d;
        }
        /// <summary>
        /// для инсерта добавляем поле-значение (труба)
        /// </summary>
        /// <param name="dataPipe"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldsAndValuesPipe(ImportAllBaza.PIPE dataPipe)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(dataPipe.NKEY_PIPE))
            {
                d["nKey_pipe"] = dataPipe.NKEY_PIPE;

                if (!string.IsNullOrWhiteSpace(dataPipe.CTYPE_PIPE))
                {
                    d["cType_pipe"] = "'" + dataPipe.CTYPE_PIPE + "'";
                }

                if (!string.IsNullOrWhiteSpace(dataPipe.CLENGTH))
                {
                    d["clength"] = (dataPipe.CLENGTH.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.CDEPTH_PIPE))
                {
                    d["cdepth_pipe"] = (dataPipe.CDEPTH_PIPE.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.NKEY_REGION))
                {
                    d["NKEY_REGION"] = dataPipe.NKEY_REGION;
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.NTYPE_PIPE_KEY))
                {
                    d["ntype_pipe_key"] = dataPipe.NTYPE_PIPE_KEY;
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.NANGLE))
                {
                    d["nAngle"] = (dataPipe.NANGLE.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.NMAXPRESSURE))
                {
                    d["nmaxpressure"] = (dataPipe.NMAXPRESSURE.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.NPRESSURE))
                {
                    d["npressure"] = (dataPipe.NPRESSURE.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.CDIAM))
                {
                    d["cdiam"] = (dataPipe.CDIAM.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.NDICT_KATEGOR_UCH_KEY))
                {
                    d["ndict_kategor_uch_key"] = dataPipe.NDICT_KATEGOR_UCH_KEY;
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.NFACTORY_BUILDER_KEY))
                {
                    d["nFactory_builder_key"] = dataPipe.NFACTORY_BUILDER_KEY;
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.NDICT_FEEL_GRADE_KEY))
                {
                    d["nDict_feel_grade_key"] = dataPipe.NDICT_FEEL_GRADE_KEY;
                }
                //if (!string.IsNullOrWhiteSpace(dataPipe.DCHANGE_DATE))
                //{
                //    d["nDataExpl"] = "'" + dataPipe.DCHANGE_DATE + "'";
                //}
                if (!string.IsNullOrWhiteSpace(dataPipe.CNUMBER_PIPE))
                {
                    d["cNumber_pipe"] = "'" + dataPipe.CNUMBER_PIPE + "'";
                }
                if (!string.IsNullOrWhiteSpace(dataPipe.CLOC_KM_BEG))
                {
                    d["cloc_km_beg"] = (dataPipe.CLOC_KM_BEG.ToString()).Replace(",", ".");
                }

            }
            return d;
        }
        /// <summary>
        /// для инсерта добавляем поле-значение (дефект)
        /// </summary>
        /// <param name="dataDefect"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldsAndValuesDefect(ImportAllBaza.DEFECT dataDefect)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(dataDefect.NDEFECT_KEY))
            {
                d["nDefect_key"] = dataDefect.NDEFECT_KEY;
                if (!string.IsNullOrWhiteSpace(dataDefect.NMETOD_USTR_DEF_KEY))
                {
                    d["nMetod_Ustr_Def_Key"] = dataDefect.NMETOD_USTR_DEF_KEY;
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NDEPTH_POS_KEY))
                {
                    d["ndepth_pos_Key"] = dataDefect.NDEPTH_POS_KEY;
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NDICT_CALC_TYPE_KEY))
                {
                    d["nDict_Calc_Type_Key"] = dataDefect.NDICT_CALC_TYPE_KEY;
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NTYPEDEFECT_KEY))
                {
                    d["nTypeDefect_Key"] = dataDefect.NTYPEDEFECT_KEY;
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NKEY_PIPE))
                {
                    d["nKey_pipe"] = dataDefect.NKEY_PIPE;
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NLENGTHDEFECT))
                {
                    d["nlengthDefect"] = (dataDefect.NLENGTHDEFECT.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NCLOCKWISE_POS))
                {
                    d["nclockwise_pos"] = (dataDefect.NCLOCKWISE_POS.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NWIDTH))
                {
                    d["nwidth"] = (dataDefect.NWIDTH.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NDEPTH))
                {
                    d["ndepth"] = (dataDefect.NDEPTH.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.CNAME))
                {
                    d["cName"] = "'" + dataDefect.CNAME + "'";
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NLOSS_METALL))
                {
                    d["nloss_metall"] = dataDefect.NLOSS_METALL;
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NPREVSEAM_DIST))
                {
                    d["nprevseam_dist"] = (dataDefect.NPREVSEAM_DIST.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.NNEXT_DIST))
                {
                    d["nnext_dist"] = (dataDefect.NNEXT_DIST.ToString()).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataDefect.CSTRESS_DESCR))
                {
                    d["cstress_descr"] = "'" + dataDefect.CSTRESS_DESCR + "'";
                }

                //if ((dataDefect.LOC_NKM != null) && (dataDefect.LOC_NKM != 0))
                //{
                //    d["nnext_dist"] = (dataDefect.LOC_NKM.ToString()).Replace(",", ".");
                //}
                //LOC_NKM??
            }
            return d;
        }

        /// <summary>
        /// для инсерта добавляем поле-значение(шурф)
        /// </summary>
        /// <param name="dataShurf"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldEndValueShurf(ImportAllBaza.SHURF dataShurf)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(dataShurf.NSHURF_KEY))
            {
                d["nShurf_key"] = dataShurf.NSHURF_KEY;

                if (!string.IsNullOrWhiteSpace(dataShurf.CNUMBERAKT))
                {
                    d["cNumberAkt"] = dataShurf.CNUMBERAKT;
                }


                if (!string.IsNullOrWhiteSpace(dataShurf.DDATESHURF))
                {
                    d["dDateShurf"] = "'" + dataShurf.DDATESHURF + "'";
                }


                if (!string.IsNullOrWhiteSpace(dataShurf.DDATE_VISIT))
                {

                    d["dDate_visit"] = "'" + dataShurf.DDATE_VISIT + "'";
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.CDEFECTOSKOPIST))
                {

                    d["cdefectoskopist"] = "'" + dataShurf.CDEFECTOSKOPIST + "'";
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.CAGENT_LPU))
                {

                    d["cagentLPU"] = "'" + dataShurf.CAGENT_LPU + "'";
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.CRELIEF_GROUND))
                {
                    d["crelief_ground"] = "'" + dataShurf.CRELIEF_GROUND + "'";
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NDICT_CHARACTER_GRUNT_KEY))
                {
                    d["nDict_Character_grunt_key"] = dataShurf.NDICT_CHARACTER_GRUNT_KEY;
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NDEPTH_LOCATION))
                {
                    d["ndepth_location"] = (dataShurf.NDEPTH_LOCATION).Replace(",", ".");
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NDEPTH))
                {
                    d["ndepth"] = (dataShurf.NDEPTH).Replace(",", ".");
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NLENGTH))
                {
                    d["nlength"] = (dataShurf.NLENGTH).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataShurf.CWATER_LEVEL))
                {
                    d["cwater_level"] = (dataShurf.CWATER_LEVEL).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataShurf.NREZISTOR_GRUNT))
                {
                    d["nrezistor_grunt"] = (dataShurf.NREZISTOR_GRUNT).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataShurf.NVOLTAGE_PIPE))
                {
                    d["nvoltage_pipe"] = (dataShurf.NVOLTAGE_PIPE).Replace(",", ".");
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NTYPE_IZOL_KEY))
                {
                    d["ntype_izol_key"] = dataShurf.NTYPE_IZOL_KEY;
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NDICT_IZOL_STATE_KEY))
                {
                    d["nDict_Izol_state_key"] = dataShurf.NDICT_IZOL_STATE_KEY;
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NNUMBER_SKIN))
                {
                    d["nNumber_skin"] = (dataShurf.NNUMBER_SKIN).Replace(",", ".");
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NNUMBEROBERTKA))
                {
                    d["nNumberObertka"] = (dataShurf.NNUMBEROBERTKA).Replace(",", ".");
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NDICT_AVAILABI_OVERL_KEY))
                {
                    d["ndict_availabi_overl_key"] = dataShurf.NDICT_AVAILABI_OVERL_KEY;
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NDICT_ADHERENC_PIPE_KEY))
                {
                    d["ndict_adherenc_pipe_key"] = dataShurf.NDICT_ADHERENC_PIPE_KEY;
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.NDICT_AVAILABIL_DAMP_KEY))
                {
                    d["ndict_availabil_damp_key"] = dataShurf.NDICT_AVAILABIL_DAMP_KEY;
                }
                if (!string.IsNullOrWhiteSpace(dataShurf.NINSPECT_SQUARE))
                {
                    d["nInspect_square"] = (dataShurf.NINSPECT_SQUARE).Replace(",", ".");
                }
                if (!string.IsNullOrWhiteSpace(dataShurf.NINSPECT_SQUARE_KOROZ))
                {
                    d["nInspect_square_koroz"] = (dataShurf.NINSPECT_SQUARE_KOROZ).Replace(",", ".");
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.CVID_KOROZ_DAMAGE))
                {
                    d["cVid_koroz_damage"] = "'" + dataShurf.CVID_KOROZ_DAMAGE + "'";
                }

                if (!string.IsNullOrWhiteSpace(dataShurf.CCONCULUSION))
                {
                    d["cConculusion"] = "'" + dataShurf.CCONCULUSION + "'";
                }

                d["isEdited"] = "2";

                //?????????????????
                //"DCHANGE_DATE":"2013-04-02",
                //"DEDITING_DATA":"",
                //"NDICT_TEMPERATURN_KOEF_KEY":""               

            }
            return d;
        }

        /// <summary>
        /// для инсерта добавляем поле-значение(шурф труба)
        /// </summary>
        /// <param name="dataShurfPipe"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldEndValueShurf_Pipe(ImportAllBaza.SHURFPIPE dataShurfPipe)
        {
           Dictionary<string, string> d = new Dictionary<string, string>();

            if ((!string.IsNullOrWhiteSpace(dataShurfPipe.NKEY_PIPE)) &&
                (!string.IsNullOrWhiteSpace(dataShurfPipe.NSHURF_KEY)))
            {
                d["nKey_pipe"] = dataShurfPipe.NKEY_PIPE;
                d["nShurf_key"] = dataShurfPipe.NSHURF_KEY;

            }

            return d;
        }

        /// <summary>
        /// создание резервная копия базы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCopyBD_OnClick(object sender, RoutedEventArgs e)
        {
            string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
            indicator.BusyContent = "Создание резервной копии...";
            indicator.IsBusy = true;
            try
            {

                SaveFileDialog dialog = new SaveFileDialog();

                DateTime time = DateTime.Now;              // Use current time

                dialog.FileName = time.ToShortDateString() + "offlinemodule.db";
                dialog.DefaultExt = "ofline.db";
                dialog.Filter = "Документ (.ldb)|*.db";


                dialog.FilterIndex = 1;


                bool? saveClicked = dialog.ShowDialog();

                if (saveClicked == true)
                {
                    File.Copy(conStr, dialog.FileName, true);
                }

               

                StreamWriter writer = new System.IO.StreamWriter(@"dataCopyBaza.txt", false);
                writer.WriteLine(time.ToShortDateString());
                writer.Close();
                txtDataCopy.Text = time.ToShortDateString();
                indicator.IsBusy = false;
            }
            catch (Exception ee)
            {
                indicator.IsBusy = false;
                MessageBox.Show("Не удается сделать резервное копирование базы. " + ee);

            }

        }

        /// <summary>
        /// загрузка резервной копии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddFileRecovery_OnClick(object sender, RoutedEventArgs e)
        {
            nameFileRezervCopy = "не определено";
            try
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = ".ldb",
                    Filter = "Документ (.ldb)|*.db"
                };
                Nullable<bool> result = dlg.ShowDialog();
                nameFileRezervCopy = dlg.FileName;

                if (result == true)
                {
                    txtNameFileRecovery.Text = nameFileRezervCopy;
                    name = dlg.SafeFileName;
                    string dateFileCopy = name;
                    dateFileCopy = dateFileCopy.Substring(0, 10);
                    txtDataCopyRezerv.Text = dateFileCopy;
                    btnInsertCopyBD.IsEnabled = true;
                }

            }
            catch (Exception ee)
            {

                txtReport.Text = " Ошибка чтения ";
            }
        }


        private void BtnInsertCopyBD_OnClick(object sender, RoutedEventArgs e)
        {

            try
            {

                indicator.IsBusy = true;
                indicator.BusyContent = "Восстановление Базы данных " + nameFileRezervCopy;

                if (Model.ShowConfirmDialog("Внимание! Будет восстановлена база данных " + nameFileRezervCopy))
                {
                    string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                    conStr = conStr.Substring(0, conStr.Length - 9);
                    name = "DB\\" + name;
                    File.Copy(nameFileRezervCopy, @name, true);
                    if (File.Exists(@name))
                    {

                        File.Delete(@"DB\\offlinemodule.db");
                        File.Copy(@name, @"DB\\offlinemodule.db", true);
                        File.Delete(@name);

                    }

                    //обнуляем все данные по трубам
                    SavePipe sp = new SavePipe();
                    sp = new SavePipe();
                    sp.ActwihtRegion = true;
                    sp.KeyMg = "";
                    sp.NameMg = "";
                    sp.KeyNit = "";
                    sp.NameNit = "";
                    sp.KmBegin = "";
                    sp.KmEnd = "";
                    sp.NameRegion = "";
                    sp.KeyRegion = "";
                    sp.KeyAkt = "";
                    sp.NumberAkt = "";
                    sp.DataAkt = "";
                    sp.KmAkt = "";
                    sp.NumberPipe = "";

                    Model.SelectMgKey = "";
                    Model.SelectMgName = "";
                    Model.SelectNitKey = "";
                    Model.SelectNitName = "";
                    Model.SelectedKmBegin = "";
                    Model.SelectedKmEnd = "";
                    Model.SelectRegionName = "";
                    Model.SelectKeyRegion = "";
                    Model.SelectPipeKm = "";
                    Model.NumberAct = "";
                    Model.SelectPipeNumberPipe = "";

                    FileStream fs = new FileStream(@"SavePipe.xml", FileMode.Create, FileAccess.Write);
                    Helper.Serialize(fs, sp);
                    fs.Close();

                    MessageBox.Show("Восстановление базы прошло успешно.");
                    indicator.IsBusy = false;
                }
                else
                {
                    indicator.IsBusy = false;
                }


            }
            catch (Exception ee)
            {

                MessageBox.Show("Ошибка переноса базы " + ee);
                indicator.IsBusy = false;
            }
        }


        /// <summary>
        /// выбор ветки в дереве
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadTreeView_OnSelected(object sender, RadRoutedEventArgs e)
        {
            _selectedTree = e.OriginalSource as RadTreeViewItem;
            //cbSelectRegion.IsEnabled = false;
            string parentKey = ((CreateTree)radTreeView.SelectedItem).KeyParent;
            if ((parentKey == "3") || (parentKey == "10"))
            {
                //txtRegionSelect.Text = ((CreateTree)radTreeView.SelectedItem).NAME;
                selectKeyOnTree = ((CreateTree)radTreeView.SelectedItem).KEY;
                //cbSelectRegion.IsEnabled = true;
            }
            else
            {
                //txtRegionSelect.Text = "";
            }
        }

    }
}



       