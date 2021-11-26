using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DEFCALC.DataModel
{
    public class ImportAllBaza
    {
        private static string ConvertToDateTime(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp).ToString("dd.MM.yyyy");

        }

        /// <summary>
        /// Словарь Тип дефекта металла
        /// </summary>
        public class METALDEFECTTYPE
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Расчетный тип дефекта металла 
        /// </summary>
        public class METALDEFECTCALCTYPE
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Расположение дефекта в металле 
        /// </summary>
        public class LOCATIONDEFECT
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Метод устранения дефекта 
        /// </summary>
        public class METODUSTRDEFECT
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }

        /// <summary>
        /// Способ ремонта газопровода 
        /// </summary>
        public class WAYREPAIRPIPELINE
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Тип грунта
        /// </summary>
        public class TYPEOFSOIL
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Тип изоляции 
        /// </summary>
        public class INSULATIONTYPE
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Состояние защитного покрытия 
        /// </summary>
        public class PROTECTCOATINGSTATE
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Наличие влаги под изоляцией
        /// </summary>
        public class MOISTUREUNDERINSUL
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Завод изготовитель 
        /// </summary>
        public class FACTORYBUILDER
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Словарь марка стали
        /// </summary>
        public class FEELGRAD
        {
            public string NKEY { get; set; }
            public string CNAME { get; set; }
        }
        /// <summary>
        /// Соответствие марка стали - завод изготовитель
        /// </summary>
        public class EKVIVALENTMARKABUILDER
        {
            public string FEELGRADEKEY { get; set; }
            public string FACTORYBUILDERKEY { get; set; }
            public string RANGESTRANGHT { get; set; }
            public string RANGEFLUD { get; set; }
            public string MODULEJUNG { get; set; }
            public string KOEFPUASON { get; set; }
            public string KOEFLINEXPANSION { get; set; }
        }

        public class NSI
        {
            public List<METALDEFECTTYPE> METALDEFECTTYPE { get; set; }
            public List<METALDEFECTCALCTYPE> METALDEFECTCALCTYPE { get; set; }
            public List<LOCATIONDEFECT> LOCATIONDEFECT { get; set; }
            public List<METODUSTRDEFECT> METODUSTRDEFECT { get; set; }
            public List<WAYREPAIRPIPELINE> WAYREPAIRPIPELINE { get; set; }
            public List<TYPEOFSOIL> TYPEOFSOIL { get; set; }
            public List<INSULATIONTYPE> INSULATIONTYPE { get; set; }
            public List<PROTECTCOATINGSTATE> PROTECTCOATINGSTATE { get; set; }
            public List<MOISTUREUNDERINSUL> MOISTUREUNDERINSUL { get; set; }
            public List<FACTORYBUILDER> FACTORYBUILDER { get; set; }
            public List<FEELGRAD> FEELGRAD { get; set; }
            public List<EKVIVALENTMARKABUILDER> EKVIVALENTMARKABUILDER { get; set; }
        }



        public class MG
        {
            public string NAME { get; set; }
            public string KEY_MG { get; set; }
            public string NTHREAD_KEY { get; set; }
        }

        public class THREAD
        {
            public string KEY_THREAD { get; set; }
            public string NAMETHREAD { get; set; }
            public string NKM_BEG { get; set; }
            public string NKM_END { get; set; }
            public string KEY_MG { get; set; }
        }

        public class SECTION
        {
            public string NKEY_REGION { get; set; }
            public string KEY_THREAD { get; set; }
            public string CNAME { get; set; }
            public string NKM_BEGIN { get; set; }
            public string NKM_END { get; set; }
        }

        public class PIPE
        {
            private string _dcahnge_date;
            public string NKEY_PIPE { get; set; }
            public string NKEY_REGION { get; set; }
            public string CDEPTH_PIPE { get; set; }
            public string CLENGTH { get; set; }
            public string CTYPE_PIPE { get; set; }
            public string NTYPE_PIPE_KEY { get; set; }
            public string NANGLE { get; set; }
            public string NMAXPRESSURE { get; set; }
            public string NPRESSURE { get; set; }
            public string CDIAM { get; set; }
            public string NDICT_KATEGOR_UCH_KEY { get; set; }
            public string NFACTORY_BUILDER_KEY { get; set; }
            public string NDICT_FEEL_GRADE_KEY { get; set; }

            public string NPIPE_KEY { get; set; }
            public string CNUMBER_PIPE { get; set; }
            public string CLOC_KM_BEG { get; set; }
            public string LOC_KM_END { get; set; }
            public string LOC_NPIPEKEY { get; set; }
        }

        public class DEFECT
        {
            public string NMETOD_USTR_DEF_KEY { get; set; }
            public string NDEFECT_KEY { get; set; }
            public string NDEPTH_POS_KEY { get; set; }
            public string NDICT_CALC_TYPE_KEY { get; set; }
            public string NTYPEDEFECT_KEY { get; set; }
            public string NKEY_PIPE { get; set; }
            public string NLENGTHDEFECT { get; set; }
            public string NCLOCKWISE_POS { get; set; }
            public string NWIDTH { get; set; }
            public string NDEPTH { get; set; }
            public string CNAME { get; set; }
            public string NLOSS_METALL { get; set; }
            public string NPREVSEAM_DIST { get; set; }
            public string NNEXT_DIST { get; set; }
            public string CSTRESS_DESCR { get; set; }
            // public double? LOC_NKM { get; set; }
            public string LOC_NPIPEKEY { get; set; }
        }

        public class SHURF
        {
            public string NSHURF_KEY { get; set; }
            public string CNUMBERAKT { get; set; }
            public string DDATESHURF { get; set; }
            public string CRELIEF_GROUND { get; set; }
            public string NDICT_CHARACTER_GRUNT_KEY { get; set; }
            public string NDEPTH_LOCATION { get; set; }
            public string NREZISTOR_GRUNT { get; set; }
            public string NVOLTAGE_PIPE { get; set; }
            public string NDEPTH { get; set; }
            public string NLENGTH { get; set; }
            public string CWATER_LEVEL { get; set; }
            public string NTYPE_IZOL_KEY { get; set; }
            public string NDICT_IZOL_STATE_KEY { get; set; }
            public string NNUMBER_SKIN { get; set; }
            public string NNUMBEROBERTKA { get; set; }
            public string NDICT_AVAILABI_OVERL_KEY { get; set; }
            public string NDICT_ADHERENC_PIPE_KEY { get; set; }
            public string NDICT_AVAILABIL_DAMP_KEY { get; set; }
            public string NINSPECT_SQUARE { get; set; }
            public string NINSPECT_SQUARE_KOROZ { get; set; }
            public string CVID_KOROZ_DAMAGE { get; set; }
            public string CCONCULUSION { get; set; }
            public string DDATE_VISIT { get; set; }
            public string CDEFECTOSKOPIST { get; set; }
            public string CAGENT_LPU { get; set; }
            public string DCHANGE_DATE { get; set; }
            public string DEDITING_DATA { get; set; }
            public string NDICT_TEMPERATURN_KOEF_KEY { get; set; }
        }

        public class SHURFPIPE
        {
            public string NKEY_PIPE { get; set; }
            public string NSHURF_KEY { get; set; }
        }

        public class ImportIas
        {
            public NSI NSI { get; set; }
            public List<MG> MG { get; set; }
            public List<THREAD> THREAD { get; set; }
            public List<SECTION> SECTION { get; set; }
            public List<PIPE> PIPES { get; set; }
            public List<DEFECT> DEFECTS { get; set; }
            public List<SHURF> SHURFS { get; set; }
            public List<SHURFPIPE> SHURF_PIPE { get; set; }
        }

       

    }
}



//namespace DEFCALC.DataModel
//{
//   public class ImportAllBaza
//    {
//       private static string ConvertToDateTime(double timestamp)
//       {
//           DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
//           return origin.AddSeconds(timestamp).ToString("dd.MM.yyyy");

//       }

//        public class MG
//        {
//            public string NAME { get; set; }
//            public string KEY_MG { get; set; }
//            public string NTHREAD_KEY { get; set; }
//        }

//        public class THREAD
//        {
//            private double? _NKM_BEG;
//            private double? _NKM_END;
//            public string KEY_THREAD { get; set; }
//            public string NAMETHREAD { get; set; }
//            public double? NKM_BEG 
//            {  get { return _NKM_BEG; }

//                set 
//                {
//                    string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
//                    string lenghtStr = value.ToString();
//                    lenghtStr = lenghtStr.Replace(",", separator);
//                    lenghtStr = lenghtStr.Replace(".", separator);
//                    double? segLength = Convert.ToDouble(lenghtStr);
//                    _NKM_BEG = segLength;
//                } 
//            }
//            public double? NKM_END
//            {
//                get { return _NKM_END; }

//                set
//                {
//                    string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
//                    string lenghtStr = value.ToString();
//                    lenghtStr = lenghtStr.Replace(",", separator);
//                    lenghtStr = lenghtStr.Replace(".", separator);
//                    double? segLength = Convert.ToDouble(lenghtStr);
//                    _NKM_END = segLength;
//                }
//            }
//            public string KEY_MG { get; set; }
//        }

//        public class SECTION
//        {
//            private double? _NKM_END;
//            private double? _NKM_BEGIN;
//            public string NKEY_REGION { get; set; }
//            public string KEY_THREAD { get; set; }
//            public string CNAME { get; set; }
//            public double? NKM_BEGIN
//            {
//                get { return _NKM_BEGIN; }

//                set
//                {
//                    string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
//                    string lenghtStr = value.ToString();
//                    lenghtStr = lenghtStr.Replace(",", separator);
//                    lenghtStr = lenghtStr.Replace(".", separator);
//                    double? segLength = Convert.ToDouble(lenghtStr);
//                    _NKM_BEGIN = segLength;
//                }
//            }
//            public double? NKM_END
//            {
//                get { return _NKM_END; }

//                set
//                {
//                    string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
//                    string lenghtStr = value.ToString();
//                    lenghtStr = lenghtStr.Replace(",", separator);
//                    lenghtStr = lenghtStr.Replace(".", separator);
//                    double? segLength = Convert.ToDouble(lenghtStr);
//                    _NKM_END = segLength;
//                }
//            }
//        }

//        public class PIPE
//        {
//            private string _dcahnge_date;
//            public string NKEY_PIPE { get; set; }
//            public string NKEY_REGION { get; set; }
//            public double? CDEPTH_PIPE { get; set; }
//            public double? CLENGTH { get; set; }
//            public string CTYPE_PIPE { get; set; }
//            public string NTYPE_PIPE_KEY { get; set; }
//            public double? NANGLE { get; set; }
//            public double? NMAXPRESSURE { get; set; }
//            public double? NPRESSURE { get; set; }
//            public double? CDIAM { get; set; }
//            public string NDICT_KATEGOR_UCH_KEY { get; set; }
//            public string NFACTORY_BUILDER_KEY { get; set; }
//            public string NDICT_FEEL_GRADE_KEY { get; set; }
           
//            public string NPIPE_KEY { get; set; }
//            public string CNUMBER_PIPE { get; set; }
//            public double? CLOC_KM_BEG { get; set; }
//            public double? LOC_KM_END { get; set; }
//            public string LOC_NPIPEKEY { get; set; }
//        }

//        public class DEFECT
//        {
//            public string NMETOD_USTR_DEF_KEY { get; set; }
//            public string NDEFECT_KEY { get; set; }
//            public string NDEPTH_POS_KEY { get; set; }
//            public string NDICT_CALC_TYPE_KEY { get; set; }
//            public string NTYPEDEFECT_KEY { get; set; }
//            public string NKEY_PIPE { get; set; }
//            public double? NLENGTHDEFECT { get; set; }
//            public double? NCLOCKWISE_POS { get; set; }
//            public double? NWIDTH { get; set; }
//            public double? NDEPTH { get; set; }
//            public string CNAME { get; set; }
//            public string NLOSS_METALL { get; set; }
//            public double? NPREVSEAM_DIST { get; set; }
//            public double? NNEXT_DIST { get; set; }
//            public string CSTRESS_DESCR { get; set; }
//           // public double? LOC_NKM { get; set; }
//            public string LOC_NPIPEKEY { get; set; }
//        }

//        public class ImportIas
//        {
//            public List<MG> MG { get; set; }
//            public List<THREAD> THREAD { get; set; }
//            public List<SECTION> SECTION { get; set; }
//            public List<PIPE> PIPES { get; set; }
//            public List<DEFECT> DEFECTS { get; set; }
//        }

      

//    }
//}

//public string DCHANGE_DATE
//{
//    get { return _dcahnge_date; }
//    set
//    {
//        if (value != "")
//        {
//            try
//            {
//                double dt = Convert.ToDouble(value);
//                _dcahnge_date = ConvertToDateTime(dt);
//            }
//            catch (Exception ee)
//            {

//                _dcahnge_date = "";
//            }

//        }
//        else
//        {
//            _dcahnge_date = value;
//        }

//    }
//}
