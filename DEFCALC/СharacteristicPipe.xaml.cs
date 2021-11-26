using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DEFCALC.DataModel;
using DrawPipe.DataModel;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for СharacteristicPipe.xaml
    /// </summary>
    public partial class СharacteristicPipe : UserControl
    {

        private bool _isLoaded = false;
        private bool _isLoaded2 = false; //загрузка пайп контрола(при показе)
        public CharacteristicPipeModel cpm_base;
        private bool isEditedPipe = false;

        private string KeyCurrentPipe = "";
        private bool _actwihtRegion;

        public СharacteristicPipe()
        {
            InitializeComponent();

        }
        //возможность редактировать трубу
        private bool GetEditPipe(string keyPipe)
        {
            bool editPipe = false;
            if (!string.IsNullOrEmpty(keyPipe))
            {
                if (KeyCurrentPipe.Length > 2)
                {
                    string ss = keyPipe.Substring(keyPipe.Length - 2);
                    if (ss == "00")
                    {
                        editPipe = true;
                        GetEditControl();
                    }
                }
            }
            return editPipe;
        }

        private void GetEditControl()
        {
            txtDiamPipe.IsReadOnly = false;
            txtDepth.IsReadOnly = false;
            txtMaxPressure.IsReadOnly = false;
            txtPressure.IsReadOnly = false;
            ddlKategorUch.IsReadOnly = false;
            ddlTemperatureKoef.IsReadOnly = false;

        }

        MainViewModel Model { get { return App.Model; } }

        public bool ActwihtRegion
        {
            get { return _actwihtRegion; }
            set
            {
                _actwihtRegion = value;

            }
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (_isLoaded2)
            {
                pipeControlch.DataContext = null;
                pipeControlch.DataContext = App.Model.PipeModel;
                pipeControlch.Init();
                pipeControlch.Draw();
                string key = Model.PipeModel.GetKeyByIndex(Model.PipeModel.CentralIndex);
                Model.PipeModel.IsVisibleButton = false;
                KeyCurrentPipe = key;
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //groupBoxDataControl.DataContext = CBLL.GetCharacteristicPipe(key, Model.KeyAkt);
                //!!!!!!!!!!!!!!!!!!!!

                this.DataContext = CBLL.GetCharacteristicPipe(key, Model.KeyAkt);
                //border.DataContext = groupBoxDataControl.DataContext;

                cpm_base = groupBoxDataControl.DataContext as CharacteristicPipeModel;
                //создаем модель для валидации конкретного контрола и передаем эту модель в датаконтекст + тип валидации
                //в данном случае тип валидации 1 - проверка на числа
                //NumberModel = new NumberModel(cpm_base.MaxPressure, 1);
                //NumberModel.IsValidate = true;
                //txtMaxPressure.DataContext = NumberModel;

                // isEditedPipe = GetEditPipe(KeyCurrentPipe);
                bool t = (GetEditPipe(KeyCurrentPipe));

                _isLoaded2 = true;


            }

            if (!_isLoaded)
            {

                DataContext = App.Model;
                _isLoaded = true;

                if ((Model.SignatureAkt == "1")||((Model.SignatureAkt == "2")))
                {

                    kmPipe.IsEnabled = false;
                    anglePipe.IsEnabled = false;
                    txtDiamPipe.IsEnabled = false;
                    txtDepth.IsEnabled = false;
                    txtMaxPressure.IsEnabled = false;
                    txtPressure.IsEnabled = false;
                    ddlKategorUch.IsEnabled = false;
                    ddlTemperatureKoef.IsEnabled = false;
                    ddlFactoryBuilder.IsEnabled = false;
                    ddlMarkaStal.IsEnabled = false;

                }
                else
                {

                    kmPipe.IsEnabled = true;
                    anglePipe.IsEnabled = true;
                    txtDiamPipe.IsEnabled = true;
                    txtDepth.IsEnabled = true;
                    txtMaxPressure.IsEnabled = true;
                    txtPressure.IsEnabled = true;
                    ddlKategorUch.IsEnabled = true;
                    ddlTemperatureKoef.IsEnabled = true;
                    ddlFactoryBuilder.IsEnabled = true;
                    ddlMarkaStal.IsEnabled = true;

                }



                _isLoaded = true;
                _isLoaded2 = true;

            }

            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;

            //Model.PipeModel.PropertyChanged -= PipeModel_PropertyChanged;
            //Model.PipeModel.PropertyChanged += PipeModel_PropertyChanged;
            //Model.PipeModel.IsVisibleButton = false;
            //cpm_base.PropertyChanged -= Cpm_base_PropertyChanged;
            //cpm_base.PropertyChanged += Cpm_base_PropertyChanged;

        }

        public void LoadPipe()
        {
            try
            {
                this.DataContext = CBLL.GetCharacteristicPipe(Model.KeyPipe, Model.KeyAkt);
                cpm_base = groupBoxDataControl.DataContext as CharacteristicPipeModel;

            }
            catch (Exception ee)
            {
                Model.Report("CharacteristicPipe.LoadPipe  " + ee);

            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if ((Model.SignatureAkt == "0") || (string.IsNullOrEmpty(Model.SignatureAkt)))
            {
                SavePipe();
            }



        }
        /// <summary>
        /// сохраняем трубу
        /// </summary>
        public void SavePipe()
        {
            try
            {
                Model.CPM = cpm_base;
                if (!string.IsNullOrEmpty(KeyCurrentPipe))
                {
                    //сохраняем всегда трубу
                    var d = cpm_base.GetFieldsAndValuesPipe();

                    //string report = CBLL.SaveNewPipeUpdate(KeyCurrentPipe, d);
                    string report = CBLL.SaveNewPipeUpdate(Model.KeyPipe,d) ;
                    Model.UpdateAngelPipe = anglePipe.Text;
                }

                Model.Report("CharacteristicPipe.SavePipe  Труба сохранилась ");

            }
            catch (Exception ee)
            {
                Model.Report("CharacteristicPipe.SavePipe  " + ee);
                
            }
        }

 


        /// <summary>
        /// при изменении угла трубы меняем (при смене фокуса в контроле) меняется и в pipemodule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void anglePipe1_LostFocus(object sender, RoutedEventArgs e)
        {
            string ff = "";

            if (((anglePipe.Text != "")) && ((cpm_base.AngleShovPipe != anglePipe.Text)))
            {
                PipeSegment ps = null;
                //при изменении угла трубы меняем (при смене фокуса в контроле) меняется и в pipemodule
                for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                {
                    if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
                    {
                        ps = Model.PipeModel.Pipe.SegmentList[i];
                    }
                }
                double result;
                if (ps != null)
                {

                    string checkedAnglePipe = anglePipe.Text;
                    string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                    checkedAnglePipe = checkedAnglePipe.Replace(".", separator);
                    checkedAnglePipe = checkedAnglePipe.Replace(",", separator);

                    if (double.TryParse(checkedAnglePipe, out result))
                    {
                        ps.Angle = result;
                        pipeControlch.SingleSegment.Init(ps);
                    }
                }

            }
            Model.PipeModel.AngleShovPipe = anglePipe.Text;

        }
        /// <summary>
        /// при изменении номера трубы меняем (при смене фокуса в контроле) меняется и в pipemodule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberPipe1_LostFocus(object sender, RoutedEventArgs e)
        {
            string ff = "";

            if (((numberPipe.Text != "")) && ((cpm_base.NumberPipe != numberPipe.Text)))
            {
                PipeSegment ps = null;
                //при изменении номера трубы меняем (при смене фокуса в контроле) меняется и в pipemodule
                for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                {
                    if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
                    {
                        ps = Model.PipeModel.Pipe.SegmentList[i];
                    }
                }
                double result;
                if (ps != null)
                {
                    ps.NumberSegment = numberPipe.Text;
                    pipeControlch.SingleSegment.Init(ps);
                }
            }
        }
        /// <summary>
        /// при изменении километража трубы меняем (при смене фокуса в контроле) меняется и в pipemodule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kmPipe1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((kmPipe.Text != "")) && ((cpm_base.KmPipe != kmPipe.Text)))
            {
                PipeSegment ps = null;
                //при изменении километража трубы меняем (при смене фокуса в контроле) меняется и в pipemodule
                for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                {
                    if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
                    {
                        ps = Model.PipeModel.Pipe.SegmentList[i];
                    }
                }
                double result;
                if (ps != null)
                {
                    ps.Km = kmPipe.Text;
                    pipeControlch.SingleSegment.Init(ps);
                }
            }
        }


        void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("SignatureAkt".Equals(e.PropertyName))
            {
                if ((Model.SignatureAkt == "1") || (Model.SignatureAkt == "2"))
                {

                    //сохраняем трубу
                    SavePipe();

                    kmPipe.IsEnabled = false;
                    anglePipe.IsEnabled = false;
                    txtDiamPipe.IsEnabled = false;
                    txtDepth.IsEnabled = false;
                    txtMaxPressure.IsEnabled = false;
                    txtPressure.IsEnabled = false;
                    ddlKategorUch.IsEnabled = false;
                    ddlTemperatureKoef.IsEnabled = false;
                    ddlFactoryBuilder.IsEnabled = false;
                    ddlMarkaStal.IsEnabled = false;

                }
                else
                {

                    kmPipe.IsEnabled = true;
                    anglePipe.IsEnabled = true;
                    txtDiamPipe.IsEnabled = true;
                    txtDepth.IsEnabled = true;
                    txtMaxPressure.IsEnabled = true;
                    txtPressure.IsEnabled = true;
                    ddlKategorUch.IsEnabled = true;
                    ddlTemperatureKoef.IsEnabled = true;
                    ddlFactoryBuilder.IsEnabled = true;
                    ddlMarkaStal.IsEnabled = true;

                }

            }
        }

 

    }
}
