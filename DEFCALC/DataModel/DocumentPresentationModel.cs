using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
    public class DocumentPresentationModel : BaseModel
    {
        //private Dict _defectockopist;
        //private Dict _dictListAgentLPU;
        private Dict _character_grunt;
        private Dict _type_izol;
        private Dict _izol_state;
        private Dict _availabl_overt;
        private Dict _adherenc_pipe;
        private Dict _availabl_dump;

        private string _numberAkt; //номер акта
        private string _dataShurf; //дата шурфования
        private string _dataVisible; //дата осмотра
        private string _dataExpl; //год ввода в эксплуатацию
        private string _charakter_grount; //характеристика рельефа местности
        private double? _depth; //глубина
        private double? _height; //высота
        private double? _lenght; //длина
        private double? _waterLevel; //уровень грунтовых вод
        private double? _rezistor_grunt; //удкльное сопротивление
        private double? _voltage_pipe; //потенциал трубы
        private string _number_skin; //количество слоев пленки
        private string _number_obertka; //количество слоев обертки
        private double? _inspect_squre; //осмотренная площадь
        private double? _inspect_koroz; //площадь с продуктами коррозии
        private string _vid_koroz; //вид коррозионногоповреждения
        private string _consulusion; //предварительное заключение
        private string _keyAkt;
        private string _defectoskopist;
        private string _agentLPU;


        MainViewModel Model { get { return App.Model; } }


        //public ObservableCollection<Dict> DictListDefectockopist { get; private set; } //словарь дефектоскопист
        //public ObservableCollection<Dict> DictListAgentLPU { get; private set; } //словарь представитель ЛПУ
        public ObservableCollection<Dict> DictListCharacter_grunt { get; private set; } //словарь характеристика грунта в шурфе
        public ObservableCollection<Dict> DictListType_izol { get; private set; } //словарь тип изоляции
        public ObservableCollection<Dict> DictListIzol_state { get; private set; } //словарь состояние защитного покрытия
        public ObservableCollection<Dict> DictListAvailabl_overt { get; private set; } //словарь наличие нахлеста
        public ObservableCollection<Dict> DictListAdherenc_pipe { get; private set; } // словарь прилепаемость к трубе
        public ObservableCollection<Dict> DictListAvailabl_dump { get; private set; } // словарь наличие влаги под изоляцией


        public DocumentPresentationModel()
        {
            //DictListDefectockopist = new ObservableCollection<Dict>();
            //DictListAgentLPU = new ObservableCollection<Dict>();
            DictListCharacter_grunt = new ObservableCollection<Dict>();
            DictListType_izol = new ObservableCollection<Dict>();
            DictListIzol_state = new ObservableCollection<Dict>();
            DictListAvailabl_overt = new ObservableCollection<Dict>();
            DictListAdherenc_pipe = new ObservableCollection<Dict>();
            DictListAvailabl_dump = new ObservableCollection<Dict>();
        }

        //public Dict SelectedDefectockopist
        //{
        //    get { return _defectockopist; }
        //    set
        //    {
        //        _defectockopist = value;
        //        NotifyPropertyChanged("SelectedDefectockopist");
        //    }
        //}

        //public Dict SelectedDictListAgentLPU
        //{
        //    get { return _dictListAgentLPU; }
        //    set
        //    {
        //        _dictListAgentLPU = value;
        //        NotifyPropertyChanged("SelectedDictListAgentLPU");
        //    }
        //}

        public Dict SelectedDictListCharacter_grunt
        {
            get { return _character_grunt; }
            set
            {
                _character_grunt = value;
                NotifyPropertyChanged("SelectedDictListCharacter_grunt");
            }
        }

        public Dict SelectedDictListType_izol
        {
            get { return _type_izol; }
            set
            {
                _type_izol = value;
                NotifyPropertyChanged("SelectedDictListType_izol");
            }
        }

        public Dict SelectedDictListIzol_state
        {
            get { return _izol_state; }
            set
            {
                _izol_state = value;
                NotifyPropertyChanged("SelectedDictListIzol_state");
            }
        }

        public Dict SelectedDictListAvailabl_overt
        {
            get { return _availabl_overt; }
            set
            {
                _availabl_overt = value;
                NotifyPropertyChanged("SelectedDictListAvailabl_overt");
            }
        }

        public Dict SelectedDictListAdherenc_pipe
        {
            get { return _adherenc_pipe; }
            set
            {
                _adherenc_pipe = value;
                NotifyPropertyChanged("SelectedDictListAdherenc_pipe");
            }
        }

        public Dict SelectedDictListAvailabl_dump
        {
            get { return _availabl_dump; }
            set
            {
                _availabl_dump = value;
                NotifyPropertyChanged("SelectedDictListAvailabl_dump");
            }
        }

        public string KeyAkt
        {
            get { return _keyAkt; }
            set
            {
                _keyAkt = value;
                NotifyPropertyChanged("KeyAkt");

            }
        }

        public string NumberAkt
        {
            get { return _numberAkt; }
            set
            {
                _numberAkt = value;
                NotifyPropertyChanged("NumberAkt");

            }
        }

        public string DataShurf
        {
            get { return _dataShurf; }
            set
            {
                _dataShurf = value;
                Model.DataShurf = value;
                //NotifyPropertyChanged("DataShurf");

            }
        }

        public string DataVisible
        {
            get { return _dataVisible; }
            set
            {
                _dataVisible = value;
                NotifyPropertyChanged("DataVisible");

            }
        }

        public string Defectoskopist
        {
            get { return _defectoskopist; }
            set
            {
                _defectoskopist = value;
                NotifyPropertyChanged("Defectoskopist");

            }
        }

        public string AgentLPU
        {
            get { return _agentLPU; }
            set
            {
                _agentLPU = value;
                NotifyPropertyChanged("AgentLPU");

            }
        }

        public string DataExpl
        {
            get { return _dataExpl; }
            set
            {
                _dataExpl = value;
                NotifyPropertyChanged("DataExpl");

            }
        }

        public string Charakter_grount
        {
            get { return _charakter_grount; }
            set
            {
                _charakter_grount = value;
                NotifyPropertyChanged("Charakter_grount");

            }
        }

        public double? Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                NotifyPropertyChanged("Depth");

            }
        }

        public double? Height
        {
            get { return _height; }
            set
            {
                _height = value;
                NotifyPropertyChanged("Height");

            }
        }

        public double? Lenght
        {
            get { return _lenght; }
            set
            {
                _lenght = value;
                NotifyPropertyChanged("Lenght");

            }
        }

        public double? WaterLevel
        {
            get { return _waterLevel; }
            set
            {
                _waterLevel = value;
                NotifyPropertyChanged("WaterLevel");

            }
        }

        public double? Rezistor_grunt
        {
            get { return _rezistor_grunt; }
            set
            {
                _rezistor_grunt = value;
                NotifyPropertyChanged("Rezistor_grunt");

            }
        }

        public double? Voltage_pipe
        {
            get { return _voltage_pipe; }
            set
            {
                _voltage_pipe = value;
                NotifyPropertyChanged("Voltage_pipe");

            }
        }

        public string Number_skin
        {
            get { return _number_skin; }
            set
            {
                _number_skin = value;
                NotifyPropertyChanged("Number_skin");

            }
        }

        public string Number_obertka
        {
            get { return _number_obertka; }
            set
            {
                _number_obertka = value;
                NotifyPropertyChanged("Number_obertka");

            }
        }
        public double? Inspect_squre
        {
            get { return _inspect_squre; }
            set
            {
                _inspect_squre = value;
                NotifyPropertyChanged("Inspect_squre");

            }
        }

        public double? Inspect_koroz
        {
            get { return _inspect_koroz; }
            set
            {
                _inspect_koroz = value;
                NotifyPropertyChanged("Inspect_koroz");

            }
        }

        public string Vid_koroz
        {
            get { return _vid_koroz; }
            set
            {
                _vid_koroz = value;
                NotifyPropertyChanged("Vid_koroz");

            }
        }


        public string Consulusion
        {
            get { return _consulusion; }
            set
            {
                _consulusion = value;
                NotifyPropertyChanged("Consulusion");

            }
        }

        // <summary>
        /// для инсерта, упдейта создаем поле- значение
        /// </summary>
        /// <returns></returns>
        
        public Dictionary<string, string> GetFieldsAndValues()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(NumberAkt))
            {
                d["cNumberAkt"] = "'" + NumberAkt + "'";
            }
            if (!string.IsNullOrWhiteSpace(DataShurf.ToString()))
            {
                d["dDateShurf"] = "'" + DataShurf + "'";
            }

            if (!string.IsNullOrWhiteSpace(DataExpl.ToString()))
            {
                d["nDataExpl"] = "'" + DataExpl + "'";
            }


            if (!string.IsNullOrWhiteSpace(DataVisible.ToString()))
            {
                
                d["dDate_visit"] = "'" + DataVisible + "'";
            }

            if (!string.IsNullOrWhiteSpace(Defectoskopist))
            {

                d["cdefectoskopist"] = "'" + Defectoskopist + "'";
            }

            if (!string.IsNullOrWhiteSpace(AgentLPU))
            {

                d["cagentLPU"] = "'" + AgentLPU + "'";
            }

            if (!string.IsNullOrWhiteSpace(Charakter_grount))
            {
                d["crelief_ground"] = "'" + Charakter_grount + "'";
            }

            if ((SelectedDictListCharacter_grunt.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedDictListCharacter_grunt.Key)))
            {
                d["nDict_Character_grunt_key"] = SelectedDictListCharacter_grunt.Key;
            }

            if ((Depth != null) && (Depth != 0))
            {
                d["ndepth_location"] = (Depth.ToString()).Replace(",", ".");
            }

            if ((Height != null) && (Height != 0))
            {
                d["ndepth"] = (Height.ToString()).Replace(",", ".");
            }

            if ((Lenght != null) && (Lenght != 0))
            {
                d["nlength"] = (Lenght.ToString()).Replace(",", ".");
            }
            if ((WaterLevel != null) )
            {
                d["cwater_level"] = (WaterLevel.ToString()).Replace(",", ".");
            }
            if ((Rezistor_grunt != null))
            {
                d["nrezistor_grunt"] = (Rezistor_grunt.ToString()).Replace(",", ".");
            }
            if ((Voltage_pipe != null))
            {
                d["nvoltage_pipe"] = (Voltage_pipe.ToString()).Replace(",", ".");
            }

            if ((SelectedDictListType_izol.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedDictListType_izol.Key)))
            {
                d["ntype_izol_key"] = SelectedDictListType_izol.Key;
            }

            if ((SelectedDictListIzol_state.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedDictListIzol_state.Key)))
            {
                d["nDict_Izol_state_key"] = SelectedDictListIzol_state.Key;
            }

            if (!string.IsNullOrWhiteSpace(Number_skin))
            {
                d["nNumber_skin"] = (Number_skin.ToString()).Replace(",", ".");
            }

            if (!string.IsNullOrWhiteSpace(Number_obertka))
            {
                d["nNumberObertka"] = (Number_obertka.ToString()).Replace(",", ".");
            }

            if ((SelectedDictListAvailabl_overt.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedDictListAvailabl_overt.Key)))
            {
                d["ndict_availabi_overl_key"] = SelectedDictListAvailabl_overt.Key;
            }

            if ((SelectedDictListAdherenc_pipe.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedDictListAdherenc_pipe.Key)))
            {
                d["ndict_adherenc_pipe_key"] = SelectedDictListAdherenc_pipe.Key;
            }

            if ((SelectedDictListAvailabl_dump.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedDictListAvailabl_dump.Key)))
            {
                d["ndict_availabil_damp_key"] = SelectedDictListAvailabl_dump.Key;
            }
            if ((Inspect_squre != null))
            {
                d["nInspect_square"] = (Inspect_squre.ToString()).Replace(",", ".");
            }
            if ((Inspect_koroz != null))
            {
                d["nInspect_square_koroz"] = (Inspect_koroz.ToString()).Replace(",", ".");
            }

            if (!string.IsNullOrWhiteSpace(Vid_koroz))
            {
                d["cVid_koroz_damage"] = "'" + Vid_koroz + "'";
            }

            if (!string.IsNullOrWhiteSpace(Consulusion))
            {
                d["cConculusion"] = "'" + Consulusion + "'";
            }
            
  
//--------------------------------------------------------
 
            return d;
        }
    }
}
