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

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for DocumentPresentation.xaml
    /// </summary>
    public partial class DocumentPresentation : UserControl
    {

        MainViewModel Model { get { return App.Model; } }
        private bool _isLoaded = false;
        private DocumentPresentationModel dpm2;

        public DocumentPresentation()
        {
            InitializeComponent();
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (!_isLoaded)
            {
                //DataContext = App.Model;
                _isLoaded = true;



                try
                {
                    DocumentPresentationModel dpm = CBLL.GetDocumentPresentation(Model.KeyAkt);
                    this.DataContext = dpm;
                    dpm2 = this.DataContext as DocumentPresentationModel;
                    if (dpm2.DataShurf != "")
                    {
                        cldShurfDate.SelectedValue = DateTime.Parse(dpm2.DataShurf);
                    }
                    if (dpm2.DataVisible != "")
                    {
                        cldVisitDate.SelectedValue = DateTime.Parse(dpm2.DataVisible);
                    }
                }
                catch (Exception ee)
                {

                    Model.Report("DocumentPr CBLL.GetDocumentPresentation " + ee);
                }



            }

            switch (Model.SignatureAkt)
            {
                case "1":
                case "2":
                    {
                        txtName.IsEnabled = false;
                        cldShurfDate.IsEnabled = false;
                        cldVisitDate.IsEnabled = false;
                        txtDefectoskopist.IsEnabled = false;
                        txtAgentLPU.IsEnabled = false;
                        txtYearExspl.IsEnabled = false;
                        txtCharacterRelief.IsEnabled = false;
                        ddlCharacterGrunt.IsEnabled = false;
                        txtDepth.IsEnabled = false;
                        txtHeight.IsEnabled = false;
                        txtLenght.IsEnabled = false;
                        txtLevelWater.IsEnabled = false;
                        txtResister.IsEnabled = false;
                        txtVoltage.IsEnabled = false;
                        ddlTypeIzol.IsEnabled = false;
                        ddlIzolState.IsEnabled = false;
                        txtNumberSkin.IsEnabled = false;
                        txtNumberObertka.IsEnabled = false;
                        ddlAvailabOverl.IsEnabled = false;
                        ddlAdherencPipe.IsEnabled = false;
                        ddlAvailabDamp.IsEnabled = false;
                        txtInspectSquare.IsEnabled = false;
                        txtInspectSquareKoroz.IsEnabled = false;
                        txtVidKorozDamage.IsEnabled = false;
                        txtConculusion.IsEnabled = false;
                    }
                    break;
                default:
                    {
                        txtName.IsEnabled = true;
                        cldShurfDate.IsEnabled = true;
                        cldVisitDate.IsEnabled = true;
                        txtDefectoskopist.IsEnabled = true;
                        txtAgentLPU.IsEnabled = true;
                        txtYearExspl.IsEnabled = true;
                        txtCharacterRelief.IsEnabled = true;
                        ddlCharacterGrunt.IsEnabled = true;
                        txtDepth.IsEnabled = true;
                        txtHeight.IsEnabled = true;
                        txtLenght.IsEnabled = true;
                        txtLevelWater.IsEnabled = true;
                        txtResister.IsEnabled = true;
                        txtVoltage.IsEnabled = true;
                        ddlTypeIzol.IsEnabled = true;
                        ddlIzolState.IsEnabled = true;
                        txtNumberSkin.IsEnabled = true;
                        txtNumberObertka.IsEnabled = true;
                        ddlAvailabOverl.IsEnabled = true;
                        ddlAdherencPipe.IsEnabled = true;
                        ddlAvailabDamp.IsEnabled = true;
                        txtInspectSquare.IsEnabled = true;
                        txtInspectSquareKoroz.IsEnabled = true;
                        txtVidKorozDamage.IsEnabled = true;
                        txtConculusion.IsEnabled = true;
                    }
                    break;
            }

            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // по закрытию формы сохраняем данные (только если акт не подписан)
            if (Model.SignatureAkt == "0")
            {
                SaveDocumentPresentation();
            }

            Model.PropertyChanged -= Model_PropertyChanged;
        }

        public void SaveDocumentPresentation()
        {
            try
            {

                DocumentPresentationModel dpm = this.DataContext as DocumentPresentationModel;
                if (dpm != null)
                {
                    //функция сохранения паспорта
                    if (cldVisitDate.SelectedDate != null)
                    {
                        DateTime d1 = (DateTime)cldVisitDate.SelectedDate;

                        dpm.DataVisible = d1.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        dpm.DataVisible = "";
                    }
                    if (cldShurfDate.SelectedDate != null)
                    {
                        DateTime d1 = (DateTime)cldShurfDate.SelectedDate;

                        dpm.DataShurf = d1.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        dpm.DataShurf = "";
                    }

                    var d = dpm.GetFieldsAndValues();
                    string report = CBLL.SaveAllAct(dpm.KeyAkt, d);
                }

            }
            catch (Exception ee)
            {
                Model.Report("DocumentPresentation.SaveDocumentPresentation " + ee);
                throw;
            }
        }


        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("DataVisible".Equals(e.PropertyName))
            {
                if (dpm2.DataVisible != "")
                {
                    cldVisitDate.SelectedValue = DateTime.Parse(dpm2.DataVisible);
                }

            }
            if ("Print".Equals(e.PropertyName))
            {


                if (Model.Print)
                {
                    PrintDocumentPresentation();
                    Model.Print = false;
                }


            }
            if ("SignatureAkt".Equals(e.PropertyName))
            {
                switch (Model.SignatureAkt)
                {
                    case "1":
                    case "2":
                        {
                            SaveDocumentPresentation();

                            txtName.IsEnabled = false;
                            cldShurfDate.IsEnabled = false;
                            cldVisitDate.IsEnabled = false;
                            txtDefectoskopist.IsEnabled = false;
                            txtAgentLPU.IsEnabled = false;
                            txtYearExspl.IsEnabled = false;
                            txtCharacterRelief.IsEnabled = false;
                            ddlCharacterGrunt.IsEnabled = false;
                            txtDepth.IsEnabled = false;
                            txtHeight.IsEnabled = false;
                            txtLenght.IsEnabled = false;
                            txtLevelWater.IsEnabled = false;
                            txtResister.IsEnabled = false;
                            txtVoltage.IsEnabled = false;
                            ddlTypeIzol.IsEnabled = false;
                            ddlIzolState.IsEnabled = false;
                            txtNumberSkin.IsEnabled = false;
                            txtNumberObertka.IsEnabled = false;
                            ddlAvailabOverl.IsEnabled = false;
                            ddlAdherencPipe.IsEnabled = false;
                            ddlAvailabDamp.IsEnabled = false;
                            txtInspectSquare.IsEnabled = false;
                            txtInspectSquareKoroz.IsEnabled = false;
                            txtVidKorozDamage.IsEnabled = false;
                            txtConculusion.IsEnabled = false;
                        }
                        break;
                    default:
                        {
                            txtName.IsEnabled = true;
                            cldShurfDate.IsEnabled = true;
                            cldVisitDate.IsEnabled = true;
                            txtDefectoskopist.IsEnabled = true;
                            txtAgentLPU.IsEnabled = true;
                            txtYearExspl.IsEnabled = true;
                            txtCharacterRelief.IsEnabled = true;
                            ddlCharacterGrunt.IsEnabled = true;
                            txtDepth.IsEnabled = true;
                            txtHeight.IsEnabled = true;
                            txtLenght.IsEnabled = true;
                            txtLevelWater.IsEnabled = true;
                            txtResister.IsEnabled = true;
                            txtVoltage.IsEnabled = true;
                            ddlTypeIzol.IsEnabled = true;
                            ddlIzolState.IsEnabled = true;
                            txtNumberSkin.IsEnabled = true;
                            txtNumberObertka.IsEnabled = true;
                            ddlAvailabOverl.IsEnabled = true;
                            ddlAdherencPipe.IsEnabled = true;
                            ddlAvailabDamp.IsEnabled = true;
                            txtInspectSquare.IsEnabled = true;
                            txtInspectSquareKoroz.IsEnabled = true;
                            txtVidKorozDamage.IsEnabled = true;
                            txtConculusion.IsEnabled = true;
                        }
                        break;
                }


            }

        }

        private void PrintDocumentPresentation()
        {
            try
            {


                DocumentPresentationModel dpm5 = DataContext as DocumentPresentationModel;
                Model.DPM = dpm5;

                if (Model.DPM == null)
                {
                    MessageBox.Show("данные  документа не определены");
                }
                else
                {
                    PrintAkt pa = new PrintAkt();
                    pa.Show();
                    Application.Current.MainWindow = pa;
                }
            }
            catch (Exception ee)
            {

                MessageBox.Show("ошибка в открытии формы печать документа" + ee);
            }

        }


    }
}

//private void btnCreatePassport_Click(object sender, RoutedEventArgs e)
//        {
//            //PrintShurf printShurf = new PrintShurf();
//            //printShurf.DataContext = this.DataContext as DocumentPresentationModel;
//            //bool? result = printShurf.ShowDialog();

//            Model.DPM = this.DataContext as DocumentPresentationModel;
//            PrintAkt pa = new PrintAkt();

//            // pa.DataContext = ToImageSource(canvas).ToString() ;
//            pa.Show();
//            Application.Current.MainWindow = pa;
//        }