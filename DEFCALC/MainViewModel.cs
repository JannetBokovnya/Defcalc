using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using DEFCALC.DataModel;
using DrawPipe.DataModel;
using DrawPipe.ViewModel;

namespace DEFCALC
{
    public partial class MainViewModel : BaseModel
    {
        public enum DefectActions
        {
            Edit,
            Delete,
            Create,
            ConfirnNoEdit
        };

        public enum NameFormWindows
        {
            MainWindow,
            SelectionRegion,
            SelectPipe,
            CreateActShurfovaniya
        } ;


        private MG _selectedmg;
        private Nit _selectedNit;
        private GridRegion _selectedgridRegion;
        private GridPipe _selectGridPipe;
        private string _getkmpipe;
        private Dict _selectDictDefectoskopist;
        private Dict _selectDictAgentLPU;
        private PipeViewModel _pipeModel;
        private string _getKeyActShurf;
        private string _getSelectKeyDefect;
        private DefectActions _defectAction;
        private Brush _colorASME;
        private Brush _colorDNV;
        private Brush _colorRstreng;
        private string _dopDefect;
        private string _maxPressureDopASME;
        private string _destroyPressureASME;
        private string _maxDopLenghtDefASME;
        private string _maxDopDepthASME;
        private string _recomResponsASME;
        private string _destroyPressureDNV;
        private string _dezPressureDNV;
        private string _koefDezPressureDNV;
        private string _destroyPressureRestreng;
        private string _coefZapRestreng;
        private string _recomResponsRestreng;
        private string _recomResponsDNV;
        private GridDefect _gridDefect;
        private ObservableCollection<GridDefect> _gridDefectlist;
        private string _numberAct;
        private string _dataAct;
        private string _selectMgKey;
        private string _selectMgName;
        private string _selectedNitKey;
        private string _selectedNitName;
        private string _selectedKmBegin;
        private string _selectedKmEnd;
        private string _selectRegionName;
        private string _selectKeyRegion;
        private Window _nameFormSend;
        private string _selectPipeKm;
        private string _selectPipeNumberPipe;
        private NameFormWindows _nameFormWindow;
        private string _keyAkt;
        private string _numberAkt;
        private string _dataAkt;
        private string _kmAkt;
        private string _numberPipe;
        private GridAct _selectedgridAct;
        private string _KeyPipe;
        private string _dataShurf;
        private string _UpdateAngelPipe;
        private DocumentPresentationModel _dpm;
        private CharacteristicPipeModel _cpm;
        private ExportDefectAkt.Export _exportDefectAkt;
        private bool _actwihtRegion;
        private string _countRegions;
        private string _selectRegion;
        private string _selectKilometr;
        private string _lblKmRegion;
        private string _lblNumberPipeRegion;
        private string _numberOneDefect;
        private ObservableCollection<SelectPipeOnGrid> _gridOneDefectCalc1;
        private bool _calcGroopDefect;
        private bool _colorDefectPipe;
        private bool _print;
        private string _plaseState;
        private string _kmPipe;
        private int _visibleReport;
        private PipeViewModel _pipeModelPrint;
        private string _signatureakt;
        private string _nullPipe;
        private string _createNewAkt;
        private ObservableCollection<GridPipe> _gridPipeList1;
        private ObservableCollection<GridDefect> _gridDefectlist1;
        private string _opencalcDefect;
        private bool _isShowBusy;
        private bool _isCallcDefect;
        private bool _isClickMenu;


        public ObservableCollection<MG> MGList { get; private set; }
        public ObservableCollection<Nit> NitList { get; private set; }
        public ObservableCollection<GridRegion> GridRegionList { get; private set; }
        public ObservableCollection<GridPipe> GridPipeList { get; private set; }
        public ObservableCollection<GridAct> GridActList { get; private set; }
        public ObservableCollection<GridPipe> GridPipeListOnSelectPipeList { get; private set; }
        public ObservableCollection<GridDefect> GridDefectList { get; private set; }
        public ObservableCollection<GridDefect> GridDefectList1 { get; private set; }
        public ObservableCollection<GridDefect> GridDefectListOnkeypipe { get; private set; }
        public ObservableCollection<GridOneDefectCalc> GridOneDefectCalc { get; private set; }
        public ObservableCollection<SelectPipeOnGrid> SelectPipeList { get; private set; }
        public ObservableCollection<GridPipe> GetPipeOnkeyPipe { get; private set; }
        public ObservableCollection<Dict> DictListDefectoskopist { get; private set; }
        public ObservableCollection<Dict> DictListAgentLPU { get; private set; }
        public ObservableCollection<Dict> DictListTypeDefect { get; private set; }
        public ObservableCollection<Shurf> Shurf { get; private set; }
        public ObservableCollection<CalculateDefectDNV> CalculateDefectListDNV { get; private set; }
        public ObservableCollection<CalculateDefectASME> CalculateDefectListASME { get; private set; }
        public ObservableCollection<CalculateDefectRSTRENG> CalculateDefectListRSTRENG { get; private set; }
        public ObservableCollection<string> Reports { get; private set; }
        public List<string> SelectedPipeActKeysList { get; private set; }



        public MainViewModel()
        {
            MGList = new ObservableCollection<MG>();
            NitList = new ObservableCollection<Nit>();
            GridRegionList = new ObservableCollection<GridRegion>();
            GridPipeList = new ObservableCollection<GridPipe>();
            GridActList = new ObservableCollection<GridAct>();
            GridPipeListOnSelectPipeList = new ObservableCollection<GridPipe>();
            GridDefectList = new ObservableCollection<GridDefect>();
            GridDefectList1 = new ObservableCollection<GridDefect>();
            GridDefectListOnkeypipe = new ObservableCollection<GridDefect>();
            GridOneDefectCalc = new ObservableCollection<GridOneDefectCalc>();
            SelectPipeList = new ObservableCollection<SelectPipeOnGrid>();
            GetPipeOnkeyPipe = new ObservableCollection<GridPipe>();
            DictListDefectoskopist = new ObservableCollection<Dict>();
            DictListAgentLPU = new ObservableCollection<Dict>();
            DictListTypeDefect = new ObservableCollection<Dict>();
            Shurf = new ObservableCollection<Shurf>();
            CalculateDefectListDNV = new ObservableCollection<CalculateDefectDNV>();
            CalculateDefectListASME = new ObservableCollection<CalculateDefectASME>();
            CalculateDefectListRSTRENG = new ObservableCollection<CalculateDefectRSTRENG>();
            PipeModel = new PipeViewModel(); //модель для встроенного контрола рисования трубы
            PipeModelPrint = new PipeViewModel();
            SelectedPipeActKeysList = new List<string>();
            Reports = new ObservableCollection<string>();

        }

        public void Report(string message)
        {
            Reports.Add(string.Format("{0}, {1}", DateTime.Now.ToLongTimeString(), message));
        }

        public void GetVisibleReport()
        {
            int result = CBLL.GetVisibleReport();
            VisibleReport = result;

        }

        //событие при нажатие меню(удалить, добавить, редактировать)
        public bool IsClickMenu
        {
            get { return _isClickMenu; }
            set
            {
                _isClickMenu = value;


            }
        }

        public bool IsShowBusy
        {
            get { return _isShowBusy; }
            set
            {
                _isShowBusy = value;
                NotifyPropertyChanged("IsShowBusy");

            }
        }

        public bool IsCallcDefect
        {
            get { return _isCallcDefect; }
            set
            {
                _isCallcDefect = value;

            }
        }

        public int VisibleReport
        {
            get { return _visibleReport; }
            set
            {
                _visibleReport = value;
                NotifyPropertyChanged("VisibleReport");

            }
        }
        /// <summary>
        /// подпись акта
        /// 1 - подписан
        /// 2 - загружен из базы
        /// 0 - редактируется
        /// </summary>
        public string SignatureAkt
        {
            get { return _signatureakt; }
            set
            {
                _signatureakt = value;
                NotifyPropertyChanged("SignatureAkt");

            }
        }

        /// <summary>
        /// преобразование объкта в дабл
        /// </summary>
        /// <param name="valueRow"></param>
        /// <returns></returns>
        public static double GetValueRow(object valueRow)
        {
            if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
            {
                double result;
                if (double.TryParse(valueRow.ToString(), out result))
                    return result;

            }

            return 0d;
        }

        /// <summary>
        /// расчет потери металла в %
        /// </summary>
        /// <param name="valueDephDef"></param>
        /// <param name="valueDephPipe"></param>
        /// <returns></returns>
        public static string GetLossMetal(object valueDephDef, object valueDephPipe)
        {
            try
            {
                double dephDef = double.Parse(valueDephDef.ToString());
                double dephPipe = double.Parse(valueDephPipe.ToString());

                return (Math.Round((dephDef / dephPipe), 3)).ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }
        /// <summary>
        /// диаметр
        /// </summary>
        /// <param name="valueRow"></param>
        /// <returns></returns>
        public static double GetValueRowDiam(object valueRow)
        {
            if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
            {
                double result;
                if (double.TryParse(valueRow.ToString(), out result))
                    result = (result / 2) / 1000;
                return result;
            }
            return 0.71d;
        }

        public static double? GetValueRowNull(object valueRow)
        {
            if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
            {
                double result;
                if (double.TryParse(valueRow.ToString(), out result))
                    return result;
            }
            return null;
        }
        /// <summary>
        /// цвет дефектов
        /// </summary>
        /// <param name="valueRow"></param>
        /// <returns></returns>
        public static Brush GetPictureValueRow(object valueRow)
        {

            if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
            {
                string color = "";
                switch (valueRow.ToString())
                {
                    case "допустимый":
                        {
                            return Brushes.Green;
                            break;
                        }
                    case "нет рекоммендаций":
                        {
                            return Brushes.Green;
                            break;
                        }

                    case "условно допустимый":
                        {
                            return Brushes.Orange;
                            break;
                        }
                    case "не допустимый":
                        {
                            return Brushes.Red;
                            break;
                        }
                    default:
                        {
                            return Brushes.White; break;
                        }

                }

            }
            return Brushes.White;
        }

        public static string GetPictureValueRow1(object valueRow)
        {

            if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
            {
                string color = "";
                switch (valueRow.ToString())
                {
                    case "допустимый":
                        {
                            return "Green";
                            break;
                        }
                    case "нет рекоммендаций":
                        {
                            return "Green";
                            break;
                        }

                    case "условно допустимый":
                        {
                            return "Orange";
                            break;
                        }
                    case "не допустимый":
                        {
                            return "Red";
                            break;
                        }
                    default:
                        {
                            return "White"; break;
                        }

                }

            }
            return "White";
        }

        public static string GetStatusAkt(object valueRow)
        {
            string status = "Редактируется";
            if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
            {
                switch (valueRow.ToString())
                {
                    case "1":
                        {
                            status = "Подписан";

                        }
                        break;
                    case "2":
                        {
                            status = "Загружен из базы";
                        }
                        break;
                    default:
                        {
                            status = "Редактируется";
                        }
                        break;
                }

            }
            return status;
        }

        public static DateTime GetdataTime(object valueRow)
        {
            DateTime result = new DateTime();

             if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
             {
                 result = DateTime.Parse(valueRow.ToString());      
             }
            
           
            return result;
        }

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
        /// контекст меню для таблицы дефектов
        /// </summary>
        /// <param name="valueRow"></param>
        /// <returns></returns>
        public static string GetTypePictureValueRow(object valueRow)
        {
            if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
            {
                switch (valueRow.ToString())
                {
                    case "2":
                        {
                            return "/DEFCALC;component/Images/edit-table.png";

                            break;
                        }
                }
                switch (valueRow.ToString())
                {
                    case "3":
                        {
                            return "/DEFCALC;component/Images/repair-table.png";

                            break;
                        }
                }
                switch (valueRow.ToString())
                {
                    case "5":
                        {
                            return "/DEFCALC;component/Images/add-table.png";

                            break;
                        }
                }
            }



            return "";
        }


        //модель для встроенного контрола рисования трубы
        public PipeViewModel PipeModel
        {
            get { return _pipeModel; }
            set
            {
                _pipeModel = value;
                NotifyPropertyChanged("PipeModel");

            }
        }

        public PipeViewModel PipeModelPrint
        {
            get { return _pipeModelPrint; }
            set
            {
                _pipeModelPrint = value;
                NotifyPropertyChanged("PipeModelPrint");

            }
        }

        //название формы из которой вызывалась другая форма - 
        //нужно что бы при выборе отмена перейти на вызывающую форму ?????????
        public Window NameFormSend
        {
            get { return _nameFormSend; }
            set
            {
                _nameFormSend = value;
                NotifyPropertyChanged("NameFormSend");

            }
        }



        public DocumentPresentationModel DPM
        {
            get { return _dpm; }
            set { _dpm = value; }
        }

        public CharacteristicPipeModel CPM
        {
            get { return _cpm; }
            set { _cpm = value; }
        }
        #region цвет дефектов при расчетах и расчет дефектов

        //определяет цвет при расчетах дефектов ASME
        public Brush ColorASME
        {
            get { return _colorASME; }
            set
            {
                _colorASME = value;
                NotifyPropertyChanged("ColorASME");

            }
        }
        //определяет цвет при расчетах дефектов DNV
        public Brush ColorDNV
        {
            get { return _colorDNV; }
            set
            {
                _colorDNV = value;
                NotifyPropertyChanged("ColorDNV");

            }
        }
        //определяет цвет при расчетах дефектов Rstreng
        public Brush ColorRstreng
        {
            get { return _colorRstreng; }
            set
            {
                _colorRstreng = value;
                NotifyPropertyChanged("ColorRstreng");

            }
        }
        //допустимость дефекта
        public string DopDefect
        {
            get { return _dopDefect; }
            set
            {
                _dopDefect = value;
                NotifyPropertyChanged("DopDefect");

            }
        }
        //макс допустимое давление ASME
        public string MaxPressureDopASME
        {
            get { return _maxPressureDopASME; }
            set
            {
                _maxPressureDopASME = value;
                NotifyPropertyChanged("MaxPressureDopASME");

            }
        }
        //разрушающее давление ASME
        public string DestroyPressureASME
        {
            get { return _destroyPressureASME; }
            set
            {
                _destroyPressureASME = value;
                NotifyPropertyChanged("DestroyPressureASME");

            }
        }
        //макс допустимая длина дефекта ASME
        public string MaxDopLenghtDefASME
        {
            get { return _maxDopLenghtDefASME; }
            set
            {
                _maxDopLenghtDefASME = value;
                NotifyPropertyChanged("MaxDopLenghtDefASME");

            }
        }
        //макс допустимая глубина деф
        public string MaxDopDepthASME
        {
            get { return _maxDopDepthASME; }
            set
            {
                _maxDopDepthASME = value;
                NotifyPropertyChanged("MaxDopDepthASME");

            }
        }
        //рекомендация по реагированию ASME   
        public string RecomResponsASME
        {
            get { return _recomResponsASME; }
            set
            {
                _recomResponsASME = value;
                NotifyPropertyChanged("RecomResponsASME");

            }
        }
        //разрушающее давление по DNV 
        public string DestroyPressureDNV
        {
            get { return _destroyPressureDNV; }
            set
            {
                _destroyPressureDNV = value;
                NotifyPropertyChanged("DestroyPressureDNV");

            }
        }
        //безопасное давление DNV
        public string DezPressureDNV
        {
            get { return _dezPressureDNV; }
            set
            {
                _dezPressureDNV = value;
                NotifyPropertyChanged("DezPressureDNV");

            }
        }
        //коэффициент безопасного давления 
        public string KoefDezPressureDNV
        {
            get { return _koefDezPressureDNV; }
            set
            {
                _koefDezPressureDNV = value;
                NotifyPropertyChanged("KoefDezPressureDNV");

            }
        }
        //разрушающее давление Restreng 
        public string DestroyPressureRestreng
        {
            get { return _destroyPressureRestreng; }
            set
            {
                _destroyPressureRestreng = value;
                NotifyPropertyChanged("DestroyPressureRestreng");

            }
        }
        //коэффициент запаса Restreng  
        public string CoefZapRestreng
        {
            get { return _coefZapRestreng; }
            set
            {
                _coefZapRestreng = value;
                NotifyPropertyChanged("CoefZapRestreng");

            }
        }
        //рекомендации по реагированию Restreng
        public string RecomResponsRestreng
        {
            get { return _recomResponsRestreng; }
            set
            {
                _recomResponsRestreng = value;
                NotifyPropertyChanged("RecomResponsRestreng");

            }
        }
        //рекомендация по реагированию DNV
        public string RecomResponsDNV
        {
            get { return _recomResponsDNV; }
            set
            {
                _recomResponsDNV = value;
                NotifyPropertyChanged("RecomResponsDNV");

            }
        }

        public ObservableCollection<SelectPipeOnGrid> GridOneDefectCalc1
        {
            get { return _gridOneDefectCalc1; }
            set
            {
                _gridOneDefectCalc1 = value;
                NotifyPropertyChanged("GridOneDefectCalc1");

            }
        }
        //нажатие на кнопку групповой дефект
        public bool CalcGroopDefect
        {
            get { return _calcGroopDefect; }
            set
            {

                _calcGroopDefect = value;
                NotifyPropertyChanged("CalcGroopDefect");

            }
        }
        //нажатие на кнопку печать
        public bool Print
        {
            get { return _print; }
            set
            {
                _print = value;
                NotifyPropertyChanged("Print");

            }
        }



        //цвет дефекта на трубе
        public bool ColorDefectPipe
        {
            get { return _colorDefectPipe; }
            set
            {
                _colorDefectPipe = value;
                NotifyPropertyChanged("ColorDefectPipe");

            }
        }
        #endregion


        #region Строка информации, которая видна на всех формах

        /// <summary>
        /// местоположение (мг+нитка+регион)
        /// </summary>
        public string PlaseState
        {
            get { return _plaseState; }
            set
            {
                _plaseState = value;
            }
        }
        /// <summary>
        /// номера труб
        /// </summary>
        public string NumberPipe
        {
            get { return _numberPipe; }
            set
            {
                _numberPipe = value;
            }
        }
        /// <summary>
        /// километраж
        /// </summary>
        public string KmPipe
        {
            get { return _kmPipe; }
            set
            {
                _kmPipe = value;
            }
        }
        #endregion


        #region форма MainWindow window  - основная

        #region основные данные при сохранении формы в xml

        public string SelectMgKey
        {
            get { return _selectMgKey; }
            set
            {
                _selectMgKey = value;
                NotifyPropertyChanged("SelectMgKey");

            }
        }

        public string SelectMgName
        {
            get { return _selectMgName; }
            set
            {
                _selectMgName = value;
                NotifyPropertyChanged("SelectMgName");

            }
        }

        public string SelectNitKey
        {
            get { return _selectedNitKey; }
            set
            {
                _selectedNitKey = value;
                NotifyPropertyChanged("SelectNitKey");

            }
        }

        public string SelectNitName
        {
            get { return _selectedNitName; }
            set
            {
                _selectedNitName = value;
                NotifyPropertyChanged("SelectNitName");

            }
        }

        public string SelectedKmBegin
        {
            get { return _selectedKmBegin; }
            set
            {
                _selectedKmBegin = value;
                NotifyPropertyChanged("SelectedKmBegin");

            }
        }



        public string SelectedKmEnd
        {
            get { return _selectedKmEnd; }
            set
            {
                _selectedKmEnd = value;
                NotifyPropertyChanged("SelectedKmEnd");

            }
        }

        public string SelectRegionName
        {
            get { return _selectRegionName; }
            set
            {
                _selectRegionName = value;
                NotifyPropertyChanged("SelectRegionName");

            }
        }

        public string SelectKeyRegion
        {
            get { return _selectKeyRegion; }
            set
            {
                _selectKeyRegion = value;
                NotifyPropertyChanged("SelectKeyRegion");

            }
        }
        /// <summary>
        /// количество участков (форма выбор участка)
        /// </summary>
        public string CountRegions
        {
            get { return _countRegions; }
            set
            {
                _countRegions = value;
                NotifyPropertyChanged("CountRegions");

            }
        }
        /// <summary>
        /// выбранный участок (форма выбор участка)
        /// </summary>
        public string SelectRegion
        {
            get { return _selectRegion; }
            set
            {
                _selectRegion = value;
                NotifyPropertyChanged("SelectRegion");

            }
        }
        /// <summary>
        /// выбранный километраж участка
        /// </summary>
        public string SelectKilometr
        {
            get { return _selectKilometr; }
            set
            {
                _selectKilometr = value;
                NotifyPropertyChanged("SelectKilometr");

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



        //по ключу акта (шурфа) находим все трубы к которым привязан этот шурф
        public void GetSelectPipeOnkeyAct()
        {
            try
            {
                DataTable dt = CBLL.GetListPipeOnkeyAct(KeyAkt);

                SelectPipeList.Clear();
                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                from t in dt.AsEnumerable()
                select t;
                    foreach (DataRow p in query)
                    {

                        SelectPipeList.Add(
                            new SelectPipeOnGrid
                                (
                                  (p.Field<object>("nKey_pipe") != null ? p.Field<object>("nKey_pipe").ToString() : "<не указано>"),
                                  (p.Field<object>("cloc_km_beg") != null ? p.Field<object>("cloc_km_beg").ToString() : "<не указано>"),
                                  (p.Field<object>("cNumber_pipe") != null ? p.Field<object>("cNumber_pipe").ToString() : "<не указано>"),
                                  GetValueRow(p.Field<object>("nAngle")),
                                  (p.Field<object>("clength") != null ? p.Field<object>("clength").ToString() : "<не указано>"),
                                  (p.Field<object>("cdepth_pipe") != null ? p.Field<object>("cdepth_pipe").ToString() : "<не указано>"),
                                  (p.Field<object>("nCountDefect") != null ? p.Field<object>("nCountDefect").ToString() : "<не указано>"),
                                  (NumberAct),
                                   GetValueRowDiam(p.Field<object>("cdiam")),
                                   "",
                                   ""



                                )
                            );
                    }
                }
                //SelectPipeList.Add(new SelectPipeOnGrid("3", "251.490", "7704", 8, "17.1", "7", "5", NumberAct));
                GridPipeListOnSelectPipeFill();
            }
            catch (Exception e)
            {
                Report("GetSelectPipeOnkeyAct " + e.ToString());
                throw;
            }
        }

        public void GetPipe(string keyPipe)
        {
            try
            {


                GetPipeOnkeyPipe = new ObservableCollection<GridPipe>();

                DataTable dt = CBLL.GetPipeOnkey(keyPipe);
                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in dt.AsEnumerable()
                        select t;
                    foreach (DataRow p in query)
                    {
                        GetPipeOnkeyPipe.Add(
                          new GridPipe
                              (
                                (p.Field<object>("nKey_pipe") != null ? p.Field<object>("nKey_pipe").ToString() : "<не указано>"),
                                (p.Field<object>("cNumber_pipe") != null ? p.Field<object>("cNumber_pipe").ToString() : "<не указано>"),
                                (p.Field<object>("cloc_km_beg") != null ? p.Field<object>("cloc_km_beg").ToString() : "<не указано>"),

                                GetValueRow(p.Field<object>("nAngle")),
                                (p.Field<object>("clength") != null ? p.Field<object>("clength").ToString() : "<не указано>"),
                                (p.Field<object>("cdepth_pipe") != null ? p.Field<object>("cdepth_pipe").ToString() : "<не указано>"),
                                "",
                                "",
                                true,
                                GetValueRow(p.Field<object>("cdiam")),
                                "",
                                ""

                              )
                          );


                    }

                }
            }
            catch (Exception e)
            {
                Report(" GetPipe " + e.ToString());
                throw;
            }
        }

        private void SetKmOnRegonXml()
        {
            SelectedKmBegin = SelectedgridRegion.BEGINKM;
            SelectedKmEnd = SelectedgridRegion.ENDKM;
            SelectRegionName = SelectedgridRegion.NAMEREGION;
            SelectKeyRegion = SelectedgridRegion.KEYREGION;
        }

        private void SetLabelCharacterRegions()
        {
            if (SelectedgridRegion != null)
            {
                SelectRegion = SelectedgridRegion.NAMEREGION;
                SelectKilometr = SelectedgridRegion.BEGINKM + " - " + SelectedgridRegion.ENDKM;
            }


        }

        #endregion
        #endregion

        public string KeyPipe
        {
            get { return _KeyPipe; }
            set
            {
                _KeyPipe = value;

            }
        }

        public string UpdateAngelPipe
        {
            get { return _UpdateAngelPipe; }
            set
            {
                _UpdateAngelPipe = value;
                NotifyPropertyChanged("UpdateAnglePipe");

            }
        }

        public string DataShurf
        {
            get { return _dataShurf; }
            set
            {
                _dataShurf = value;
                NotifyPropertyChanged("DataShurf");

            }
        }



        #region форма selectRegion window

        //наполнение комбобокса данными для формы SelectionRegion
        public void MGListFill()
        {
            try
            {
                MGList.Clear();
                MGList.Add(new MG("Выберите МГ", "-1"));

                DataTable dt = CBLL.GetAllMG();

               
                SelectedMG = null;
                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                from t in dt.AsEnumerable()
                select t;
                    
                    foreach (DataRow p in query)
                    {
                        MGList.Add(
                            new MG
                                (
                                    (p.Field<object>("Name") != null ? p.Field<object>("Name").ToString() : "<не указано>"),
                                    (p.Field<object>("Key_MG") != null ? p.Field<object>("Key_MG").ToString() : "<не указано>")
                                )
                            );
                    }
                }
                SelectedMG = MGList[0];
            }
            catch (Exception e)
            {
                Report("MGListFill " + e.ToString());
                throw;
            }

            //MGList.Add(new MG("Выбрать", "-1"));
            //MGList.Add(new MG("Союз", "1"));
            //MGList.Add(new MG("Уренгой", "2"));
            //MGList.Add(new MG("Урал", "3"));
            //MGList.Add(new MG("Урал3", "4"));
        }
        //выбор мг из комбобокса для формы SelectionRegion
        public MG SelectedMG
        {
            get { return _selectedmg; }
            set
            {
                _selectedmg = value;
                NotifyPropertyChanged("SelectedMG");

                //загружаем нитки
                NitListFill();
            }
        }

        //наполнение данными комбобокса нити для формы SelectionRegion
        public void NitListFill()
        {
            try
            {


                NitList.Clear();
                NitList.Add(new Nit("-1", "Выберите нить"));
                SelectedNit = NitList[0]; 

                if ((SelectedMG != null) && (SelectedMG.KeyMG != "-1"))
                {
                    SelectMgKey = SelectedMG.KeyMG;
                    SelectMgName = SelectedMG.NameMG;


                    DataTable dt = CBLL.GetNitOnMg(SelectedMG.KeyMG);

                    if (dt.Rows.Count > 0)
                    {
                        IEnumerable<DataRow> query =
                    from t in dt.AsEnumerable()
                    select t;
                        
                        foreach (DataRow p in query)
                        {
                            NitList.Add(
                                new Nit
                                    (
                                      (p.Field<object>("Key_thread") != null ? p.Field<object>("Key_thread").ToString() : "<не указано>"),
                                      (p.Field<object>("NameThread") != null ? p.Field<object>("NameThread").ToString() : "<не указано>")
                                    )
                                );
                        }
                    }



                    //NitList.Add(new Nit("-1", "Выбрать"));
                    //NitList.Add(new Nit("1", "Уральск1"));
                    //NitList.Add(new Nit("2", "Уральск2"));
                    //NitList.Add(new Nit("3", "Уральск3"));
                    //NitList.Add(new Nit("4", "Уральск4"));
                }
            }
            catch (Exception e)
            {
                Report("NitListFill " + e.ToString());
                throw;
            }
        }

        public string OpencalcDefect
        {
            get { return _opencalcDefect; }
            set
            {
                _opencalcDefect = value;
                NotifyPropertyChanged("OpencalcDefect");
            }
        }

        //событие для наполненного списка труб
        public ObservableCollection<GridPipe> GridPipeList1
        {
            get { return _gridPipeList1; }
            set
            {
                _gridPipeList1 = value;
                NotifyPropertyChanged("GridPipeList");
            }
        }

        //выбор из комбобокса нити
        public Nit SelectedNit
        {
            get { return _selectedNit; }
            set
            {
                _selectedNit = value;

                NotifyPropertyChanged("SelectedNit");
                //наполняем грид участков
                GridRegionListFill();
            }
        }

        //заполняем данными таблицу участков
        public void GridRegionListFill()
        {

            if ((SelectedNit != null) && (SelectedNit.KeyNit != "-1"))
            {


                SelectNitName = SelectedNit.NameNit;
                SelectNitKey = SelectedNit.NameNit;


                GridRegionList.Clear();
                DataTable dt = CBLL.GetAllregionsOnNit(SelectedNit.KeyNit);
                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                from t in dt.AsEnumerable()
                select t;
                    foreach (DataRow p in query)
                    {

                        GridRegionList.Add(
                            new GridRegion
                                (
                                  (p.Field<object>("nkey_region") != null ? p.Field<object>("nkey_region").ToString() : "<не указано>"),
                                  (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>"),
                                  (p.Field<object>("nkm_begin") != null ? p.Field<object>("nkm_begin").ToString() : "<не указано>"),
                                  (p.Field<object>("nkm_end") != null ? p.Field<object>("nkm_end").ToString() : "<не указано>"),
                                  (p.Field<object>("lenghtRegion") != null ? p.Field<object>("lenghtRegion").ToString() : "<не указано>")

                                )
                            );
                    }
                }
                //GridRegionList.Add(new GridRegion("1", "Госграница КС Уральск", "0", "110", "110"));
                //GridRegionList.Add(new GridRegion("2", "Граница с Уральским ЛПУ", "170", "247", "77"));
                //GridRegionList.Add(new GridRegion("3", "Граница с Чижинским ЛПУ", "247", "315", "68"));
                //GridRegionList.Add(new GridRegion("4", "Граница с Алтайским ЛПУ", "350", "430", "80"));
                CountRegions = GridRegionList.Count.ToString();
            }
            else
            {
                GridRegionList.Clear();
                CountRegions = "0";
            }
        }

        public GridRegion SelectedgridRegion
        {
            get { return _selectedgridRegion; }
            set
            {
                _selectedgridRegion = value;
                if (value != null)
                    SetKmOnRegonXml();
                SetLabelCharacterRegions();
                NotifyPropertyChanged("SelectedgridRegion");
            }
        }
        #endregion

        #region форма selectPipe window

        public void GridPipeListFill()
        {
            string allActinitemPipe = "";

            //if (SelectedgridRegion !=null)
            try
            {

                if (SelectKeyRegion != null)
                {
                    GridPipeList.Clear();
                    // DataTable dt = CBLL.GetPipeOnRegion(SelectedgridRegion.KEYREGION);
                    //передаем ключ уже сохраненного xml ключ участка
                    DataTable dt = CBLL.GetPipeOnRegion(SelectKeyRegion);
                    DataTable dtListAct = CBLL.GetPipeOnRegionAllAct(SelectKeyRegion);
                    if (dt.Rows.Count > 0)
                    {
                        IEnumerable<DataRow> query =
                    from t in dt.AsEnumerable()
                    select t;
                        foreach (DataRow p in query)
                        {

                            if (dtListAct.Rows.Count > 0)
                            {
                                IEnumerable<DataRow> query2 =
                                  from tAct in dtListAct.AsEnumerable()
                                  select tAct;
                                foreach (DataRow pAct in query2)
                                {

                                    if (p.Field<object>("nKey_pipe").ToString() == pAct.Field<object>("nKey_pipe").ToString())
                                        allActinitemPipe = allActinitemPipe + pAct.Field<object>("cNumberAkt").ToString() + ", ";
                                }

                            }
                            if (allActinitemPipe != "")
                            {
                                allActinitemPipe = allActinitemPipe.Substring(0, allActinitemPipe.Length - 2);
                            }


                            GridPipeList.Add(
                                new GridPipe
                                    (
                                      (p.Field<object>("nKey_pipe") != null ? p.Field<object>("nKey_pipe").ToString() : "<не указано>"),
                                      (p.Field<object>("cNumber_pipe") != null ? p.Field<object>("cNumber_pipe").ToString() : "<не указано>"),
                                      (p.Field<object>("cloc_km_beg") != null ? p.Field<object>("cloc_km_beg").ToString() : ""),
                                      GetValueRow(p.Field<object>("nAngle")),
                                      (p.Field<object>("clength") != null ? p.Field<object>("clength").ToString() : ""),
                                      (p.Field<object>("cdepth_pipe") != null ? p.Field<object>("cdepth_pipe").ToString() : ""),
                                      (p.Field<object>("nCountDefect") != null ? p.Field<object>("nCountDefect").ToString() : ""),
                                      (allActinitemPipe),
                                      false,
                                      GetValueRowDiam(p.Field<object>("cdiam")),
                                      (p.Field<object>("depthdef") != null ? p.Field<object>("depthdef").ToString() : ""),
                                      (p.Field<object>("sqldef") != null ? p.Field<object>("sqldef").ToString() : "")

                                    )
                                );



                            allActinitemPipe = "";
                        }

                        double max = GridPipeList.Max(r => GetValueRow(r.KM));
                        double min = GridPipeList.Min(r => GetValueRow(r.KM));
                        LblKmRegion = min.ToString() + " - " + max.ToString();

                        double minNumberPipe = GridPipeList.Min(r => GetValueRow(r.NUMBERPIPE));

                        double maxNumberPipe = GridPipeList.Max(r => GetValueRow(r.NUMBERPIPE));
                        LblNumberPipeRegion = minNumberPipe.ToString() + " - " + maxNumberPipe.ToString();

                    }


                    //GridPipeList.Add(new GridPipe("1", "7701", "251.251", 6, "17.1", "17", "79", "", false));
                    //GridPipeList.Add(new GridPipe("2", "7702", "251.330", 1, "16.1", "80", "25", "", false));
                    //GridPipeList.Add(new GridPipe("3", "7703", "251.410", 2, "18.1", "20", "80", "", false));
                    //GridPipeList.Add(new GridPipe("4", "7704", "251.490", 8, "17.1", "7", "5", "18", false));
                    //GridPipeList.Add(new GridPipe("5", "7705", "251.569", 5, "19.1", "5", "20", "", false));
                    //GridPipeList.Add(new GridPipe("6", "7706", "251.251", 9, "18.1", "3", "10", "1", false));

                    SelectPipeList.Clear();

                }
            }
            catch (Exception ee)
            {
                string t = ee.ToString();
            }
        }
        /// <summary>
        /// километраж участков в форме выбор труб
        /// </summary>
        public string LblKmRegion
        {
            get { return _lblKmRegion; }
            set
            {
                _lblKmRegion = value;
                NotifyPropertyChanged("LblKmRegion");

            }
        }
        /// <summary>
        /// номера труб(первая-последняя) в флрме выбора труб
        /// </summary>
        public string LblNumberPipeRegion
        {
            get { return _lblNumberPipeRegion; }
            set
            {
                _lblNumberPipeRegion = value;
                NotifyPropertyChanged("LblNumberPipeRegion");

            }
        }


        public void GridPipeListOnSelectPipeFill()
        {
            if (SelectPipeList.Count != 0)
            {
                GridPipeListOnSelectPipeList.Clear();
                for (int i = 0; i < SelectPipeList.Count; i++)
                {
                    GridPipeListOnSelectPipeList.Add(new GridPipe(SelectPipeList[i].KeyPipe, SelectPipeList[i].NumberPipe, SelectPipeList[i].KmPipe,
                        SelectPipeList[i].ANGLESHOV, SelectPipeList[i].LENGHT, SelectPipeList[i].DEPTHPIPE, SelectPipeList[i].NUMBERDEFECT, SelectPipeList[i].NUMBERAKT, true, SelectPipeList[i].TUBERADIUS, "", ""));

                }
            }

        }
        //строка показывает километраж выбранных труб от 1 до несколько
        public string SelectPipeKm
        {
            get { return _selectPipeKm; }
            set
            {
                _selectPipeKm = value;
                NotifyPropertyChanged("SelectPipeKm");

            }
        }
        //строка показывает номера выбранных труб от 1 до несколько
        public string SelectPipeNumberPipe
        {
            get { return _selectPipeNumberPipe; }
            set
            {
                _selectPipeNumberPipe = value;
                NotifyPropertyChanged("SelectPipeNumberPipe");

            }
        }

        //выбор в таблице GridPipe
        public GridPipe SelectedGridPipeList
        {
            get { return _selectGridPipe; }
            set
            {
                _selectGridPipe = value;
                NotifyPropertyChanged("SelectedGridPipeList");
                //if (SelectedGridPipeList !=null)
                //    GetKmPipe = SelectedGridPipeList.KM;
            }
        }

        //выбор км из таблицы
        public string GetKmPipe
        {
            get { return _getkmpipe; }
            set
            {
                _getkmpipe = value;
                NotifyPropertyChanged("GetKmPipe");
            }
        }

        //public void SelectPipeGrid(ObservableCollection<object> selectItems)
        //{
        //    SelectPipeList.Clear();
        //    foreach (var item in selectItems)
        //    {
        //        //SelectPipeList.Add(new SelectPipeOnGrid(item.KEYPIPE,  ));
        //    }   
        //}


        #endregion

        #region форма createActShurfovaniya window

        public string NullPipe
        {
            get { return _nullPipe; }
            set
            {
                _nullPipe = value;

            }
        }


        /// <summary>
        /// создаем по кнопке новый акт шурфования - возвращает ключ акта шутфования
        /// </summary>
        /// <param name="keyAct"></param>
        /// <param name="selectPipeList"></param>
        public void SaveNewAct(Dictionary<string, string> d, string createPipe)
        {
            try
            {

                //проверяем на привязку акта к трубе
                //если пустой акт - то трубу создаем новую, не привязанную к участку
                if (SelectPipeList.Count == 0)
                {

                    int countPipe;
                    int.TryParse(createPipe, out countPipe);
                    if (countPipe > 0)
                    {
                        for (int i = 0; i < countPipe; i++)
                        {
                            string keyPipe = CBLL.CreateNewPipe();
                            SelectPipeList.Add(new SelectPipeOnGrid(keyPipe, "", "", 0, "11", "", "", "", 0.71, "", ""));
                        }
                    }
                    else
                    {
                        string keyPipe = CBLL.CreateNewPipe();
                        SelectPipeList.Add(new SelectPipeOnGrid(keyPipe, "", "", 0, "11", "", "", "", 0.71, "", ""));
                    }

                    GridPipeListOnSelectPipeFill();
                }
                GetKeyAkt = CBLL.SaveAct(d, SelectPipeList);
                //GetKeyAkt = CBLL.SaveAct(numberAkt, dataShurf, keydefectoskop, keyAgentLpu, SelectPipeList, is_edited);
                KeyAkt = GetKeyAkt;

                //NumberAct = numberAkt;
                //DataAct = dataShurf.ToString();
            }
            catch (Exception ee)
            {

                throw;
            }
        }

        public string GetKeyAkt
        {
            get { return _getKeyActShurf; }
            set
            {
                _getKeyActShurf = value;
                NotifyPropertyChanged("GetKeyAkt");
                //загружаем все дефекты на трубы (для рисования труб с дефектами)
                GridDefectListFill();

            }
        }
        /// <summary>
        /// сохраняем номер акта шурфования
        /// </summary>
        public string NumberAct
        {
            get { return _numberAct; }
            set
            {
                _numberAct = value;

                NotifyPropertyChanged("NumberAct");

            }
        }
        /// <summary>
        /// сохраняем дату шурфования
        /// </summary>
        public string DataAct
        {
            get { return _dataAct; }
            set
            {
                _dataAct = value;
                NotifyPropertyChanged("DataAct");

            }
        }

        #endregion
        #region EditAct - ModulCalculationDefect


        /// <summary>
        /// по клику в таблице дефектов, строим название таблицы (нижней) для одного дефекта
        /// </summary>
        public string NumberOneDefect
        {
            get { return _numberOneDefect; }
            set
            {
                _numberOneDefect = value;
                NotifyPropertyChanged("NumberOneDefect");

            }
        }



        /// <summary>
        /// список всех дефектов на выбранных трубах(для построение трубы с дефектами)
        /// </summary>
        public void GridDefectListFill()
        {
            //KeyAkt = GetKeyAkt;
            GridDefectList.Clear();
            //должна быть сводная таблица дефетов и их расчет, причем поле в расчетах crecom - пишет "допустимый" и т.д.
            // а уже на основании этого вставляем в соответсвующие ячейки картинку
            DataTable dt = CBLL.GetDefectPipe(SelectPipeList);

            if (dt.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
            from t in dt.AsEnumerable()
            select t;
                foreach (DataRow p in query)
                {

                    GridDefectList.Add(
                        new GridDefect
                            (
                              (p.Field<object>("nDefect_key") != null ? p.Field<object>("nDefect_key").ToString() : "<не указано>"),
                              GetValueRow(p.Field<object>("nclockwise_pos")),
                              GetValueRow(p.Field<object>("nLengthDefect")),
                              GetValueRow(p.Field<object>("nwidth")),
                              GetValueRow(p.Field<object>("nprevseam_dist")),
                              (p.Field<object>("nKey_pipe") != null ? p.Field<object>("nKey_pipe").ToString() : ""),
                              (p.Field<object>("defType") != null ? p.Field<object>("defType").ToString() : "не определен"),
                              (p.Field<object>("ndepth") != null ? p.Field<object>("ndepth").ToString() : ""),
                              ("на внутренней поверхности трубы ???"),
                              (p.Field<object>("cname") != null ? p.Field<object>("cname").ToString() : "<не указано>"),
                              (p.Field<object>("nloss_metall") != null ? p.Field<object>("nloss_metall").ToString() : ""),
                              GetPictureValueRow1(p.Field<object>("asme")),
                              GetPictureValueRow1(p.Field<object>("dnv")),
                              GetPictureValueRow1(p.Field<object>("rstreng")),
                              GetTypePictureValueRow(p.Field<object>("isedited"))

                            )
                        );
                }
            }
            //GridDefectList.Add(new GridDefect("25", 3, 11870, 100, 0, "1", "царапина", "3.925", "на внутренней поверхности трубы", "7701-3312", "21", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/green.JPG", "Red", "/DEFCALC;component/Images/add.png"));
            //GridDefectList.Add(new GridDefect("2", 1.7, 6.9, 1.5, 3.3816731590062, "2", "язва", "0.0032", "на внутренней поверхности трубы", "7701-3312", "21", "/DEFCALC;component/Images/ellow1.JPG", "/DEFCALC;component/Images/green.JPG", "/DEFCALC;component/Images/red1.JPG", "Orange", "/DEFCALC;component/Images/add.png"));
            //GridDefectList.Add(new GridDefect("3", 2.7, 7.5, 1.7, 3.3816731590062, "4", "язва", "0.0032", "на внутренней поверхности трубы", "7701-3312", "21", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/ellow1.JPG", "Green", "/DEFCALC;component/Images/add.png"));
            //GridDefectList.Add(new GridDefect("4", 5.7, 5.5, 1.9, 3.3816731590062, "5", "язва", "0.0032", "на внутренней поверхности трубы", "7701-3312", "21", "/DEFCALC;component/Images/green.JPG", "/DEFCALC;component/Images/ellow1.JPG", "/DEFCALC;component/Images/red1.JPG", "Red", "/DEFCALC;component/Images/add.png"));
            //GridDefectList.Add(new GridDefect("5", 10.7, 4.8, 1.4, 3.3816731590062, "6", "язва", "0.0032", "на внутренней поверхности трубы", "7701-3312", "21", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/red1.JPG", "Red", "/DEFCALC;component/Images/add.png"));
            //GridDefectList.Add(new GridDefect("6", 8.7, 6.8, 1.8, 3.3816731590062, "3", "язва", "0.0032", "на внутренней поверхности трубы", "7701-3312", "21", "/DEFCALC;component/Images/ellow1.JPG", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/ellow1.JPG", "Red", "/DEFCALC;component/Images/add.png"));

            // GridDefectList1 = GridDefectList;
        }

        //&&&&&&&&&&&&&&&&&&& надо продумать!!
        public void GridDefectListOnControlCalc()
        {
            //KeyAkt = GetKeyAkt;
            GridDefectList1.Clear();
            //должна быть сводная таблица дефетов и их расчет, причем поле в расчетах crecom - пишет "допустимый" и т.д.
            // а уже на основании этого вставляем в соответсвующие ячейки картинку
            DataTable dt = CBLL.GetDefectPipe(SelectPipeList);

            if (dt.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
            from t in dt.AsEnumerable()
            select t;
                foreach (DataRow p in query)
                {

                    GridDefectList1.Add(
                        new GridDefect
                            (
                              (p.Field<object>("nDefect_key") != null ? p.Field<object>("nDefect_key").ToString() : "<не указано>"),
                              GetValueRow(p.Field<object>("nclockwise_pos")),
                              GetValueRow(p.Field<object>("nLengthDefect")),
                              GetValueRow(p.Field<object>("nwidth")),
                              GetValueRow(p.Field<object>("nprevseam_dist")),
                              (p.Field<object>("nKey_pipe") != null ? p.Field<object>("nKey_pipe").ToString() : ""),
                              (p.Field<object>("defType") != null ? p.Field<object>("defType").ToString() : "<не указано>"),
                              (p.Field<object>("ndepth") != null ? p.Field<object>("ndepth").ToString() : ""),
                              ("на внутренней поверхности трубы ???"),
                              (p.Field<object>("cname") != null ? p.Field<object>("cname").ToString() : "<не указано>"),
                              (p.Field<object>("nloss_metall") != null ? p.Field<object>("nloss_metall").ToString() : ""),
                              GetPictureValueRow1(p.Field<object>("asme")),
                              GetPictureValueRow1(p.Field<object>("dnv")),
                              GetPictureValueRow1(p.Field<object>("rstreng")),
                              GetTypePictureValueRow(p.Field<object>("isedited"))

                            )
                        );
                }
            }
            OpencalcDefect = "1";
        }
        /// <summary>
        /// список всех дефектов на выбраннной(одной трубе)
        /// </summary>
        public void GridDefectListFillOnKeyPipe(string keyPipe)
        {
            //KeyAkt = GetKeyAkt;
            GridDefectListOnkeypipe.Clear();
            //должна быть сводная таблица дефетов и их расчет, причем поле в расчетах crecom - пишет "допустимый" и т.д.
            // а уже на основании этого вставляем в соответсвующие ячейки картинку
            DataTable dt = CBLL.GetDefectOnkeypipe(keyPipe);
            if (dt.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
            from t in dt.AsEnumerable()
            select t;
                //Color aa = new Color(Colors.Red.Green.Orange); 
                foreach (DataRow p in query)
                {

                    GridDefectListOnkeypipe.Add(
                        new GridDefect
                            (
                              (p.Field<object>("nDefect_key") != null ? p.Field<object>("nDefect_key").ToString() : "<не указано>"),
                              GetValueRow(p.Field<object>("nclockwise_pos")),
                              GetValueRow(p.Field<object>("nLengthDefect")),
                              GetValueRow(p.Field<object>("nwidth")),
                              GetValueRow(p.Field<object>("nprevseam_dist")),
                              (p.Field<object>("nKey_pipe") != null ? p.Field<object>("nKey_pipe").ToString() : ""),
                              (p.Field<object>("defType") != null ? p.Field<object>("defType").ToString() : "<не указано>"),
                              (p.Field<object>("ndepth") != null ? p.Field<object>("ndepth").ToString() : ""),
                              ("на внутренней поверхности трубы ???"),
                              (p.Field<object>("cname") != null ? p.Field<object>("cname").ToString() : ""),
                              GetLossMetal(p.Field<object>("ndepth"), p.Field<object>("cdepth_pipe")),
                        //(p.Field<object>("nloss_metall") != null ? p.Field<object>("nloss_metall").ToString() : ""),
                              GetPictureValueRow1(p.Field<object>("asme")),
                              GetPictureValueRow1(p.Field<object>("dnv")),
                              GetPictureValueRow1(p.Field<object>("rstreng")),
                              GetTypePictureValueRow(p.Field<object>("isedited"))


                            )
                        );
                }

                ColorDefectPipe = true;
            }

        }
        //public void GridDefectListFill1()
        //{

        //    GridDefectList.Clear();
        //    //должна быть сводная таблица дефетов и их расчет, причем поле в расчетах crecom - пишет "допустимый" и т.д.
        //    // а уже на основании этого вставляем в соответсвующие ячейки картинку
        //    DataTable dt = CBLL.GetDefectPipe(SelectPipeList);

        //    GridDefectList.Add(new GridDefect("1", 4.7, 5.5, 1.3, 7.3816731590062, "1", "язва", "0.0032", "на внутренней поверхности трубы", "7701-3312", "21", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/red1.JPG", "/DEFCALC;component/Images/green.JPG", "Red", "/DEFCALC;component/Images/add.png"));
        //    GridDefectList.Add(new GridDefect("2", 1.7, 6.9, 1.5, 7.3816731590062, "3", "язва", "0.0032", "на внутренней поверхности трубы", "7701-3312", "21", "/DEFCALC;component/Images/ellow1.JPG", "/DEFCALC;component/Images/green.JPG", "/DEFCALC;component/Images/red1.JPG", "Orange", "/DEFCALC;component/Images/add.png"));

        //    GridDefectList1 = GridDefectList;
        //}


        //ключ дефекта в строке  таблицы дефектов
        public string GetSelectKeyDefect
        {
            get { return _getSelectKeyDefect; }
            set
            {
                _getSelectKeyDefect = value;
                NotifyPropertyChanged("GetSelectKeyDefect");
                //загружаем расчеты для данного дефекта(если есть)
                //if (!string.IsNullOrWhiteSpace(_getSelectKeyDefect))
                //{
                //    CalculateDefect();    
                //}

            }
        }
        /// <summary>
        /// расчеты для данного дефекта(входной параметр - ключ дефекта)
        /// </summary>
        public void CalculateDefect()
        {

            DopDefect = "";
            MaxPressureDopASME = "";
            DestroyPressureASME = "";
            MaxDopLenghtDefASME = "";
            MaxDopDepthASME = "";
            RecomResponsASME = "";
            DestroyPressureDNV = "";
            DezPressureDNV = "";
            KoefDezPressureDNV = "";
            ColorDNV = Brushes.White;
            RecomResponsDNV = "";
            DestroyPressureRestreng = "";
            CoefZapRestreng = "";
            RecomResponsRestreng = "";
            ColorASME = Brushes.White;
            ColorRstreng = Brushes.White;


            CalculateDefectListDNV.Clear();
            CalculateDefectListASME.Clear();
            CalculateDefectListRSTRENG.Clear();
            //по каждому расчету своя выборка(хотя таблица одна и та же )
            // 1- DNV, 2-ASME, 3-RSTRENG
            DataTable dt = CBLL.GetCalculateOneDefect(GetSelectKeyDefect, "1");

            if (dt.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in dt.AsEnumerable()
                    select t;
                foreach (DataRow p in query)
                {

                    CalculateDefectListDNV.Add(
                        new CalculateDefectDNV
                            (
                            (p.Field<object>("nDefect_key") != null
                                 ? p.Field<object>("nDefect_key").ToString()
                                 : "<не указано>"),
                            (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : ""),
                            (p.Field<object>("nDestroyPressure") != null
                                 ? p.Field<object>("nDestroyPressure").ToString()
                                 : ""),
                            (p.Field<object>("nMaxPressureDop") != null
                                 ? p.Field<object>("nMaxPressureDop").ToString()
                                 : ""),
                            (p.Field<object>("nCoefZap") != null
                                 ? p.Field<object>("nCoefZap").ToString()
                                 : ""),
                            GetPictureValueRow(p.Field<object>("cName")),
                            (p.Field<object>("cRecomRespons") != null
                                 ? p.Field<object>("cRecomRespons").ToString()
                                 : "")
                            )
                        );
                }

                DestroyPressureDNV = CalculateDefectListDNV[0].DestroyPressureDNV;
                DezPressureDNV = CalculateDefectListDNV[0].DezPressureDNV;
                KoefDezPressureDNV = CalculateDefectListDNV[0].KoefDezPressureDNV;
                ColorDNV = CalculateDefectListDNV[0].ColorDNV;
                RecomResponsDNV = CalculateDefectListDNV[0].RecomResponsDNV;
            }
            DataTable dt2 = CBLL.GetCalculateOneDefect(GetSelectKeyDefect, "2");
            if (dt2.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in dt2.AsEnumerable()
                    select t;
                foreach (DataRow p in query)
                {

                    CalculateDefectListASME.Add(
                        new CalculateDefectASME
                            (
                            (p.Field<object>("nDefect_key") != null
                                 ? p.Field<object>("nDefect_key").ToString()
                                 : "<не указано>"),
                            (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : ""),
                            (p.Field<object>("nMaxPressureDop") != null
                                 ? p.Field<object>("nMaxPressureDop").ToString()
                                 : ""),
                            (p.Field<object>("nDestroyPressure") != null
                                 ? p.Field<object>("nDestroyPressure").ToString()
                                 : ""),
                            (p.Field<object>("nMaxDopLenghtDefASME") != null
                                 ? p.Field<object>("nMaxDopLenghtDefASME").ToString()
                                 : ""),
                                  (p.Field<object>("nMaxDopDepthASME") != null
                                 ? p.Field<object>("nMaxDopDepthASME").ToString()
                                 : ""),
                            (p.Field<object>("cRecomRespons") != null
                                 ? p.Field<object>("cRecomRespons").ToString()
                                 : ""),
                                  GetPictureValueRow(p.Field<object>("cName"))
                            )
                        );
                }

                MaxPressureDopASME = CalculateDefectListASME[0].MaxPressureDopASME;
                DestroyPressureASME = CalculateDefectListASME[0].DestroyPressureASME;
                MaxDopLenghtDefASME = CalculateDefectListASME[0].MaxDopLenghtDefASME;
                MaxDopDepthASME = CalculateDefectListASME[0].MaxDopDepthASME;
                RecomResponsASME = CalculateDefectListASME[0].RecomResponsASME;
                ColorASME = CalculateDefectListASME[0].ColorASME;
            }

            DataTable dt3 = CBLL.GetCalculateOneDefect(GetSelectKeyDefect, "3");

            if (dt3.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in dt3.AsEnumerable()
                    select t;
                foreach (DataRow p in query)
                {

                    CalculateDefectListRSTRENG.Add(
                        new CalculateDefectRSTRENG
                            (
                            (p.Field<object>("nDefect_key") != null
                                 ? p.Field<object>("nDefect_key").ToString()
                                 : "<не указано>"),
                            (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : ""),
                            (p.Field<object>("nDestroyPressure") != null
                                 ? p.Field<object>("nDestroyPressure").ToString()
                                 : ""),
                                  (p.Field<object>("nCoefZap") != null
                                 ? p.Field<object>("nCoefZap").ToString()
                                 : ""),
                            (p.Field<object>("cRecomRespons") != null
                                 ? p.Field<object>("cRecomRespons").ToString()
                                 : ""),
                                  GetPictureValueRow(p.Field<object>("cName"))
                            )
                        );
                }
                DestroyPressureRestreng = CalculateDefectListRSTRENG[0].DestroyPressureRestreng;
                CoefZapRestreng = CalculateDefectListRSTRENG[0].CoefZapRestreng;
                RecomResponsRestreng = CalculateDefectListRSTRENG[0].RecomResponsRestreng;
                ColorRstreng = CalculateDefectListRSTRENG[0].ColorRestreng;


            }

            //DopDefect = CalculateDefectListDNV[0].DopDefect;

            //////////////////////////////////////////////////////////////////////////
            //заполняем таблицу расчета по одному дефекту
            GridOneDefectCalc.Clear();

            GridOneDefectCalc.Add(
                new GridOneDefectCalc(
                    "Максимальное безопасное давление, МПа",
                    MaxPressureDopASME,
                    DezPressureDNV,
                    "",
                    ""
                    )
                );

            GridOneDefectCalc.Add(
              new GridOneDefectCalc(
                  "Разрушающее давление, МПа",
                  DestroyPressureASME,
                  DestroyPressureDNV,
                  "",
                  DestroyPressureRestreng
                  )
              );
            GridOneDefectCalc.Add(
               new GridOneDefectCalc(
                   "Максимально допустимая длина дефекта, мм",
                   MaxDopLenghtDefASME,
                   "",
                   "",
                   ""
                   )
               );
            GridOneDefectCalc.Add(
              new GridOneDefectCalc(
                  "Максимально допустимая глубина дефекта, мм",
                   MaxDopDepthASME,
                  "",
                  "",
                  ""
                  )
              );
            GridOneDefectCalc.Add(
              new GridOneDefectCalc(
                  "Коэффициент безопасного давления, б/р",
                  "",
                  KoefDezPressureDNV,
                  "",
                  ""
                  )
              );
            GridOneDefectCalc.Add(
             new GridOneDefectCalc(
                 "Коэффициент запаса, б/р",
                 "",
                 "",
                 "",
                 CoefZapRestreng
                 )
             );
            GridOneDefectCalc.Add(
            new GridOneDefectCalc(
                "Рекомендации по реагированию",
                RecomResponsASME,
                RecomResponsDNV,
                "",
                RecomResponsRestreng
                )
            );
            GridOneDefectCalc.Add(
           new GridOneDefectCalc(
               "",
               "",
               "",
               "",
               ""
               )
           );

            GridOneDefectCalc1 = new ObservableCollection<SelectPipeOnGrid>();





        }


        public DefectActions DefectAction
        {

            get { return _defectAction; }
            set
            {
                _defectAction = value;
                NotifyPropertyChanged("DefectAction");

            }
        }

        public NameFormWindows NameFormWindow
        {
            get { return _nameFormWindow; }
            set
            {
                _nameFormWindow = value;
                NotifyPropertyChanged("NameFormWindow");

            }
        }

        #endregion



        #region ListOfActs

        public void GridListAct()
        {
            try
            {

                GridActList.Clear();
                SelectedgridAct = null;
                string placestate = "<не определено>";
                //передаем список труб 

                DataTable dt = CBLL.GetListAct();
                DataTable dtr = CBLL.GetAllRegions();
                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in dt.AsEnumerable()
                        select t;
                    foreach (DataRow p in query)
                    {

                        if (p.Field<object>("nkey_region") != null)
                        {
                            if (dtr.Rows.Count > 0)
                            {
                                IEnumerable<DataRow> query2 =
                                    from tAct in dtr.AsEnumerable()
                                    select tAct;
                                foreach (DataRow pr in query2)
                                {

                                    if (p.Field<object>("nkey_region").ToString() ==
                                        pr.Field<object>("nkey_region").ToString())
                                    {
                                        placestate = pr.Field<object>("cname").ToString();
                                        break;
                                    }

                                }

                            }
                        }

                        string numberPipeOnKeyShurfList = "";
                        string firstnemberPipe = "";
                        string firstAnglePipe = "";
                        string firstKm = "";
                        string allKmPipeList = "";

                        string nShurfKey = p.Field<object>("nShurf_key") != null
                                               ? p.Field<object>("nShurf_key").ToString()
                                               : "";

                        if (nShurfKey != "")
                        {
                            //список всех номеров труб на конкретном шурфе
                            DataTable dtNumberPipe = CBLL.GetNumberPipeOnAkt(nShurfKey);

                            if (dtNumberPipe.Rows.Count > 0)
                            {
                                firstnemberPipe = (dtNumberPipe.Rows[0].Field<object>("cNumber_pipe") != null
                                                       ? dtNumberPipe.Rows[0].Field<object>("cNumber_pipe").ToString()
                                                       : "<не определено>");
                                firstAnglePipe = (dtNumberPipe.Rows[0].Field<object>("nangle") != null
                                                      ? dtNumberPipe.Rows[0].Field<object>("nangle").ToString()
                                                      : "<не определено>");
                                firstKm = (dtNumberPipe.Rows[0].Field<object>("cloc_km_beg") != null
                                               ? dtNumberPipe.Rows[0].Field<object>("cloc_km_beg").ToString()
                                               : "<не определено>");
                                for (int ii = 0; ii < dtNumberPipe.Rows.Count; ii++)
                                {
                                    numberPipeOnKeyShurfList = numberPipeOnKeyShurfList +
                                                               (dtNumberPipe.Rows[ii].Field<object>("cNumber_pipe") !=
                                                                null
                                                                    ? dtNumberPipe.Rows[ii].Field<object>("cNumber_pipe")
                                                                                           .ToString()
                                                                    : "<не определено>") + ", ";
                                    allKmPipeList = allKmPipeList +
                                                    (dtNumberPipe.Rows[ii].Field<object>("cloc_km_beg") != null
                                                         ? dtNumberPipe.Rows[ii].Field<object>("cloc_km_beg").ToString()
                                                         : "<не определено>") + ", ";
                                }
                            }




                            //if (dtNumberPipe.Rows.Count > 0)
                            //{
                            //    IEnumerable<DataRow> query3 =
                            //      from tNumberPipe in dtNumberPipe.AsEnumerable()
                            //      select tNumberPipe;
                            //    foreach (DataRow numberPipeList in query3)
                            //    {
                            //        numberPipeOnKeyShurfList = numberPipeOnKeyShurfList + (numberPipeList.Field<object>("cNumber_pipe") != null ? p.Field<object>("cNumber_pipe").ToString() : "<не определено>") + ", ";
                            //    }

                            //}
                            if (numberPipeOnKeyShurfList != "")
                            {
                                numberPipeOnKeyShurfList = numberPipeOnKeyShurfList.Substring(0,
                                                                                              numberPipeOnKeyShurfList
                                                                                                  .Length - 2);
                                allKmPipeList = allKmPipeList.Substring(0, allKmPipeList.Length - 2);
                            }

                        }



                        GridActList.Add(
                            new GridAct(
                                (p.Field<object>("nShurf_key") != null ? p.Field<object>("nShurf_key").ToString() : ""),

                                (p.Field<object>("dDateShurf") != null
                                     ? p.Field<object>("dDateShurf").ToString()
                                     : "<не определено>"),
                                      GetdataTime(p.Field<object>("dDateShurf")),
                                (p.Field<object>("cNumberAkt") != null
                                     ? p.Field<object>("cNumberAkt").ToString()
                                     : "<не определено>"),

                                firstKm,
                                firstnemberPipe,
                                firstAnglePipe,
                                GetStatusAkt(p.Field<object>("isEdited")),
                                (placestate),
                                (p.Field<object>("nkey_region") != null ? p.Field<object>("nkey_region").ToString() : ""),
                                (p.Field<object>("nCountPipe") != null ? p.Field<object>("nCountPipe").ToString() : ""),
                                numberPipeOnKeyShurfList,
                                allKmPipeList,
                                (p.Field<object>("isEdited") != null
                                     ? p.Field<object>("isEdited").ToString()
                                     : "")
                                )
                            );
                        placestate = "<не определено>";
                    }

                }
            }
            catch (Exception e)
            {

                Report("Ошибка списка актов " + e.ToString());
            }

            //MGList.Clear();
            //SelectedMG = null;
            //if (dt.Rows.Count > 0)
            //{
            //    IEnumerable<DataRow> query =
            //from t in dt.AsEnumerable()
            //select t;
            //    MGList.Add(new MG("Выбрать", "-1"));
            //    foreach (DataRow p in query)
            //    {
            //        MGList.Add(
            //            new MG
            //                (
            //                    (p.Field<object>("Name") != null ? p.Field<object>("Name").ToString() : "<не указано>"),
            //                    (p.Field<object>("Key_MG") != null ? p.Field<object>("Key_MG").ToString() : "<не указано>")
            //                )
            //            );
            //    }
            //}


            //GridActList.Add(new GridAct("3", "23.04.2001", "23", "256.67", "56", "Ivanov"));
            //GridActList.Add(new GridAct("4", "23.05.2001", "24", "257.67", "56", "Petrov"));
            //GridActList.Add(new GridAct("5", "23.06.2001", "25", "258.67", "56", "Sidorov"));
            //GridActList.Add(new GridAct("6", "23.07.2001", "26", "259.67", "56", "Svata"));
            //GridActList.Add(new GridAct("7", "23.08.2001", "27", "256.67", "56", "Anna"));

        }

        public GridAct SelectedgridAct
        {
            get { return _selectedgridAct; }
            set
            {
                _selectedgridAct = value;
                if (value != null)
                    //  SetKmOnRegonXml();
                    NotifyPropertyChanged("SelectedgridAct");
            }
        }



        #endregion


        #region ModulCalculationDefect


        #endregion


        public void SetSignatureAkt(string iseditakt)
        {
            try
            {
                if (iseditakt == "1")
                {
                    ASBLL.SetSignatureAkt(KeyAkt, "1");
                    SignatureAkt = "1";
                }
                else
                {
                    ASBLL.SetSignatureAkt(KeyAkt, "0");
                    SignatureAkt = "0";
                }

            }
            catch (Exception e)
            {

                Report("Ошибка подписи акта " + e);
            }
        }




        public ExportDefectAkt.Export ExportDefectAktModel
        {
            get { return _exportDefectAkt; }
            set
            {
                _exportDefectAkt = value;

                NotifyPropertyChanged("CreateExportDefectAkt");

            }
        }


        public void FileExportToIAS(string selectAkt)
        {
            ExportDefectAkt.Export shurf = new ExportDefectAkt.Export();
            shurf.DEFECTS = new List<ExportDefectAkt.DEFECT>();
            shurf.SHURF = new List<ExportDefectAkt.SHURF>();
            shurf.SHURF_TRUBA = new List<ExportDefectAkt.SHURF_TRUBA>();
            //shurf = new List<ExportDefectAkt.SHURF>(); 

            //shurf.SHURF  = new ExportDefectAkt.SHURF();
            //shDEFECTS = new List<ExportDefectAkt.DEFECT>();



            DataTable dt = CBLL.GetAktOnIas(selectAkt);
            if (dt.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in dt.AsEnumerable()
                    select t;
                foreach (DataRow p in query)
                {

                    ExportDefectAkt.SHURF shurfAdd = new ExportDefectAkt.SHURF();
                    //shurf.SHURF = new ExportDefectAkt.SHURF();
                    //shurf.SHURF..DEFECTS = new List<ExportDefectAkt.DEFECT>();



                    shurfAdd.NSHURF_KEY = p.Field<object>("nShurf_key").ToString();
                    shurfAdd.CNUMBERAKT = (p.Field<object>("cNumberAkt") != null
                                               ? p.Field<object>("cNumberAkt").ToString()
                                               : "");
                    shurfAdd.DDATESHURF = (p.Field<object>("dDateShurf") != null
                                               ? p.Field<object>("dDateShurf").ToString()
                                               : "");
                    shurfAdd.CRELIEF_GROUND = (p.Field<object>("crelief_ground") != null
                                                   ? p.Field<object>("crelief_ground").ToString()
                                                   : "");
                    shurfAdd.NDICT_CHARACTER_GRUNT_KEY = (p.Field<object>("nDict_Character_grunt_key") != null
                                                              ? p.Field<object>("nDict_Character_grunt_key")
                                                                 .ToString()
                                                              : "");
                    shurfAdd.NDEPTH_LOCATION = (p.Field<object>("ndepth_location") != null
                                                    ? p.Field<object>("ndepth_location").ToString()
                                                    : "");
                    shurfAdd.NREZISTOR_GRUNT = (p.Field<object>("nrezistor_grunt") != null
                                                    ? p.Field<object>("nrezistor_grunt").ToString()
                                                    : "");
                    shurfAdd.NVOLTAGE_PIPE = (p.Field<object>("nvoltage_pipe") != null
                                                  ? p.Field<object>("nvoltage_pipe").ToString()
                                                  : "");
                    shurfAdd.NDEPTH = (p.Field<object>("ndepth") != null ? p.Field<object>("ndepth").ToString() : "");
                    shurfAdd.NLENGTH = (p.Field<object>("nlength") != null
                                            ? p.Field<object>("nlength").ToString()
                                            : "");
                    shurfAdd.CWATER_LEVEL = (p.Field<object>("cwater_level") != null
                                                 ? p.Field<object>("cwater_level").ToString()
                                                 : "");
                    shurfAdd.NTYPE_IZOL_KEY = (p.Field<object>("ntype_izol_key") != null
                                                   ? p.Field<object>("ntype_izol_key").ToString()
                                                   : "");
                    shurfAdd.NDICT_IZOL_STATE_KEY = (p.Field<object>("nDict_Izol_state_key") != null
                                                         ? p.Field<object>("nDict_Izol_state_key").ToString()
                                                         : "");
                    shurfAdd.NNUMBER_SKIN = (p.Field<object>("nNumber_skin") != null
                                                 ? p.Field<object>("nNumber_skin").ToString()
                                                 : "");
                    shurfAdd.NNUMBEROBERTKA = (p.Field<object>("nNumberObertka") != null
                                                   ? p.Field<object>("nNumberObertka").ToString()
                                                   : "");
                    shurfAdd.NDICT_AVAILABI_OVERL_KEY = (p.Field<object>("ndict_availabi_overl_key") != null
                                                             ? p.Field<object>("ndict_availabi_overl_key")
                                                                .ToString()
                                                             : "");
                    shurfAdd.NDICT_ADHERENC_PIPE_KEY = (p.Field<object>("ndict_adherenc_pipe_key") != null
                                                            ? p.Field<object>("ndict_adherenc_pipe_key").ToString()
                                                            : "");
                    shurfAdd.NDICT_AVAILABIL_DAMP_KEY = (p.Field<object>("ndict_availabil_damp_key") != null
                                                             ? p.Field<object>("ndict_availabil_damp_key")
                                                                .ToString()
                                                             : "");
                    shurfAdd.NINSPECT_SQUARE = (p.Field<object>("nInspect_square") != null
                                                    ? p.Field<object>("nInspect_square").ToString()
                                                    : "");
                    shurfAdd.NINSPECT_SQUARE_KOROZ = (p.Field<object>("nInspect_square_koroz") != null
                                                          ? p.Field<object>("nInspect_square_koroz").ToString()
                                                          : "");
                    shurfAdd.CVID_KOROZ_DAMAGE = (p.Field<object>("cVid_koroz_damage") != null
                                                      ? p.Field<object>("cVid_koroz_damage").ToString()
                                                      : "");
                    shurfAdd.CCONCULUSION = (p.Field<object>("cConculusion") != null
                                                 ? p.Field<object>("cConculusion").ToString()
                                                 : "");
                    shurfAdd.DDATE_VISIT = (p.Field<object>("dDate_visit") != null
                                                ? p.Field<object>("dDate_visit").ToString()
                                                : "");
                    shurfAdd.DCHANGE_DATE = (p.Field<object>("dChange_date") != null
                                                 ? p.Field<object>("dChange_date").ToString()
                                                 : "");
                    shurfAdd.DEDITING_DATA = (p.Field<object>("dEditing_data") != null
                                                  ? p.Field<object>("dEditing_data").ToString()
                                                  : "");
                    //shurfAdd.NDICT_TEMPERATURN_KOEF_KEY = (p.Field<object>("nDict_temperaturn_koef_key") != null
                    //                                           ? p.Field<object>("nDict_temperaturn_koef_key")
                    //                                              .ToString()
                    //                                           : "");
                    //shurfAdd.NCKZ1 = (p.Field<object>("nCkz1") != null ? p.Field<object>("nCkz1").ToString() : "");
                    //shurfAdd.I1 = (p.Field<object>("I1") != null ? p.Field<object>("I1").ToString() : "");
                    //shurfAdd.U1 = (p.Field<object>("U1") != null ? p.Field<object>("U1").ToString() : "");
                    //shurfAdd.I2 = (p.Field<object>("I2") != null ? p.Field<object>("I2").ToString() : "");
                    //shurfAdd.U2 = (p.Field<object>("U2") != null ? p.Field<object>("U2").ToString() : "");
                    //shurfAdd.NCKZ2 = (p.Field<object>("nCkz2") != null ? p.Field<object>("nCkz2").ToString() : "");

                    shurf.SHURF.Add(shurfAdd);

                    DataTable dtd = CBLL.GetDefectOnIas(p.Field<object>("nShurf_key").ToString());
                    if (dtd.Rows.Count > 0)
                    {

                        IEnumerable<DataRow> query2 =
                            from t in dtd.AsEnumerable()
                            select t;
                        {
                            foreach (DataRow p2 in query2)
                            {
                                double _keydefect = (GetValueRow(p2.Field<object>("nDefect_key"))%100);
                                if ((p2.Field<object>("isEdited").ToString() == "1") && (_keydefect.ToString() != "0"))
                                {
                                }
                                else
                                {
                              
                                ExportDefectAkt.DEFECT ed = new ExportDefectAkt.DEFECT();

                                ed.NDEFECT_KEY = p2.Field<object>("nDefect_key").ToString();
                                ed.NLENGTHDEFECT = (p2.Field<object>("nlengthDefect") != null
                                                        ? p2.Field<object>("nlengthDefect").ToString()
                                                        : "");
                                ed.NTYPEDEFECT_KEY = (p2.Field<object>("nTypeDefect_Key") != null
                                                          ? p2.Field<object>("nTypeDefect_Key").ToString()
                                                          : "");
                                ed.NCLOCKWISE_POS = (p2.Field<object>("nclockwise_pos") != null
                                                         ? p2.Field<object>("nclockwise_pos").ToString()
                                                         : "");
                                ed.NWIDTH = (p2.Field<object>("nwidth") != null
                                                 ? p2.Field<object>("nwidth").ToString()
                                                 : "");
                                ed.NDEPTH = (p2.Field<object>("nwidth") != null
                                                 ? p2.Field<object>("nwidth").ToString()
                                                 : "");
                                ed.CNAME = (p2.Field<object>("cName") != null
                                                ? p2.Field<object>("cName").ToString()
                                                : "");
                                //ed.NLOSS_METALL = (p2.Field<object>("nloss_metall") != null
                                //                       ? p2.Field<object>("nloss_metall").ToString()
                                //                       : "");
                                ed.NPREVSEAM_DIST = (p2.Field<object>("nprevseam_dist") != null
                                                         ? p2.Field<object>("nprevseam_dist").ToString()
                                                         : "");
                                ed.NNEXT_DIST = (p2.Field<object>("nnext_dist") != null
                                                     ? p2.Field<object>("nnext_dist").ToString()
                                                     : "");
                                ed.NPREVMARK_DIST = (p2.Field<object>("nprevmark_dist") != null
                                                         ? p2.Field<object>("nprevmark_dist").ToString()
                                                         : "");
                                ed.NNEXTMARK_DIST = (p2.Field<object>("nnextmark_dist") != null
                                                         ? p2.Field<object>("nnextmark_dist").ToString()
                                                         : "");
                                ed.CSTRESS_DESCR = (p2.Field<object>("cstress_descr") != null
                                                        ? p2.Field<object>("cstress_descr").ToString()
                                                        : "");
                                ed.NKEY_PIPE = (p2.Field<object>("nKey_pipe") != null
                                                    ? p2.Field<object>("nKey_pipe").ToString()
                                                    : "");
                                ed.NDICT_CALC_TYPE_KEY = (p2.Field<object>("nDict_Calc_Type_Key") != null
                                                              ? p2.Field<object>("nDict_Calc_Type_Key").ToString()
                                                              : "");
                                ed.NDEPTH_POS_KEY = (p2.Field<object>("ndepth_pos_Key") != null
                                                         ? p2.Field<object>("ndepth_pos_Key").ToString()
                                                         : "");
                                ed.NMETOD_USTR_DEF_KEY = (p2.Field<object>("nMetod_Ustr_Def_Key") != null
                                                              ? p2.Field<object>("nMetod_Ustr_Def_Key").ToString()
                                                              : "");
                                ed.DCHANGE_DATE = (p2.Field<object>("dChange_date") != null
                                                       ? p2.Field<object>("dChange_date").ToString()
                                                       : "");
                                ed.CDATAUSTRDEFECT = (p2.Field<object>("cDataUstrDefect") != null
                                                          ? p2.Field<object>("cDataUstrDefect").ToString()
                                                          : "");
                                ed.NDICT_DEFECT_USTR_KEY = (p2.Field<object>("nDict_defect_ustr_key") != null
                                                                ? p2.Field<object>("nDict_defect_ustr_key")
                                                                    .ToString()
                                                                : "");
                                ed.ISEDITED = (p2.Field<object>("isEdited") != null
                                                   ? p2.Field<object>("isEdited").ToString()
                                                   : "");
                                ed.NMETOD_USTR_DEF_OTREM_KEY = (p2.Field<object>("nMetod_Ustr_Def_Otrem_Key") !=
                                                                null
                                                                    ? p2.Field<object>("nMetod_Ustr_Def_Otrem_Key")
                                                                        .ToString()
                                                                    : "");
                                ed.NGROUP_KEY = (p2.Field<object>("nGroup_key") != null
                                                     ? p2.Field<object>("nGroup_key").ToString()
                                                     : "");

                                shurf.DEFECTS.Add(ed);
                            }

                        }
                        }
                    }
                    DataTable dtst = CBLL.GetShurf_Truba(selectAkt);
                    if (dtst.Rows.Count > 0)
                    {
                        IEnumerable<DataRow> queryst =
                            from t in dtst.AsEnumerable()
                            select t;
                        foreach (DataRow pst in queryst)
                        {
                            ExportDefectAkt.SHURF_TRUBA st = new ExportDefectAkt.SHURF_TRUBA();
                            st.NKEY_PIPE = pst.Field<object>("nKey_pipe").ToString();
                            st.NSHURF_KEY = pst.Field<object>("nShurf_key").ToString();

                            shurf.SHURF_TRUBA.Add(st);
                        }
                    }
                }
            }

            ExportDefectAktModel = shurf;

        }
        /// <summary>
        /// Список тип дефекта 
        /// </summary>
        public void DictListTypeDefectFill()
        {
            DataTable dt = CBLL.GetDictTypeDefect();

            DictListTypeDefect.Clear();

            if (dt.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in dt.AsEnumerable()
                    select t;

                foreach (DataRow p in query)
                {
                    DictListTypeDefect.Add(
                           new Dict
                               (
                                 (p.Field<object>("nTypeDefect_Key") != null ? p.Field<object>("nTypeDefect_Key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }

        }




        public bool ShowConfirmDialog(string msg)
        {
            bool result = false;

            // Configure the message box to be displayed
            string messageBoxText = msg;
            string caption = "Подтверждение действия";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult diagResult = MessageBox.Show(messageBoxText, caption, button, icon);
            // Process message box results
            switch (diagResult)
            {
                case MessageBoxResult.Yes:
                    result = true;
                    break;
                case MessageBoxResult.No:
                    result = false;
                    break;
            }
            return result;
        }

    }
}


