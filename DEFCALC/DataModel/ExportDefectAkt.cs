using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
   public class ExportDefectAkt
    {
        private static string ConvertToUnixTime(DateTime datatimevalue)
        {
            string result = "";
            result = (datatimevalue - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds.ToString();
            return result;

        }
 
        
        public class SHURF
        {
            private string _dDateShurf;
            private string _dchange_date;
            private string _dediting_date;

            public string NSHURF_KEY { get; set; }
            public string CNUMBERAKT { get; set; }
            public string DDATESHURF
            {
                get { return _dDateShurf; }
                set
                {
                    if (value != "")
                    {
                        try
                        {
                            DateTime dt = Convert.ToDateTime(value);
                            _dDateShurf = ConvertToUnixTime(dt);
                        }
                        catch (Exception ee)
                        {

                            _dDateShurf = "";
                        }

                    }
                    else
                    {
                        _dDateShurf = value;
                    }

                }
            }
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
            public string NDICT_DEFECTOSKOPIST_KEY { get; set; }
            public string NDICT_AGENT_LPU_KEY { get; set; }
            public string DCHANGE_DATE 
            {
                get { return _dchange_date; }
                set
                {
                    if (value != "")
                    {
                        try
                        {
                            DateTime dt = Convert.ToDateTime(value);
                            _dchange_date = ConvertToUnixTime(dt);
                        }
                        catch (Exception ee)
                        {

                            _dchange_date = "";
                        }

                    }
                    else
                    {
                        _dchange_date = value;
                    }

                }
            }
            public string DEDITING_DATA
            {
                get { return _dediting_date; }
                set
                {
                    if (value != "")
                    {
                        try
                        {
                            DateTime dt = Convert.ToDateTime(value);
                            _dediting_date = ConvertToUnixTime(dt);
                        }
                        catch (Exception ee)
                        {

                            _dediting_date = "";
                        }

                    }
                    else
                    {
                        _dediting_date = value;
                    }

                }
            }
            //public string NDICT_TEMPERATURN_KOEF_KEY { get; set; }
            //public string NCKZ1 { get; set; }
            //public string I1 { get; set; }
            //public string U1 { get; set; }
            //public string I2 { get; set; }
            //public string U2 { get; set; }
            //public string NCKZ2 { get; set; }

           
        }

        public class DEFECT
        {
            private string _dcange_date;
            public string NDEFECT_KEY { get; set; }
            public string NLENGTHDEFECT { get; set; }
            public string NTYPEDEFECT_KEY { get; set; }
            public string NCLOCKWISE_POS { get; set; }
            public string NWIDTH { get; set; }
            public string NDEPTH { get; set; }
            public string CNAME { get; set; }
            public string NLOSS_METALL { get; set; }
            public string NPREVSEAM_DIST { get; set; }
            public string NNEXT_DIST { get; set; }
            public string NPREVMARK_DIST { get; set; }
            public string NNEXTMARK_DIST { get; set; }
            public string CSTRESS_DESCR { get; set; }
            public string NKEY_PIPE { get; set; }
            public string NDICT_CALC_TYPE_KEY { get; set; }
            public string NDEPTH_POS_KEY { get; set; }
            public string NMETOD_USTR_DEF_KEY { get; set; }
            public string DCHANGE_DATE
            {

                get { return _dcange_date; }
                set
                {
                    if (value != "")
                    {
                        try
                        {
                            DateTime dt = Convert.ToDateTime(value);
                            _dcange_date = ConvertToUnixTime(dt);
                        }
                        catch (Exception ee)
                        {

                            _dcange_date = "";
                        }

                    }
                    else
                    {
                        _dcange_date = value;
                    }

                }
            }
            public string CDATAUSTRDEFECT { get; set; }
            public string NDICT_DEFECT_USTR_KEY { get; set; }
            public string ISEDITED { get; set; }
            public string NMETOD_USTR_DEF_OTREM_KEY { get; set; }
            public string NGROUP_KEY { get; set; }
        }

        public class SHURF_TRUBA
       {
            public string NSHURF_KEY { get; set; }
            public string NKEY_PIPE { get; set; }
       }

       public class Export
       {
           
           public List<DEFECT> DEFECTS { get; set; }
           public List<SHURF> SHURF { get; set; }
           public List<SHURF_TRUBA> SHURF_TRUBA { get; set; }
       }
    }
}
