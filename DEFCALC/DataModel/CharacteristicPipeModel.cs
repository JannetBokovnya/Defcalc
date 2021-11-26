using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace DEFCALC.DataModel
{
    public class CharacteristicPipeModel : BaseModel
    {
        private Dict _ketegor_uch;
        private Dict _temperaturn_koef;
        private Dict _factory_builder;
        private Dict _feelGrade;
        private string _diam = ""; //внешний диаметр
        private string _depth_pipe = ""; //толщина стенки
        private string _maxPressure = ""; //максимальное рабочее давление
        private string _pressure = ""; //давление
        private string _range_fluid = ""; //предел текучести
        private string _modulUng = "";  //модуль Юнга
        private string _koefPuanson = ""; //коэффициент Пуансона
        private string _koefLinExpansion = ""; // коэффициент линейного расширения
        private string _range_stranght = "";//предел прочности
        private Dict _ecvivalentMarka_Builder;
        private string _numberPipe = ""; //номер трубы
        private string _kmPipe = "";//километраж трубы
        private string _angleShovPipe = ""; //сварной шов
        private string _dataExpl = ""; //год ввода трубы в эксплуатацию
        private string _lenghtPipe = ""; //длина трубы

        public ObservableCollection<Dict> DictListKetegor_uch { get; private set; }//категория участка
        public ObservableCollection<Dict> DictListTemperaturn_koef { get; private set; }//температурный коэффициент
        public ObservableCollection<Dict> DictListFactory_builder { get; private set; }//завод изготовитель
        public ObservableCollection<Dict> DictListFeelGrade { get; private set; } //словарь марка стали
        public ObservableCollection<Dict> DictListFactoryOnFeelGrade { get; private set; } //конкретный словарь соответствия завода-марки стали
        public ObservableCollection<Dict> DictGetTemperKoef_value { get; private set; }//(ДЛЯ РАСЧЕТОВ) словарь соответствия ключ температурного коеф и значения коеф(ДЛЯ РАСЧЕТОВ)
        public ObservableCollection<Dict> DictGetKategorUch_value { get; private set; } //(ДЛЯ РАСЧЕТОВ) словарь соответствия ключ категор уч - и значения категории 

        public List<EcvivalentMarka_Builder> EcvivalentMarka_BuilderList { get; private set; }// соответствие марка-стали - завод изготовитель
        
        public CharacteristicPipeModel()
        {
            DictListKetegor_uch = new ObservableCollection<Dict>();
            DictListTemperaturn_koef = new ObservableCollection<Dict>();
            DictListFactory_builder = new ObservableCollection<Dict>();
            DictListFeelGrade = new ObservableCollection<Dict>();
            DictGetTemperKoef_value = new ObservableCollection<Dict>();
            DictGetKategorUch_value = new ObservableCollection<Dict>();
            EcvivalentMarka_BuilderList = new List<EcvivalentMarka_Builder>();
            DictListFactoryOnFeelGrade = new ObservableCollection<Dict>();
           

        }

        //public  string GetItemOnEcvivalentMarka_Builder(string keybuilder, string keyMarka)
        //{
        //    string r = "";
           
        //    foreach (var emb in EcvivalentMarka_BuilderList)
        //    {
        //        if ((emb.KeFactory_builder == keybuilder) && (emb.KeyFeel_grade == keyMarka))
        //        {
        //            Range_fluid = emb.Range_fluid;
        //        }
        //    }
        //    return r;
           
        //}



        public Dict SelectedKetegor_uch
        {
            get { return _ketegor_uch; }
            set
            {
                _ketegor_uch = value;
                NotifyPropertyChanged("SelectedKetegor_uch");
            }
        }

        public Dict SelectedTemperaturn_koef
        {
            get { return _temperaturn_koef; }
            set
            {
                _temperaturn_koef = value;
                NotifyPropertyChanged("SelectedTemperaturn_koef");
            }
        }

        public Dict SelectedFactory_builder
        {
            get { return _factory_builder; }
            set
            {
                _factory_builder = value;
                SetFeelGrade();
               // SetRangeFluid();
                NotifyPropertyChanged("SelectedFactory_builder");
            }
        }

        public Dict SelectedFeelGrade
        {
            get { return _feelGrade; }
            set
            {
                _feelGrade = value;
                SetRangeFluid();
                NotifyPropertyChanged("SelectedFeelGrade");
            }
        }

        //по изменению события завод изготовитель меняется и марка стали(зависимости завод изготовитель- марка стали)
        private void SetFeelGrade()
        {
            if (SelectedFactory_builder == null)
            {
                Range_fluid = string.Empty;
                ModulUng = string.Empty;
                Range_stranght = string.Empty;
                KoefPuanson = string.Empty;
                KoefLinExpansion = string.Empty;
            }
            else
            {
                if (SelectedFactory_builder.Key == "-1")
                {
                    DictListFactoryOnFeelGrade.Clear();
                    DictListFactoryOnFeelGrade.Add(new Dict("-1", "Выбрать"));
                    SelectedFeelGrade = DictListFactoryOnFeelGrade[0];

                }
                else
                {
                    for (int ii = 0; ii < DictListFactory_builder.Count; ii++)
                    {
                        if (DictListFactory_builder[ii].Key == SelectedFactory_builder.Key)
                        {
                            //SelectedFactory_builder = DictListFactory_builder[ii];
                            //когда известен ключ завода ищем все марки стали этого завода и наполняем комбобокс для отображения
                            DictListFactoryOnFeelGrade.Clear();
                            DictListFactoryOnFeelGrade.Add(new Dict("-1", "Выбрать"));

                            foreach (var embL in EcvivalentMarka_BuilderList)
                            {
                                if (embL.KeFactory_builder == SelectedFactory_builder.Key)
                                {

                                    DictListFactoryOnFeelGrade.Add(
                                  new Dict
                                      (
                                        (embL.KeyFeel_grade),
                                        (embL.NameFeelGrade)
                                      )
                                  );

                                }
                            }
                            SelectedFeelGrade = DictListFactoryOnFeelGrade[0];
                            break;
                        }
                    }
                    // если нет соответствия марки стали-завода изготовителя

                    if (DictListFactoryOnFeelGrade.Count == 1)
                    {
                        DictListFactoryOnFeelGrade.Clear();

                        foreach (var embL in DictListFeelGrade)
                        {
                            DictListFactoryOnFeelGrade.Add(
                          new Dict
                              (
                                (embL.Key),
                                (embL.Name)
                              )
                          );
                        }
                    }
                }
      

                //for (int ii = 0; ii < DictListFactoryOnFeelGrade.Count; ii++)
                //{

                //    if (DictListFactoryOnFeelGrade[ii].Key == SelectedFactory_builder.Key)
                //    {
                //        SelectedFeelGrade = DictListFactoryOnFeelGrade[ii];
                //        break;
                //    }
                //}
            }
        }
        private void SetRangeFluid()
        {
            if (SelectedFactory_builder == null || SelectedFeelGrade == null)
            {
                Range_fluid = string.Empty;
                ModulUng = string.Empty;
                Range_stranght = string.Empty;
                KoefPuanson = string.Empty;
                KoefLinExpansion = string.Empty;
            }
            else
            {
                Range_fluid = string.Empty;
                foreach (var emb in EcvivalentMarka_BuilderList)
                {
                    if ((emb.KeFactory_builder == SelectedFactory_builder.Key) && (emb.KeyFeel_grade == SelectedFeelGrade.Key))
                    {
                        Range_fluid = emb.Range_fluid;
                        ModulUng = emb.ModuleUng;
                        Range_stranght = emb.Range_stranght;
                        KoefPuanson = emb.KoefPuanson;
                        KoefLinExpansion = emb.KoefLinExpansion;
                        break;
                    }
                }
            }
        }
       
        /// <summary>
        /// диаметр трубы
        /// </summary>
        public string Diam
        {
            get { return _diam; }
            set
            {
                _diam = value;
                NotifyPropertyChanged("Diam");

            }
        }
        /// <summary>
        /// номер трубы
        /// </summary>
        public string NumberPipe
        {
            get { return _numberPipe; }
            set
            {
                _numberPipe = value;
                NotifyPropertyChanged("NumberPipe");

            }
        }
        /// <summary>
        /// длина трубы
        /// </summary>
        public string LenghtPipe
        {
            get { return _lenghtPipe; }
            set
            {
                _lenghtPipe = value;
                NotifyPropertyChanged("LenghtPipe");

            }
        }

        /// <summary>
        /// километраж трубы
        /// </summary>
        public string KmPipe
        {
            get { return _kmPipe; }
            set
            {
                _kmPipe = value;
                NotifyPropertyChanged("KmPipe");

            }
        }
        /// <summary>
        /// угол шва трубы
        /// </summary>
        public string AngleShovPipe
        {
            get { return _angleShovPipe; }
            set
            {
                _angleShovPipe = value;
                NotifyPropertyChanged("AngleShovPipe");

            }
        }

/// <summary>
/// толщина стенки трубы
/// </summary>
        public string Depth_pipe
        {
            get { return _depth_pipe; }
            set
            {
                _depth_pipe = value;
                NotifyPropertyChanged("Depth_pipe");

            }
        }
        /// <summary>
        /// Максимальное рабочее давление в трубе
        /// </summary>
        public string MaxPressure
        {
            get { return _maxPressure; }
            set
            {
                string checkedValueMaxPressure = (string)value;
                string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                checkedValueMaxPressure = checkedValueMaxPressure.Replace(".", separator);
                checkedValueMaxPressure = checkedValueMaxPressure.Replace(",", separator);
                //checkedValueMaxPressure = checkedValueMaxPressure.Replace(",", ".");

                _maxPressure = checkedValueMaxPressure;
                NotifyPropertyChanged("MaxPressure");

            }
        }
        /// <summary>
        /// Давление в трубопроводе в трубопроводе
        /// </summary>
        public string Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
                NotifyPropertyChanged("Pressure");

            }
        }
        /// <summary>
        /// Предел текучести
        /// </summary>
        public string Range_fluid
        {
            get { return _range_fluid; }
            set
            {
                _range_fluid = value;
                NotifyPropertyChanged("Range_fluid");

            }
        }
        /// <summary>
        /// Предел прочности
        /// </summary>
        public string Range_stranght
        {
            get { return _range_stranght; }
            set
            {
                _range_stranght = value;
                NotifyPropertyChanged("Range_stranght");

            }
        }
        /// <summary>
        /// модуль Юнга
        /// </summary>
        public string ModulUng
        {
            get { return _modulUng; }
            set
            {
                _modulUng = value;
                NotifyPropertyChanged("ModulUng");

            }
        }
        /// <summary>
        /// коэффициент Пуансона
        /// </summary>
        public string KoefPuanson
        {
            get { return _koefPuanson; }
            set
            {
                _koefPuanson = value;
                NotifyPropertyChanged("KoefPuanson");

            }
        }
        /// <summary>
        /// Коэффициент линейного расширения
        /// </summary>
        public string KoefLinExpansion
        {
            get { return _koefLinExpansion; }
            set
            {
                _koefLinExpansion = value;
                NotifyPropertyChanged("KoefLinExpansion");

            }
        }
        /// <summary>
        /// год ввода в эксплуатацию
        /// </summary>
        public string DataExpl
        {
            get { return _dataExpl; }
            set
            {
                _dataExpl = value;
                NotifyPropertyChanged("DataExpl");

            }
        }


        public Dictionary<string, string> GetFieldsAndValuesPipe()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(Diam))
            {
                d["cdiam"] = (Diam.ToString()).Replace(",", "."); ;
            }

            if (!string.IsNullOrWhiteSpace(Depth_pipe))
            {
                d["cdepth_pipe"] = (Depth_pipe.ToString()).Replace(",", "."); ;
            }

            if (!string.IsNullOrWhiteSpace(MaxPressure))
            {
                
                d["nmaxpressure"] = (MaxPressure.ToString()).Replace(",", ".");
            }
            else
            {
                d["nmaxpressure"] = "null";
            }
            if (!string.IsNullOrWhiteSpace(Pressure))
            {
                d["npressure"] = (Pressure.ToString()).Replace(",", "."); 
            }
            else
            {
                d["npressure"] = "null";
            }

            if ((SelectedKetegor_uch.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedKetegor_uch.Key)))
            {
                d["ndict_kategor_uch_key"] = SelectedKetegor_uch.Key;
            }
            else
            {
                d["ndict_kategor_uch_key"] = "null";
            }

            if ((SelectedTemperaturn_koef.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedTemperaturn_koef.Key)))
            {
                d["nDict_temperaturn_koef_key"] = SelectedTemperaturn_koef.Key;
            }
            else
            {
                d["nDict_temperaturn_koef_key"] = "null";
            }

            if (SelectedFactory_builder != null)
            {
                if ((SelectedFactory_builder.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedFactory_builder.Key)))
                {
                    d["nFactory_builder_key"] = SelectedFactory_builder.Key;
                }
                else
                {
                    d["nFactory_builder_key"] = "null";
                }
            }
            else
            {
                d["nFactory_builder_key"] = "null";
            }
           
            if ((SelectedFeelGrade != null))
            {
                if ((SelectedFeelGrade.Key != "-1") && (SelectedFeelGrade != null))
                {
                    d["nDict_feel_grade_key"] = SelectedFeelGrade.Key;
                }
                else
                {
                    d["nDict_feel_grade_key"] = "null";
                }

            }
            else
            {
                d["nDict_feel_grade_key"] = "null";
            }
           
            //if (!string.IsNullOrWhiteSpace(DataExpl))
            {
                d["nDataExpl"] = "'" + DataExpl + "'";
            }

            if (!string.IsNullOrWhiteSpace(AngleShovPipe))
            {
                d["nAngle"] = (AngleShovPipe.ToString()).Replace(",", "."); ;
            }

            //if (!string.IsNullOrWhiteSpace(KmPipe))
            {
                d["cloc_km_beg"] = "'" + KmPipe + "'";
            }

           // if (!string.IsNullOrWhiteSpace(NumberPipe))
            {
                d["cNumber_pipe"] = "'" + NumberPipe + "'";
            }


            return d;
        }

        public string GetTemp_Koef_onKey(string key)
        {
            string koef = "0.9";
  
            if ((!String.IsNullOrEmpty(key)) && (key != "-1"))
            {
                for (int ii = 0; ii < DictGetTemperKoef_value.Count; ii++)
                {
                    if (DictGetTemperKoef_value[ii].Key == key)
                    {
                        koef = DictGetTemperKoef_value[ii].Name;
                        break;
                    }
                }
            }

            return koef;
        }
        public string Getketegor_Uch_OnKey(string key)
        {
            string kategorPipe = "6.0";
            if ((!String.IsNullOrEmpty(key)) && (key != "-1"))
            {
                for (int ii = 0; ii < DictGetKategorUch_value.Count; ii++)
                {
                    if (DictGetKategorUch_value[ii].Key == key)
                    {
                        kategorPipe = DictGetKategorUch_value[ii].Key;
                        break;
                    }
                }
            }
            return kategorPipe;
        }
    }
        
 
}
