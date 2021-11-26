using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for CreateActShurfovaniya.xaml
    /// </summary>
    public partial class CreateActShurfovaniya : Window
    {
        private bool _actwihtRegion;

        private BackgroundWorker bw = new BackgroundWorker();//паралельный поток

        public CreateActShurfovaniya()
        {
            LocalizationManager.DefaultResourceManager = ControlResources.ResourceManager;
            InitializeComponent();
            DataContext = App.Model;
            if (Model.SelectPipeList.Count == 0)
            {
                txtRegion.Visibility = Visibility.Visible;
                txtKm.Visibility = Visibility.Visible;
                NumberP.Visibility = Visibility.Visible;
                NumberPipeNo.Visibility = Visibility.Visible;
                NumberDecimalDigits.Visibility = Visibility.Visible;
                txtKt.Visibility = Visibility.Visible;
                NumberPipeVTD.Visibility = Visibility.Collapsed;
                txtselectPipe.Visibility = Visibility.Collapsed;
                txtselectRegion.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtRegion.Visibility = Visibility.Collapsed;
                txtKm.Visibility = Visibility.Collapsed;
                NumberP.Visibility = Visibility.Collapsed;
                NumberPipeNo.Visibility = Visibility.Collapsed;
                NumberDecimalDigits.Visibility = Visibility.Collapsed;
                txtKt.Visibility = Visibility.Collapsed;
                NumberPipeVTD.Visibility = Visibility.Visible;
                txtselectPipe.Visibility = Visibility.Visible;
                txtselectRegion.Visibility = Visibility.Visible;
            }

        }

        MainViewModel Model { get { return App.Model; } }

        /// <summary>
        /// переменная - показывать участок или нет
        /// </summary>
        public bool ActwihtRegion
        {
            get { return _actwihtRegion; }
            set
            {
                _actwihtRegion = value;

            }
        }

        /// <summary>
        /// Клик на кнопке "Создать акт" переходим на форму редактирования акта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateActShurfovaniya_Click(object sender, RoutedEventArgs e)
        {
            Indicator.IsBusy = true;
            try
            {
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //Анализировать если комбобокс SelectedDictDefectoskopist = -1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //if ((!string.IsNullOrEmpty(txtNumberOfAct.Text))&&(cldShurfDate.SelectedValue !=null))
                if ((!string.IsNullOrEmpty(txtNumberOfAct.Text)) && (cldShurfDate.SelectedValue != null))
                {

                    //создаем новый акт 
                    string defectoskopist = "";
                    string agentLpu = "";

                    //is_edited = 2 - insert! 1- edit act
                    Model.NumberAct = txtNumberOfAct.Text;
                    if (cldShurfDate.SelectedDate != null)
                    {
                        DateTime d1 = (DateTime)cldShurfDate.SelectedDate;
                        Model.DataShurf = d1.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        Model.DataShurf = "";
                    }
                    var d = GetFieldsAndValues();
                    if (Model.SelectPipeList.Count == 0)
                    {
                        string comment = "";
                        comment = txtRegion.Text + ",  " + txtKm.Text + ",  " + NumberPipeNo.Text;
                        d.Add("Comment", "'" + comment + "'");

                    }
                    //сохраняем акт
                    Model.SaveNewAct(d, NumberDecimalDigits.Value.ToString());
                    CreateNewAkt();




                    bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                    bw.RunWorkerCompleted += (bw_RunWorkerCompleted);
                    bw.ProgressChanged += (bw_ProgressChanged);

                    bw.WorkerReportsProgress = true;
                    bw.WorkerSupportsCancellation = true;
                    bw.RunWorkerAsync();

                   

                }
                else
                {
                    Indicator.IsBusy = false;
                    MessageBox.Show("Не все поля заполнены!!!");
                }

            }
            catch (Exception ee)
            {
                Indicator.IsBusy = false;
                Model.Report("btnCreateActShurfovaniya_Click " + ee);
                MessageBox.Show("Ошибка в создании акта - "+ ee);
            }

  

        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //расчет дефектов на трубах этого акта
            //расчет дефектов
            //перенести в параллельный процесс
            
                CalcDefect cd = new CalcDefect();
                cd.InitCalc();
           
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Indicator.IsBusy = false;
               
            }
            else
            {
                Indicator.IsBusy = false;
                MessageBox.Show("Ошибка расчета дефектов");
            }

            EditAct editAct = new EditAct();
            editAct.DataContext = App.Model;
            Application.Current.MainWindow = editAct;
            editAct.Show();
            this.Close();


            ////http://msdn.microsoft.com/ru-ru/library/ms229675.aspx
            //// http://social.msdn.microsoft.com/Forums/ru-RU/aspnetru/thread/ad892d3e-0db7-44d5-ad06-59a55750be2c
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

        }

        private Dictionary<string, string> GetFieldsAndValues()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(txtNumberOfAct.Text))
            {
                d["cNumberAkt"] = "'" + txtNumberOfAct.Text + "'";
            }
            if (cldShurfDate.SelectedDate != null)
            {
                DateTime d1 = (DateTime)cldShurfDate.SelectedDate;

                d["dDateShurf"] = "'" + d1.ToString("dd.MM.yyyy") + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtDefectoskopist.Text))
            {
                d["cdefectoskopist"] = "'" + txtDefectoskopist.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtAgentLPU.Text))
            {
                d["cagentLPU"] = "'" + txtAgentLPU.Text + "'";
            }

            return d;
        }



        /// <summary>
        /// Клик на кнопке "Отменить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Model.NameFormWindow = MainViewModel.NameFormWindows.SelectPipe;

            switch (Model.NameFormWindow)
            {
                case MainViewModel.NameFormWindows.SelectPipe:
                    {

                        if (!ActwihtRegion)
                        {

                            //Переходим на главную страницу проекта
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            Application.Current.MainWindow = mainWindow;
                            this.Close();

                        }
                        else
                        {
                            //Переходим на главную страницу выбора трубы
                            SelectPipe selectPipe = new SelectPipe();
                            Model.NameFormWindow = MainViewModel.NameFormWindows.MainWindow;
                            selectPipe.Show();
                            Application.Current.MainWindow = selectPipe;
                            this.Close();
                        }

                    }
                    break;


            }

        }
        /// <summary>
        /// при загрузке окна проверяем показывать участок или нет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model.VisibleReport == 0)
                grdMain.RowDefinitions[3].Height = new GridLength(0);

            //??? непонятно?????
            //if (!ActwihtRegion)
            //{
            //    grdMain.RowDefinitions[0].Height = new GridLength(0);

            //}

            else
            {
                string km = "";
                string numberPipe = "";
                string text1 = "";
                km = "Километраж: " + Model.SelectPipeKm;
                numberPipe = "Номера труб: " + Model.SelectPipeNumberPipe;

            }

            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void CreateNewAkt()
        {
           
            Model.NumberAct = txtNumberOfAct.Text;
            Model.DataAct = cldShurfDate.SelectedValue.Value.ToString();
            Model.ActwihtRegion = ActwihtRegion;
            Model.SignatureAkt = "0";

            //сохраняем в xml данные по акту и с выбором мг, нитки и т.д.
            SavePipe sp = new SavePipe();

            if (!ActwihtRegion)
            {
                sp = new SavePipe();
                sp.ActwihtRegion = ActwihtRegion;
                sp.KeyMg = "";
                sp.NameMg = "";
                sp.KeyNit = "";
                sp.NameNit = "";
                sp.KmBegin = "";
                sp.KmEnd = "";
                sp.NameRegion = "";
                sp.KeyRegion = "";
                sp.KeyAkt = Model.KeyAkt;
                sp.NumberAkt = Model.NumberAct;
                sp.DataAkt = Model.DataAct;
                sp.KmAkt = "";
                sp.NumberPipe = "";
                sp.ActwihtRegion = ActwihtRegion;

                Model.SelectMgKey = "";
                Model.SelectMgName = "";
                Model.SelectNitKey = "";
                Model.SelectNitName = "";
                Model.SelectedKmBegin = "";
                Model.SelectedKmEnd = "";
                Model.SelectRegionName = "";
                Model.SelectKeyRegion = "";


                Model.SelectPipeKm = "";
                Model.SelectPipeNumberPipe = "";


            }
            else
            {
                sp = new SavePipe();
                sp.KeyMg = Model.SelectMgKey;
                sp.NameMg = Model.SelectMgName;
                sp.KeyNit = Model.SelectNitKey;
                sp.NameNit = Model.SelectNitName;
                sp.KmBegin = Model.SelectedKmBegin;
                sp.KmEnd = Model.SelectedKmEnd;
                sp.NameRegion = Model.SelectRegionName;
                sp.KeyRegion = Model.SelectKeyRegion;
                sp.KeyAkt = Model.KeyAkt;
                sp.NumberAkt = Model.NumberAct;
                sp.DataAkt = Model.DataAct;
                sp.KmAkt = Model.SelectPipeKm;
                sp.NumberPipe = Model.SelectPipeNumberPipe;
                sp.ActwihtRegion = ActwihtRegion;
            }


            FileStream fs = new FileStream(@"SavePipe.xml", FileMode.Create, FileAccess.Write);
            Helper.Serialize(fs, sp);
            fs.Close();

        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("CreateNewAkt".Equals(e.PropertyName))
            {
                

            }
        }
            


    }
}

