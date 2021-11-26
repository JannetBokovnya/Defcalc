using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using DrawPipe.DataModel;
using Telerik.Windows.Controls;

namespace DEFCALC.DataModel
{
    public class CalcDefect : BaseModel
    {

        MainViewModel Model { get { return App.Model; } }
        private CharacteristicPipeModel cpm_baseOnDefect;
        private ObservableCollection<GridDefect> gridDefectList;
        private ObservableCollection<GridDefect> gridOnePipeDefectList;
        private ObservableCollection<GridPipe> gridPipeList;
        private string resultGroupDef = "";
        private string s = "";
        private string ss = "";
        private string err = "";
        private string report = "";
        private GroupTypeDefect gtd = new GroupTypeDefect();
        public OutGroupDefects.RootObject outGroupDefects;

        public CalcDefect()
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;


        }
        //расчет дефектов по днв и т.д.
        public void InitCalc()
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
            //список всех труб на выбранном акте
            Model.GridPipeListOnSelectPipeFill();
            //список всех дефектов
            Model.GridDefectListOnControlCalc();

            gridOnePipeDefectList = new ObservableCollection<GridDefect>();
            gridDefectList = new ObservableCollection<GridDefect>();
            gridDefectList = Model.GridDefectList;
            gridPipeList = Model.GridPipeListOnSelectPipeList;
            //цикл по ключам труб
            foreach (GridPipe pipe in gridPipeList)
            {
                //характеристика данной трубы
                cpm_baseOnDefect = CBLL.GetCharacteristicPipe(pipe.KEYPIPE, Model.KeyAkt);
                if (cpm_baseOnDefect.Depth_pipe == "")
                {
                    //MessageBox.Show("Нельзя расчитать дефекты на трубе, т.к. не задана глубина трубы № " +
                    //                cpm_baseOnDefect.NumberPipe);
                    if (gridDefectList != null)
                    {
                        //все дефекты на выбранных трубах(акт)
                        gridOnePipeDefectList.Clear();
                        foreach (GridDefect defect in gridDefectList)
                        {
                            if (pipe.KEYPIPE == defect.KeySegmentOnDefect)
                            {
                                //удаляем расчеты на дефектов на трубе
                                string t = CBLL.DeleteCalcDefect(defect.KeyDefect);

                            }
                        }
                    }
                }
                else
                {


                    if (gridDefectList != null)
                    {
                        //все дефекты на выбранных трубах(акт)
                        gridOnePipeDefectList.Clear();
                        foreach (GridDefect defect in gridDefectList)
                        {
                            if (pipe.KEYPIPE == defect.KeySegmentOnDefect)
                            {
                                gridOnePipeDefectList.Add(
                                    new GridDefect(defect.KeyDefect, defect.Angle, defect.W, defect.H, defect.ShiftX,
                                                   defect.KeySegmentOnDefect, defect.TypeDefect, defect.Depth,
                                                   defect.HintDefect, defect.NumberDefect, defect.Poterimetal, "",
                                                   "", "", "")
                                    );
                            }
                        }
                        if (gridOnePipeDefectList.Count != 0)
                        {
                            CalcAllDefect(cpm_baseOnDefect, gridOnePipeDefectList);
                        }

                    }
                }
            }

            if (err != "")
            {
                MessageBox.Show(err);
            }


        }


        //расчет групп дефектов
        public void CalcColony_Group()
        {
            //расчитываем группы и колонии дефектов
            gridOnePipeDefectList = new ObservableCollection<GridDefect>();
            gridPipeList = Model.GridPipeListOnSelectPipeList;
            //список всех дефектов на трубах акта
            gridDefectList = Model.GridDefectList;

            Process proc = new Process();
            try
            {
                foreach (GridPipe pipe in gridPipeList)
                {
                    //получаем данные по конкретной трубе

                    cpm_baseOnDefect = CBLL.GetCharacteristicPipe(pipe.KEYPIPE, Model.KeyAkt);

                    if (cpm_baseOnDefect.Depth_pipe == "")
                    {
                        resultGroupDef = resultGroupDef + cpm_baseOnDefect.NumberPipe + ", ";
                        string resultdelete = CBLL.DeleteColonyDefects(pipe.KEYPIPE);
                    }
                    else
                    {
                        //общие данные для расчетов(колонии и группы) по трубе(труба)
                        JasonDefectCalc.TubeJason tube = DataTubeOnCalc(cpm_baseOnDefect);

                        //дефекты для одной трубы по ключу трубы
                        gridOnePipeDefectList.Clear();

                        foreach (GridDefect defect in gridDefectList)
                        {
                          

                            if (pipe.KEYPIPE == defect.KeySegmentOnDefect)
                            {
                                gridOnePipeDefectList.Add(
                                    new GridDefect(defect.KeyDefect, defect.Angle, defect.W, defect.H, defect.ShiftX,
                                                   defect.KeySegmentOnDefect, defect.TypeDefect, defect.Depth,
                                                   defect.HintDefect, defect.NumberDefect, defect.Poterimetal, "", "", "",
                                                   "")
                                    );

                            }
                        }
                        if (gridOnePipeDefectList.Count != 0)
                        {

                            foreach (GridDefect defect in gridOnePipeDefectList)
                            {
                                //расчитываем только коррозионные дефекты
                                string typeDefect = TypeDefect(defect.TypeDefect, gtd);

                                if (typeDefect == "коррозионный дефект")
                                {
                                    //получаем данные по дефектам на трубе(для расчетов)
                                    JasonDefectCalc.Defect df = DataTubeDefectOnCalc(defect);
                                    tube.Tube.Defects.Add(df);
                                }
                            }


                            string jsonString = JsonHelper.JsonSerializer<JasonDefectCalc.TubeJason>(tube);
                            jsonString = jsonString.Replace("DefectoscopeJson", "Defectoscope");
                            string jsonStringASME = "DNVI" + jsonString;
                            //запускаем расчет
                            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("java");
                            string s = "-jar " + "Calc_Res.jar" + " \"" + jsonStringASME + "\"";
                            proc.StartInfo.Arguments = (s);
                            proc.StartInfo.CreateNoWindow = true;
                            // proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.RedirectStandardOutput = true;
                            proc.Start();
                            string ss = proc.StandardOutput.ReadToEnd();

                            //получили структуру групп дефектов и колоний
                            outGroupDefects = JsonHelper.JsonDeserialize<OutGroupDefects.RootObject>(ss);

                            //удаляем все колонии и группы дефектов на данной трубе
                            string resultdelete = CBLL.DeleteColonyDefects(pipe.KEYPIPE);
                            if (resultdelete == "")
                            {
                                //сохраняем в таблицу  структуру  колоний
                                for (int ii = 0; ii < outGroupDefects.groupingResult.colonies.Count; ii++)
                                {
                                    var dcoloni = GetFieldsAndValuesColony(outGroupDefects.groupingResult.colonies[ii],
                                                                           pipe.KEYPIPE);
                                    string keyColoni = "";
                                    keyColoni = CBLL.SaveColonyDefects(dcoloni, pipe.KEYPIPE);
                                }

                                //сохраняем в таблицу структуру групп
                                for (int i = 0; i < outGroupDefects.groupingResult.groups.Count; i++)
                                {
                                    var dgroup = GetFieldsAndValuesGroup(outGroupDefects.groupingResult.groups[i],
                                                                         pipe.KEYPIPE);
                                    string keyGroup = "";
                                    keyGroup = CBLL.SaveGroupDefects(dgroup, pipe.KEYPIPE);
                                }
                            }
                            else
                            {
                                Model.Report("Данные в базу не могут записаться");
                            }
                        }
                    }
                }

            }

            catch (Exception ee)
            {
                MessageBox.Show("Ошибка в расчете групп дефектов ");

            }
            if (resultGroupDef != "")
            {
                resultGroupDef = resultGroupDef.Substring(0, resultGroupDef.Length - 2);
                MessageBox.Show("Нельзя расчитать группы дефектов, т.к. на трубах № \n " + resultGroupDef + "\n не задана толщина стенки трубы ");
            }

        }

        /// <summary>
        /// данные по трубе (для расчетов)
        /// </summary>
        /// <returns></returns>
        private JasonDefectCalc.TubeJason DataTubeOnCalc(CharacteristicPipeModel cpm_baseOnDefectOnPipe)
        {
            JasonDefectCalc.TubeJason tube = new JasonDefectCalc.TubeJason();



            tube.Tube = new JasonDefectCalc.Tube();
            tube.Tube.Defects = new List<JasonDefectCalc.Defect>();
            tube.Defectoscope = new JasonDefectCalc.Defectoscope();
            tube.Coeffs = new JasonDefectCalc.Coeffs();

            tube.Tube.TubeWidth = (((cpm_baseOnDefectOnPipe.Depth_pipe) == "")
                                       ? "18.0"
                                       : (cpm_baseOnDefectOnPipe.Depth_pipe).Replace(",", "."));
            tube.Tube.Diam = (cpm_baseOnDefectOnPipe.Diam).Replace(",", ".");
            ; // "1400.0";
            tube.Tube.WorkPress = (((cpm_baseOnDefectOnPipe.MaxPressure) == "")
                                       ? "6.0"
                                       : (cpm_baseOnDefectOnPipe.MaxPressure).Replace(",", ".")); // "6.0";
            tube.Tube.PredTek = (((cpm_baseOnDefectOnPipe.Range_fluid) == "")
                                     ? "470.0"
                                     : (cpm_baseOnDefectOnPipe.Range_fluid).Replace(",", ".")); // "470.0";
            tube.Tube.RealWorkPress = (((cpm_baseOnDefectOnPipe.Pressure) == "")
                                           ? "7.0"
                                           : (cpm_baseOnDefectOnPipe.Pressure).Replace(",", ".")); // "7.0";
            tube.Tube.SigmaL = "500"; //??????
            tube.Tube.PredProch = (((cpm_baseOnDefectOnPipe.Range_stranght) == "")
                                       ? "531.0"
                                       : (cpm_baseOnDefectOnPipe.Range_stranght).Replace(",", "."));
            tube.Tube.PredNapr = "600"; //???????????????
            tube.Tube.Puas = (((cpm_baseOnDefectOnPipe.KoefPuanson) == "")
                                  ? "0.3"
                                  : (cpm_baseOnDefectOnPipe.KoefPuanson).Replace(",", "."));
            tube.Tube.LengthReg = (((cpm_baseOnDefectOnPipe.LenghtPipe) == "")
                                       ? "14"
                                       : (cpm_baseOnDefectOnPipe.LenghtPipe).Replace(",", "."));
            tube.Tube.PipeCat = cpm_baseOnDefect.Getketegor_Uch_OnKey(cpm_baseOnDefect.SelectedKetegor_uch.Key); //категория участка трубопровода(словарь разобраться!!!)
            tube.Tube.LinRas = (((cpm_baseOnDefectOnPipe.KoefLinExpansion) == "")
                                    ? "1.5E-5"
                                    : (cpm_baseOnDefectOnPipe.KoefLinExpansion).Replace(",", "."));
            tube.Tube.Jung = (((cpm_baseOnDefectOnPipe.ModulUng) == "")
                                  ? "210000.0"
                                  : (cpm_baseOnDefectOnPipe.ModulUng).Replace(",", "."));
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

        /// <summary>
        /// данные по дефектам на трубе(для расчетов)
        /// </summary>
        /// <returns></returns>
        private JasonDefectCalc.Defect DataTubeDefectOnCalc(GridDefect defect)
        {
            JasonDefectCalc.Defect df = new JasonDefectCalc.Defect();
            df.CorDepth = (((defect.Depth) == "")
                                           ? "2.0"
                                           : (defect.Depth).Replace(",", "."));
            df.MaxCorLength = (((defect.W.ToString()) == "")
                                   ? "16.0"
                                   : (defect.W.ToString()).Replace(",", "."));
            df.CorWidth = (((defect.H.ToString()) == "")
                               ? "20.0"
                               : (defect.H.ToString()).Replace(",", "."));
            df.Angle = (((defect.Angle.ToString()) == "")
                            ? "6.83333"
                            : (defect.Angle.ToString()).Replace(",", "."));
            df.Kilometer = (((defect.ShiftX.ToString()) == "")
                                ? "0"
                                : (defect.ShiftX.ToString()).Replace(",", "."));
            df.Type = "0.0"; //откуда берется тип!!!!!!!!!!!!!!!!!
            df.ID = defect.KeyDefect;
            return df;
        }

        /// <summary>
        /// возвращает поле-значение для инсерта в базу групп дефектов
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetFieldsAndValuesGroup(OutGroupDefects.Group groupDefects, string keyPipe)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(groupDefects.rightX.ToString()))
            {
                d["nRight"] = (groupDefects.rightX.ToString()).Replace(",", "."); 
            }

            if (!string.IsNullOrWhiteSpace(groupDefects.leftX.ToString()))
            {
                d["nLeft"] = (groupDefects.leftX.ToString()).Replace(",", "."); 
            }

            if (!string.IsNullOrWhiteSpace(groupDefects.upPhi.ToString()))
            {
                d["nUpRad"] = (groupDefects.upPhi.ToString()).Replace(",", ".");
            }

            if (!string.IsNullOrWhiteSpace(groupDefects.downPhi.ToString()))
            {
                d["nDownRad"] = (groupDefects.downPhi.ToString()).Replace(",", "."); ;
            }

            if (!string.IsNullOrWhiteSpace(groupDefects.rightX.ToString()))
            {
                d["nKey_pipe"] = "'" + keyPipe + "'";
            }

            return d;
        }
        /// <summary>
        /// возвращает поле-значение для инсерта в базу колоний дефектов
        /// </summary>
        /// <param name="colonyGroupDefects"></param>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetFieldsAndValuesColony(OutGroupDefects.Colony colonyDefects, string keyPipe)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(colonyDefects.leftX.ToString()))
            {
                d["nLeft"] = (colonyDefects.leftX.ToString()).Replace(",", ".");
            }

            if (!string.IsNullOrWhiteSpace(colonyDefects.rightX.ToString()))
            {
                d["nRight"] = (colonyDefects.rightX.ToString()).Replace(",", ".");
            }

            if (!string.IsNullOrWhiteSpace(keyPipe))
            {
                d["nKey_pipe"] = "'" + keyPipe + "'";
            }

            return d;
        }



        //события 
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("OpencalcDefect".Equals(e.PropertyName))
            {

 
            }
        }



        private void CalcAllDefect(CharacteristicPipeModel onePipe,
                                   ObservableCollection<GridDefect> gridOnePipeDefectList)
        {
            StreamWriter writerClear = new System.IO.StreamWriter(@"CalcOneDefect.txt", false);
            writerClear.Write("");
            writerClear.Close();

            Process proc = new Process();
            JasonDefectCalc.Defect df = new JasonDefectCalc.Defect();
            JasonDefectCalc.TubeJason tube = new JasonDefectCalc.TubeJason();

            tube.Tube = new JasonDefectCalc.Tube();
            tube.Tube.Defects = new List<JasonDefectCalc.Defect>();
            tube.Defectoscope = new JasonDefectCalc.Defectoscope();
            tube.Coeffs = new JasonDefectCalc.Coeffs();

            tube.Tube.TubeWidth = (((cpm_baseOnDefect.Depth_pipe) == "")
                                       ? "18.0"
                                       : (cpm_baseOnDefect.Depth_pipe).Replace(",", "."));
            tube.Tube.Diam = (onePipe.Diam).Replace(",", ".");
            ; // "1400.0";
            tube.Tube.WorkPress = (((onePipe.MaxPressure) == "")
                                       ? "6.0"
                                       : (onePipe.MaxPressure).Replace(",", ".")); // "6.0";
            tube.Tube.PredTek = (((onePipe.Range_fluid) == "")
                                     ? "470.0"
                                     : (onePipe.Range_fluid).Replace(",", ".")); // "470.0";
            tube.Tube.RealWorkPress = (((onePipe.Pressure) == "")
                                           ? "7.0"
                                           : (onePipe.Pressure).Replace(",", ".")); // "7.0";
            tube.Tube.SigmaL = "500"; //??????
            tube.Tube.PredProch = (((onePipe.Range_stranght) == "")
                                       ? "531.0"
                                       : (onePipe.Range_stranght).Replace(",", "."));
            tube.Tube.PredNapr = "600"; //???????????????
            tube.Tube.Puas = (((onePipe.KoefPuanson) == "")
                                  ? "0.3"
                                  : (onePipe.KoefPuanson).Replace(",", "."));
            tube.Tube.LengthReg = (((onePipe.LenghtPipe) == "")
                                       ? "14"
                                       : (onePipe.LenghtPipe).Replace(",", "."));
            tube.Tube.PipeCat = onePipe.Getketegor_Uch_OnKey(cpm_baseOnDefect.SelectedKetegor_uch.Key);
            // "0.9";//??? стандарт??; //категория участка трубопровода(словарь разобраться!!!)
            tube.Tube.LinRas = (((onePipe.KoefLinExpansion) == "")
                                    ? "1.5E-5"
                                    : (onePipe.KoefLinExpansion).Replace(",", "."));
            tube.Tube.Jung = (((onePipe.ModulUng) == "")
                                  ? "210000.0"
                                  : (onePipe.ModulUng).Replace(",", "."));
            tube.Tube.Tp = "0";
            tube.Tube.Teks = "0";
            tube.Tube.Pproject = "16.0"; //пока стандартно - потом меняется!!!


            foreach (GridDefect defect in gridOnePipeDefectList)
            {

                //расчитываем только коррозионные дефекты
                string typeDefect = TypeDefect(defect.TypeDefect, gtd);

                if (typeDefect == "коррозионный дефект")
                {

                df.CorDepth = (((defect.Depth.ToString()) == "") ? "2.0" : (defect.Depth.ToString()).Replace(",", "."));
                df.MaxCorLength = (((defect.W.ToString()) == "") ? "16.0" : (defect.W.ToString()).Replace(",", "."));
                df.CorWidth = (((defect.H.ToString()) == "") ? "20.0" : (defect.H.ToString()).Replace(",", "."));
                df.Angle = (((defect.Angle.ToString()) == "")
                                ? "6.83333"
                                : (defect.Angle.ToString()).Replace(",", "."));
                df.Kilometer = (((defect.ShiftX.ToString()) == "")
                                    ? "0"
                                    : (defect.ShiftX.ToString()).Replace(",", "."));
                df.Type = "0.0"; //откуда берется тип!!!!!!!!!!!!!!!!!
                df.ID = defect.KeyDefect; // "5726";

                tube.Tube.Defects.Add(df);

                tube.Defectoscope.DefectoscopeJson = "0.5"; //??? стандарт??
                tube.Coeffs.DesignCoef = "0.7"; //??? стандарт??
                tube.Coeffs.KoefT =
                    cpm_baseOnDefect.GetTemp_Koef_onKey(cpm_baseOnDefect.SelectedTemperaturn_koef.Key);
                // "0.9";//??? стандарт??
                tube.Coeffs.ModelCoef = "1"; //??? стандарт??
                tube.DefType = "2"; //??? стандарт откуда брать данные?

                string jsonString = JsonHelper.JsonSerializer<JasonDefectCalc.TubeJason>(tube);
                jsonString = jsonString.Replace("DefectoscopeJson", "Defectoscope");
                //--------------------------------------
                //для ASME
                string jsonStringASME = "ASME" + jsonString;
                //proc.StartInfo.FileName ="C:\test>"+ @"java "+  "-jar " + "Calc_Res.jar" ;
                //string s = " \"" + jsonStringASME + "\"";
                //proc.StartInfo.FileName = @"java";
                // proc.StartInfo.FileName = @"C:\Program Files\Java\jre1.7.0_10\bin\java.exe";
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

                if (ss != "")
                {
                    OutDefectCalc outDefectCalcASME = new OutDefectCalc();
                    outDefectCalcASME = JsonHelper.JsonDeserialize<OutDefectCalc>(ss);
                    var d = outDefectCalcASME.GetFieldsAndValues();
                    report = CBLL.SaveCalcdefectOnASME(defect.KeyDefect, "2", d);
                }
                else
                {
                    err = err + "дефект:" + defect.NumberDefect + "  по ASME не расчитался\n";
                }

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

                if (ss != "")
                {
                    OutDefectCalc outDefectCalcDNV = new OutDefectCalc();
                    outDefectCalcDNV = JsonHelper.JsonDeserialize<OutDefectCalc>(ss);
                    var dnv = outDefectCalcDNV.GetFieldsAndValues();
                    report = CBLL.SaveCalcdefectOnASME(defect.KeyDefect, "1", dnv);

                    StreamWriter writerdnv = new System.IO.StreamWriter(@"CalcOneDefect.txt", true);
                    writerdnv.WriteLine("DNV_ : ");
                    writerdnv.WriteLine(jsonStringDNV);
                    writerdnv.WriteLine(ss);
                    writerdnv.Close();
                }
                else
                {
                    err = err + "дефект:" + defect.NumberDefect + "  по DNV не расчитался\n";
                }

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

                if (ss != "")
                {
                    OutDefectCalc outDefectCalcRSTRENG = new OutDefectCalc();
                    outDefectCalcRSTRENG = JsonHelper.JsonDeserialize<OutDefectCalc>(ss);
                    var rstreng = outDefectCalcRSTRENG.GetFieldsAndValues();
                    report = CBLL.SaveCalcdefectOnASME(defect.KeyDefect, "3", rstreng);

                    StreamWriter writerrstreng = new System.IO.StreamWriter(@"CalcOneDefect.txt", true);
                    writerrstreng.WriteLine("RSTR : ");
                    writerrstreng.WriteLine(jsonStringRSTRENG);
                    writerrstreng.WriteLine(ss);
                    writerrstreng.Close();
                }
                else
                {
                    err = err + "дефект:" + defect.NumberDefect + "  по RSTR не расчитался\n";
                }

                //--------------------------------------------
                // txtjava.Text = ss;
                proc.WaitForExit();

            }
        }

    }



        private string TypeDefect(string nameTypeDefect, GroupTypeDefect listTypeDefect)
        {
            string groupTypeDefect = "";

            groupTypeDefect = listTypeDefect.GetGroupTypeDefect(nameTypeDefect);

            return groupTypeDefect;
        }
    }
}

           //if ("OpencalcDefect".Equals(e.PropertyName))
           // {

                    //gridOnePipeDefectList = new ObservableCollection<GridDefect>();
                    //gridDefectList = new ObservableCollection<GridDefect>();
                    //gridDefectList = Model.GridDefectList;
                    //gridPipeList = Model.GridPipeListOnSelectPipeList;
                    ////цикл по ключам труб
                    //foreach (GridPipe pipe in gridPipeList)
                    //{
                    //    //характеристика данной трубы
                    //    cpm_baseOnDefect = CBLL.GetCharacteristicPipe(pipe.KEYPIPE, Model.KeyAkt);
                    //    if (cpm_baseOnDefect.Depth_pipe == "")
                    //    {
                    //        //MessageBox.Show("Нельзя расчитать дефекты на трубе, т.к. не задана глубина трубы № " +
                    //        //                cpm_baseOnDefect.NumberPipe);
                    //        if (gridDefectList != null)
                    //        {
                    //            //все дефекты на выбранных трубах(акт)
                    //            gridOnePipeDefectList.Clear();
                    //            foreach (GridDefect defect in gridDefectList)
                    //            {
                    //                if (pipe.KEYPIPE == defect.KeySegmentOnDefect)
                    //                {
                    //                    //удаляем расчеты на дефектов на трубе
                    //                    string t = CBLL.DeleteCalcDefect(defect.KeyDefect);

                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {


                    //        if (gridDefectList != null)
                    //        {
                    //            //все дефекты на выбранных трубах(акт)
                    //            gridOnePipeDefectList.Clear();
                    //            foreach (GridDefect defect in gridDefectList)
                    //            {
                    //                if (pipe.KEYPIPE == defect.KeySegmentOnDefect)
                    //                {
                    //                    gridOnePipeDefectList.Add(
                    //                        new GridDefect(defect.KeyDefect, defect.Angle, defect.W, defect.H, defect.ShiftX,
                    //                                       defect.KeySegmentOnDefect, defect.TypeDefect, defect.Depth,
                    //                                       defect.HintDefect, defect.NumberDefect, defect.Poterimetal, "",
                    //                                       "", "", "")
                    //                        );
                    //                }
                    //            }
                    //            if (gridOnePipeDefectList.Count != 0)
                    //            {
                    //                CalcAllDefect(cpm_baseOnDefect, gridOnePipeDefectList);
                    //            }

                    //        }
                    //    }
                    //}

                    //if (err != "")
                    //{
                    //    MessageBox.Show(err);
                    //}
           // }