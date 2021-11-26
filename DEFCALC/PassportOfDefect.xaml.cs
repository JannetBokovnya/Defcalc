using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DEFCALC.DataModel;
using DrawPipe.DataModel;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for PassportOfDefect.xaml
    /// </summary>
    public partial class PassportOfDefect : Window
    {

        private Image img;
        private BitmapImage btimg;
        private string _uri = "";
        private Uri u;

        public PassportOfDefect()
        {
            InitializeComponent();
        }

        PasportDefectModel Model { get { return DataContext as PasportDefectModel; } }
        MainViewModel MainModel { get { return App.Model; } }


        private CharacteristicPipeModel cpm_baseOnDefect;
        private double PrevSeam_Dist_old = 0d;
        private double NextSeam_old = 0d;
        private double PreviousReper_old = 0d;
        private double NextReper_old = 0d;

        private string report = "";
        private string s = "";
        private string ss = "";
        private PasportDefectModel pdm = new PasportDefectModel();
        private GroupTypeDefect gtd = new GroupTypeDefect();


        /// <summary>
        /// Клик на кнопке "Создать паспорт дефекта"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreatePassport_Click(object sender, RoutedEventArgs e)
        {
            if ((MainModel.SignatureAkt == "0") || (string.IsNullOrEmpty(MainModel.SignatureAkt)))
            {

                if (cldataDefectUstr.SelectedDate != null)
                {
                    DateTime d1 = (DateTime)cldataDefectUstr.SelectedDate;
                    pdm.DataUstrDefect = d1.ToString("dd.MM.yyyy");
                }
                else
                {
                    pdm.DataUstrDefect = "";
                }


                StreamWriter writerClear = new System.IO.StreamWriter(@"CalcOneDefect.txt", false);
                writerClear.Write("");
                writerClear.Close();

                //если паспорт новый - то создаем новый дефект
                string _keyNewdefect = "";

                if (Model.KeyDefect == "0")
                {
                    //создаем новый дефект
                    var newdefect = pdm.GetFieldsAndValues();
                    _keyNewdefect = CBLL.SavePasportDefectModel(pdm.KeyDefect, newdefect, Model.KeyPipe, "5");
                }

                //расчитываем только коррозионные дефекты

                string typeDefect = TypeDefect(pdm.SelectedTypeDefect.Name, gtd);

                if (typeDefect == "коррозионный дефект")
                {

                    if (cpm_baseOnDefect.Depth_pipe != "")
                    {
                        //расчет дефекта
                        Process proc = new Process();
                        try
                        {

                            //получаем данные по трубе
                            JasonDefectCalc.TubeJason tube = DataTubeOnCalc();
                            //получаем данные по дефектам на трубе(для расчетов)
                            JasonDefectCalc.Defect df = DataTubeDefectOnCalc();


                            tube.Tube.Defects.Add(df);

                            string jsonString = JsonHelper.JsonSerializer<JasonDefectCalc.TubeJason>(tube);
                            jsonString = jsonString.Replace("DefectoscopeJson", "Defectoscope");
                            //--------------------------------------
                            //для ASME
                            string jsonStringASME = "ASME" + jsonString;
                            //proc.StartInfo.FileName ="C:\test>"+ @"java "+  "-jar " + "Calc_Res.jar" ;
                            //string s = " \"" + jsonStringASME + "\"";
                            //proc.StartInfo.FileName = @"java";
                            // proc.StartInfo.FileName = @"C:\Program Files\Java\jre1.7.0_10\bin\java.exe";
                            StreamWriter writeJava = new StreamWriter(@"JavaFilename.txt", true);
                            writeJava.WriteLine(" proc.StartInfo.FileName - ");
                            writeJava.WriteLine(System.Environment.GetEnvironmentVariable("java"));
                            writeJava.WriteLine("ASME : ");
                            writeJava.WriteLine(jsonStringASME);
                            writeJava.Close();

                            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("java");
                            s = "-jar " + "Calc_Res.jar" + " \"" + jsonStringASME + "\"";
                            proc.StartInfo.Arguments = (s);
                            proc.StartInfo.CreateNoWindow = true;
                            // proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.RedirectStandardOutput = true;
                            proc.Start();
                            ss = proc.StandardOutput.ReadToEnd();
                            StreamWriter writer = new System.IO.StreamWriter(@"CalcOneDefect.txt", true);
                            writer.WriteLine("ASME : ");
                            writer.WriteLine(jsonStringASME);
                            writer.WriteLine(ss);
                            writer.Close();


                            OutDefectCalc outDefectCalcASME = new OutDefectCalc();
                            outDefectCalcASME = JsonHelper.JsonDeserialize<OutDefectCalc>(ss);

                            //-----------------------------------------------

                            //DNV
                            string jsonStringDNV = "DNV_" + jsonString;
                            //proc.StartInfo.FileName = @"java";
                            //proc.StartInfo.FileName = @"C:\Program Files\Java\jre1.7.0_10\bin\java.exe";
                            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("java");
                            s = "";
                            s = "-jar " + "Calc_Res.jar" + " \"" + jsonStringDNV + "\"";
                            proc.StartInfo.Arguments = (s);
                            proc.StartInfo.CreateNoWindow = true;

                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.RedirectStandardOutput = true;
                            proc.Start();
                            ss = proc.StandardOutput.ReadToEnd();

                            OutDefectCalc outDefectCalcDNV = new OutDefectCalc();
                            outDefectCalcDNV = JsonHelper.JsonDeserialize<OutDefectCalc>(ss);


                            StreamWriter writerdnv = new System.IO.StreamWriter(@"CalcOneDefect.txt", true);
                            writerdnv.WriteLine("DNV_ : ");
                            writerdnv.WriteLine(jsonStringDNV);
                            writerdnv.WriteLine(ss);
                            writerdnv.Close();
                            //--------------------------------------------------------------

                            //RSTRENG
                            string jsonStringRSTRENG = "RSTR" + jsonString;

                            //proc.StartInfo.FileName = @"java";
                            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("java");
                            //proc.StartInfo.FileName = @"C:\Program Files\Java\jre1.7.0_10\bin\java.exe";
                            s = "";

                            s = "-jar " + "Calc_Res.jar" + " \"" + jsonStringRSTRENG + "\"";
                            proc.StartInfo.Arguments = (s);
                            proc.StartInfo.CreateNoWindow = true;

                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.RedirectStandardOutput = true;
                            proc.Start();
                            ss = proc.StandardOutput.ReadToEnd();

                            OutDefectCalc outDefectCalcRSTRENG = new OutDefectCalc();
                            outDefectCalcRSTRENG = JsonHelper.JsonDeserialize<OutDefectCalc>(ss);

                            StreamWriter writerrstreng = new System.IO.StreamWriter(@"CalcOneDefect.txt", true);
                            writerrstreng.WriteLine("RSTR : ");
                            writerrstreng.WriteLine(jsonStringRSTRENG);
                            writerrstreng.WriteLine(ss);
                            writerrstreng.Close();
                            //--------------------------------------------
                            // txtjava.Text = ss;
                            proc.WaitForExit();
                            //Save();


                            //если все расчиталось - то записываем результаты в базу
                            var dnv = outDefectCalcDNV.GetFieldsAndValues();
                            var rstreng = outDefectCalcRSTRENG.GetFieldsAndValues();
                            var d = outDefectCalcASME.GetFieldsAndValues();

                            if (Model.KeyDefect == "0")
                            {
                                if (_keyNewdefect != "")
                                {
                                    report = CBLL.SaveCalcdefectOnASME(_keyNewdefect, "1", dnv);
                                    report = CBLL.SaveCalcdefectOnASME(_keyNewdefect, "3", rstreng);
                                    report = CBLL.SaveCalcdefectOnASME(_keyNewdefect, "2", d);
                                }

                            }
                            else
                            {
                                report = CBLL.SaveCalcdefectOnASME(Model.KeyDefect, "1", dnv);
                                report = CBLL.SaveCalcdefectOnASME(Model.KeyDefect, "3", rstreng);
                                report = CBLL.SaveCalcdefectOnASME(Model.KeyDefect, "2", d);
                            }


                            DialogResult = true;
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("Ошибка в расчете  дефекта " + ee);
                            DialogResult = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(" Нельзя расчитать дефект, т.к. не известа толщина стенки трубы ");
                        DialogResult = true;
                    }
                }
                else
                {
                    MessageBox.Show(" Нельзя расчитать дефект, т.к. тип дефекта не известен ");
                    DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("Акт подписан нельзя добавлять, редактировать дефекты");

            }

        }


        private string TypeDefect(string nameTypeDefect, GroupTypeDefect listTypeDefect)
        {
            string groupTypeDefect = "";

            groupTypeDefect = listTypeDefect.GetGroupTypeDefect(nameTypeDefect);

            return groupTypeDefect;
        }


        private JasonDefectCalc.TubeJason DataTubeOnCalc()
        {
            JasonDefectCalc.TubeJason tube = new JasonDefectCalc.TubeJason();



            tube.Tube = new JasonDefectCalc.Tube();
            tube.Tube.Defects = new List<JasonDefectCalc.Defect>();
            tube.Defectoscope = new JasonDefectCalc.Defectoscope();
            tube.Coeffs = new JasonDefectCalc.Coeffs();

            tube.Tube.TubeWidth = (((cpm_baseOnDefect.Depth_pipe) == "")
                                       ? "18.0"
                                       : (cpm_baseOnDefect.Depth_pipe).Replace(",", "."));
            tube.Tube.Diam = (cpm_baseOnDefect.Diam).Replace(",", ".");
            ; // "1400.0";
            tube.Tube.WorkPress = (((cpm_baseOnDefect.MaxPressure) == "")
                                       ? "6.0"
                                       : (cpm_baseOnDefect.MaxPressure).Replace(",", ".")); // "6.0";
            tube.Tube.PredTek = (((cpm_baseOnDefect.Range_fluid) == "")
                                     ? "470.0"
                                     : (cpm_baseOnDefect.Range_fluid).Replace(",", ".")); // "470.0";
            tube.Tube.RealWorkPress = (((cpm_baseOnDefect.Pressure) == "")
                                           ? "7.0"
                                           : (cpm_baseOnDefect.Pressure).Replace(",", ".")); // "7.0";
            tube.Tube.SigmaL = "500"; //??????
            tube.Tube.PredProch = (((cpm_baseOnDefect.Range_stranght) == "")
                                       ? "531.0"
                                       : (cpm_baseOnDefect.Range_stranght).Replace(",", "."));
            tube.Tube.PredNapr = "600"; //???????????????
            tube.Tube.Puas = (((cpm_baseOnDefect.KoefPuanson) == "")
                                  ? "0.3"
                                  : (cpm_baseOnDefect.KoefPuanson).Replace(",", "."));
            tube.Tube.LengthReg = (((cpm_baseOnDefect.LenghtPipe) == "")
                                       ? "14"
                                       : (cpm_baseOnDefect.LenghtPipe).Replace(",", "."));
            tube.Tube.PipeCat = cpm_baseOnDefect.Getketegor_Uch_OnKey(cpm_baseOnDefect.SelectedKetegor_uch.Key);
            // "0.9";//??? стандарт??; //категория участка трубопровода(словарь разобраться!!!)
            tube.Tube.LinRas = (((cpm_baseOnDefect.KoefLinExpansion) == "")
                                    ? "1.5E-5"
                                    : (cpm_baseOnDefect.KoefLinExpansion).Replace(",", "."));
            tube.Tube.Jung = (((cpm_baseOnDefect.ModulUng) == "")
                                  ? "210000.0"
                                  : (cpm_baseOnDefect.ModulUng).Replace(",", "."));
            tube.Tube.Tp = "0";
            tube.Tube.Teks = "0";
            tube.Tube.Pproject = "16.0"; //пока стандартно - потом меняется!!!

            tube.Defectoscope.DefectoscopeJson = "0.5"; //??? стандарт??
            tube.Coeffs.DesignCoef = "0.7"; //??? стандарт??
            tube.Coeffs.KoefT = "0.9"; //??? стандарт??
            tube.Coeffs.ModelCoef = "1"; //??? стандарт??
            tube.DefType = "2"; //??? стандарт откуда брать данные?

            return tube;
        }

        private JasonDefectCalc.Defect DataTubeDefectOnCalc()
        {

            JasonDefectCalc.Defect df = new JasonDefectCalc.Defect();

            df.CorDepth = (((Model.Depth.ToString()) == "") ? "2.0" : (Model.Depth.ToString()).Replace(",", "."));
            df.MaxCorLength = (((Model.W.ToString()) == "") ? "16.0" : (Model.W.ToString()).Replace(",", "."));
            df.CorWidth = (((Model.H.ToString()) == "") ? "20.0" : (Model.H.ToString()).Replace(",", "."));
            df.Angle = (((Model.Angle.ToString()) == "")
                            ? "6.83333"
                            : (Model.Angle.ToString()).Replace(",", "."));
            df.Kilometer = (((Model.PrevSeam_Dist.ToString()) == "")
                                ? "0"
                                : (Model.PrevSeam_Dist.ToString()).Replace(",", "."));
            df.Type = "0.0"; //откуда берется тип!!!!!!!!!!!!!!!!!
            df.ID = Model.KeyDefect; // "5726";
            return df;
        }

        /// <summary>
        /// Клик на кнопке "Отменить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pdm = this.DataContext as PasportDefectModel;
            cpm_baseOnDefect = CBLL.GetCharacteristicPipe(Model.KeyPipe, MainModel.KeyAkt);

            if (pdm.DataUstrDefect != "")
            {
                cldataDefectUstr.SelectedValue = DateTime.Parse(pdm.DataUstrDefect);
            }
           



            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
            //сохраняем данные 
            if (!string.IsNullOrEmpty(txtPreviousSeam.Text))
            {
                PrevSeam_Dist_old = double.Parse(txtPreviousSeam.Text);//Model.PrevSeam_Dist;    
            }
            if (!string.IsNullOrEmpty(txtToNextSeam.Text))
            {
                NextSeam_old = double.Parse(txtToNextSeam.Text);
            }

            if (!string.IsNullOrEmpty(txtPreviousReper.Text))
            {
                PreviousReper_old = double.Parse(txtPreviousReper.Text);
            }

            if (!string.IsNullOrEmpty(txtNextReper.Text))
            {
                NextReper_old = double.Parse(txtNextReper.Text);
            }


            if ((MainModel.SignatureAkt == "1") || (MainModel.SignatureAkt == "2"))
            {
                txtName.IsEnabled = false;
                ddlType.IsEnabled = false;
                ddlCountType.IsEnabled = false;
                txtLong.IsEnabled = false;
                txtWidth.IsEnabled = false;
                txtDeep.IsEnabled = false;
                txtAnglePlace.IsEnabled = false;
                ddlPlaceInMetall.IsEnabled = false;
                ddlDefectUstr.IsEnabled = false;
                txtPreviousSeam.IsEnabled = false;
                txtStressKorrDescription.IsEnabled = false;
                ddlMethodOfRepear.IsEnabled = false;

                img = new Image();
                btimg = new BitmapImage();
                _uri = "";
                _uri = "../ImagesIcon/ok_disabled.png";
                u = new Uri(_uri, UriKind.Relative);
                btimg.BeginInit();
                btimg.UriSource = u;
                btimg.EndInit();
                img.Source = btimg;
                btnCreatePassport.Content = img;
                btnCreatePassport.IsEnabled = false;



            }

        }

        void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("PrevSeam_Dist".Equals(e.PropertyName))
            {
                try
                {

                    string checkedValue = txtPreviousSeam.Text;
                    string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                    checkedValue = checkedValue.Replace(".", separator);
                    checkedValue = checkedValue.Replace(",", separator);

                    //double prevSeam_Dist_new = double.Parse(txtPreviousSeam.Text);
                    double prevSeam_Dist_new = double.Parse(checkedValue);
                    double difference = PrevSeam_Dist_old - prevSeam_Dist_new;

                    if (!string.IsNullOrEmpty(txtToNextSeam.Text) && (txtToNextSeam.Text != "0"))
                        txtToNextSeam.Text = (NextSeam_old + difference).ToString();
                    if (!string.IsNullOrEmpty(txtPreviousReper.Text) && (txtPreviousReper.Text != "0"))
                        txtPreviousReper.Text = (PreviousReper_old - difference).ToString();
                    if (!string.IsNullOrEmpty(txtNextReper.Text) && (txtNextReper.Text != "0"))
                        txtNextReper.Text = (NextReper_old + difference).ToString();

                   
                }
                catch (Exception)
                {

                    throw;
                }

            }


        }
        /// <summary>
        /// клик на комбобокс - дефект устранен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlDefectUstr_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if ((ddlDefectUstr.SelectedItem != null) && (Model.SelectedDefectUstr.Key != "-1"))
            {
                string kk = Model.SelectedDefectUstr.Key;
                if (kk == "1")
                {
                    cldataDefectUstr.IsReadOnly = false;
                    ddlMethodUstrDef.IsReadOnly = false;
                    ddlMethodUstrDef.IsEnabled = true;
                    cldataDefectUstr.IsEnabled = true;
                }
                else
                {
                    if (pdm.SelectedMetodUstr2 != null)
                    {
                        pdm.SelectedMetodUstr2 = pdm.DictListMetodUstr2[0];
                        cldataDefectUstr.SelectedDate = null;
                        cldataDefectUstr.IsReadOnly = true;
                        cldataDefectUstr.IsEnabled = false;
                        ddlMethodUstrDef.IsReadOnly = true;
                        ddlMethodUstrDef.IsEnabled = false;
                    }
                   
                }

            }
            else
            {
                if (pdm.SelectedMetodUstr2 != null)
                {
                    pdm.SelectedMetodUstr2 = pdm.DictListMetodUstr2[0];
                    ddlMethodUstrDef.IsReadOnly = true;
                    cldataDefectUstr.SelectedDate = null;
                    cldataDefectUstr.IsReadOnly = true;
                    cldataDefectUstr.IsEnabled = false;
                    ddlMethodUstrDef.IsReadOnly = true;
                    ddlMethodUstrDef.IsEnabled = false;
                }
               
            }

        }

        private void TxtDeep_OnLostFocus(object sender, RoutedEventArgs e)
        {
            //по потери фокуса глубины дефекта расчитываем потери металла cpm_baseOnDefect
            if (!string.IsNullOrEmpty(txtDeep.Text))
                txtLostOfMetall.Text = (GetLossMetal(txtDeep.Text, cpm_baseOnDefect.Depth_pipe)).ToString();
        }
        /// <summary>
        /// расчет потери металла
        /// </summary>
        /// <param name="valueDephDef"></param>
        /// <param name="valueDephPipe"></param>
        /// <returns></returns>
        public static double GetLossMetal(string valueDephDef, string valueDephPipe)
        {
            try
            {

                string checkedvalueDephDef = valueDephDef;
                string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                checkedvalueDephDef = checkedvalueDephDef.Replace(".", separator);
                checkedvalueDephDef = checkedvalueDephDef.Replace(",", separator);

                double dephDef = double.Parse(checkedvalueDephDef);
                double dephPipe = double.Parse(valueDephPipe);

                return Math.Round((dephDef / dephPipe), 3);
            }
            catch (Exception)
            {
                return 0d;
            }

        }
    }
}

