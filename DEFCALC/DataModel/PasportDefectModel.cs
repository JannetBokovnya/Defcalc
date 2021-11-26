
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DEFCALC.DataModel
{
    public class PasportDefectModel : BaseModel
    {
       
        private string _keyDefect; //ключ дефекта
        private string _defectName; //название дефекта
        private Dict _typeDefect; //тип дефекта
        private Dict _calcType; // расчетный тип
        private string _w; //длина дефекта
        private string _h;//высота(ширина) дефекта
        private string _depth; //глубина дефекта
        private double? _lossMetal; //потери металла
        private string _prevSeam_Dist; //расстояние до предыдущего шва
        private double? _next_Dist; //расстояние до следующего сварного шва
        private double? _prevMark_Dist; //расстояние до предыдущего репера
        private double? _nextReper_Dist; // расстояние до следующего репера
        private string _angle; //  угловое положение дефекта час
        private Dict _depthPos;//расположение в глубине металла
        private string _stress_Koor; //описанре стресс коррозии
        private Dict _metodUstr; // метод устранения
        private Dict _defectUstr;
        private string _dataUstrDefect;
        private Dict _metodUstr2;
        private string _keyPipe; //ключ трубы
        private bool _saveDefect;


       
        public ObservableCollection<Dict> DictListTypeDefect { get; private set; }
        public ObservableCollection<Dict> DictListCalcType { get; private set; }
        public ObservableCollection<Dict> DictListDepthPos { get; private set; }
        public ObservableCollection<Dict> DictListMetodUstr { get; private set; }
        public ObservableCollection<Dict> DictListDefectUstr { get; private set; }
        public ObservableCollection<Dict> DictListMetodUstr2 { get; private set; }
        


        public PasportDefectModel()
        {
            
            DictListTypeDefect = new ObservableCollection<Dict>();
            DictListCalcType = new ObservableCollection<Dict>();
            DictListDepthPos = new ObservableCollection<Dict>();
            DictListMetodUstr = new ObservableCollection<Dict>();
            DictListDefectUstr = new ObservableCollection<Dict>();
            DictListMetodUstr2 = new ObservableCollection<Dict>();
        }


        public Dict SelectedTypeDefect
        {
            get { return _typeDefect; }
            set
            {
                _typeDefect = value;
                NotifyPropertyChanged("SelectedTypeDefect");
            }
        }

        public Dict SelectedCalcType
        {
            get { return _calcType; }
            set
            {
                _calcType = value;
                NotifyPropertyChanged("SelectedCalcType");
            }
        }

        public Dict SelectedDepthPos
        {
            get { return _depthPos; }
            set
            {
                _depthPos = value;
                NotifyPropertyChanged("SelectedDepthPos");
            }
        }

        public Dict SelectedMetodUstr
        {
            get { return _metodUstr; }
            set
            {
                _metodUstr = value;
                NotifyPropertyChanged("SelectedMetodUstr");
            }
        }
        public Dict SelectedMetodUstr2
        {
            get { return _metodUstr2; }
            set
            {
                _metodUstr2 = value;
                NotifyPropertyChanged("SelectedMetodUstr2");
            }
        }

        public Dict SelectedDefectUstr
        {
            get { return _defectUstr; }
            set
            {
                _defectUstr = value;
                NotifyPropertyChanged("SelectedDefectUstr");
            }
        }



        public string KeyDefect
        {
            get { return _keyDefect; }
            set
            {
                _keyDefect = value;
                NotifyPropertyChanged("KeyDefect");

            }
        }
        public string KeyPipe
        {
            get { return _keyPipe; }
            set
            {
                _keyPipe = value;
                NotifyPropertyChanged("KeyPipe");

            }
        }

        public bool SaveDefect
        {
            get { return _saveDefect; }
            set
            {
                _saveDefect = value;
            }
        }

        public string DefectName
        {
            get { return _defectName; }
            set
            {
                _defectName = value;
                NotifyPropertyChanged("DefectName");

            }
        }


        public string W
        {
            get { return _w; }
            set
            {
                _w = value;
                NotifyPropertyChanged("W");

            }
        }
       
        public string H
        {
            get { return _h; }
            set
            {
                _h = value;
                NotifyPropertyChanged("H");

            }
        }

        public string Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                NotifyPropertyChanged("Depth");

            }
        }

        public double? LossMetal
        {
            get { return _lossMetal; }
            set
            {
                _lossMetal = value;
                NotifyPropertyChanged("LossMetal");

            }
        }

        public string PrevSeam_Dist
        {
            get { return _prevSeam_Dist; }
            set
            {
               
                
                //double valDouble;
                //string valued;
                //valued = value.ToString();
                //string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                //valued = valued.Replace(".", separator);
                //valued = valued.Replace(",", separator);

                //if (double.TryParse(valued, out valDouble))
                //{
                //    _prevSeam_Dist = value;
                //}
                //else
                //{
                //    throw new Exception("Допустимы только  числа");
                //}
                _prevSeam_Dist = value;
                NotifyPropertyChanged("PrevSeam_Dist");

            }
        }

        public double? Next_Dist
        {
            get { return _next_Dist; }
            set
            {
                _next_Dist = value;
                NotifyPropertyChanged("Next_Dist");

            }
        }

        public double? PrevMark_Dist
        {
            get { return _prevMark_Dist; }
            set
            {
                _prevMark_Dist = value;
                NotifyPropertyChanged("PrevMark_Dist");

            }
        }

        public double? NextReper_Dist
        {
            get { return _nextReper_Dist; }
            set
            {
                _nextReper_Dist = value;
                NotifyPropertyChanged("NextReper_Dist");

            }
        }

        public string Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                NotifyPropertyChanged("Angle");

            }
        }

        public string Stress_Koor
        {
            get { return _stress_Koor; }
            set
            {
                _stress_Koor = value;
                NotifyPropertyChanged("Stress_Koor");

            }
        }

        public string DataUstrDefect
        {
            get { return _dataUstrDefect; }
            set
            {
                _dataUstrDefect = value;
                NotifyPropertyChanged("DataUstrDefect");

            }
        }

/// <summary>
/// для инсерта создаем поле- значение
/// </summary>
/// <returns></returns>
        public Dictionary<string, string> GetFieldsAndValues()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(DefectName))
            {
                d["cName"] = "'" + DefectName + "'";
            }

            if ((SelectedTypeDefect.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedTypeDefect.Key)))
            {
                d["ntypedefect_key"] = SelectedTypeDefect.Key;
            }

            if ((SelectedCalcType.Key != "-1") && (!string.IsNullOrWhiteSpace(SelectedCalcType.Key)))
            {
                d["nDict_Calc_Type_Key"] = SelectedCalcType.Key;
            }

            //if ((W != null) && (W != 0))
            //{
            //    d["nlengthDefect"] = (W.ToString()).Replace(",", ".");
            //}

            if (!string.IsNullOrWhiteSpace(W))
            {
                d["nlengthDefect"] = (W.ToString()).Replace(",", ".");
            }

            //if ((H != null) && (H != 0))
            //{
            //    d["nwidth"] = (H.ToString()).Replace(",", ".");
            //}

            if (!string.IsNullOrWhiteSpace(H))
            {
                d["nwidth"] = (H.ToString()).Replace(",", ".");
            }

            //if ((Depth != null) && (Depth != 0))
            //{
            //    d["ndepth"] = (Depth.ToString()).Replace(",", ".");
            //}

            if (!string.IsNullOrWhiteSpace(Depth))
            {
                d["ndepth"] = (Depth.ToString()).Replace(",", ".");
            }

            if ((LossMetal != null) && (LossMetal != 0))
            {
                d["nloss_metall"] = (LossMetal.ToString()).Replace(",", ".");
            }

            //if ((PrevSeam_Dist != null) && (PrevSeam_Dist != 0))
            //{
            //    d["nprevseam_dist"] = (PrevSeam_Dist.ToString()).Replace(",", ".");
            //}

            if (!string.IsNullOrWhiteSpace(PrevSeam_Dist))
            {
                d["nprevseam_dist"] = (PrevSeam_Dist.ToString()).Replace(",", ".");
            }

            if ((Next_Dist != null) && (Next_Dist != 0))
            {
                d["nnext_dist"] = (Next_Dist.ToString()).Replace(",", ".");
            }

            if ((PrevMark_Dist != null) && (PrevMark_Dist != 0))
            {
                d["nprevmark_dist"] = (PrevMark_Dist.ToString()).Replace(",", ".");
            }

            if ((NextReper_Dist != null) && (NextReper_Dist != 0))
            {
                d["nnextmark_dist"] = (NextReper_Dist.ToString()).Replace(",", ".");
            }

            //if ((Angle != null)&& (Angle != 0))
            //{
            //    d["nclockwise_pos"] = (Angle.ToString()).Replace(",", ".");
            //}

            if (!string.IsNullOrWhiteSpace(Angle))
            {
                d["nclockwise_pos"] = (Angle.ToString()).Replace(",", ".");
            }

            if ((SelectedDepthPos.Key != "-1") && !string.IsNullOrWhiteSpace(SelectedDepthPos.Key))
            {
                d["ndepth_pos_Key"] = SelectedDepthPos.Key;
            }

            if (!string.IsNullOrWhiteSpace(Stress_Koor))
            {
                d["cstress_descr"] = "'" + Stress_Koor + "'";
            }

            if ((SelectedMetodUstr.Key != "-1") && !string.IsNullOrWhiteSpace(SelectedMetodUstr.Key))
            {
                d["nMetod_Ustr_Def_Key"] = SelectedMetodUstr.Key;
            }

            if ((SelectedDefectUstr.Key != "-1") && !string.IsNullOrWhiteSpace(SelectedDefectUstr.Key))
            {
                d["nDict_defect_ustr_key"] = SelectedDefectUstr.Key;
            }
            else
            {
                d["nDict_defect_ustr_key"] = "null";
            }

             if ((SelectedMetodUstr2.Key != "-1") && !string.IsNullOrWhiteSpace(SelectedMetodUstr2.Key))
             {
                 d["nMetod_Ustr_Def_Otrem_Key"] = SelectedMetodUstr2.Key;
             }
             else
             {
                 d["nMetod_Ustr_Def_Otrem_Key"] = "null";
             }

             if (!string.IsNullOrWhiteSpace(DataUstrDefect.ToString()))
             {
                 d["cDataUstrDefect"] = "'" + DataUstrDefect + "'";
             }
             else
             {
                 d["cDataUstrDefect"] = "null";
             }

            return d;

        }

    }
}



//public Dict SelectedDictAgentLPU
//{
//    get { return _selectedDictAgentLPU; }
//    set
//    {
//        _selectedDictAgentLPU = value;
//        NotifyPropertyChanged("SelectedDictAgentLPU");
//    }
//}


