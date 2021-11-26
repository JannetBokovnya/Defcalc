using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using DEFCALC.DataModel;
using System.Windows;

//using DBConn;


//using DBConn;


namespace DEFCALC
{
    class ASBLL
    {


        private static string Disconnect(SQLiteConnection connection)
        {
            try
            {
                connection.Dispose();
                return string.Empty;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        #region Тест
        /// <summary>
        /// Тестовая фцнкция подключения к Oracle БД.
        /// </summary>
        /// <returns>Возвращаемое значение</returns>
        //public static string TestOracleBbConnection()
        //{

        //    DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();

        //    DBConn.Conn connOra = databaseFactory.CreateObject("OraConn");

        //    DBConn.DBParam[] oip = new DBConn.DBParam[1];
        //    oip[0] = new DBConn.DBParam();
        //    oip[0].ParameterName = "inObjKey";
        //    oip[0].DbType = DbType.Int64;
        //    oip[0].Value = 63911603;


        //    //connOra.SetCurrentSession(new DBConn.UserData("64c6ab28-2512-4544-9371-94504700ba6e", "127.0.0.1", "yu_homenko-PC", "cuvh3555brh01555gsntbfzo", "IE 8.0"));
        //    //connOra.ConnectionString("Data Source=10.10.20.228/lat4prod;Persist Security Info=True;User ID=gis_admin;Password=gis_admin;Unicode=True");

        //    return connOra.ExecuteQuery<string>("gis_meta.borehole_util.checkBorehole", oip);

        //}


        public static string TestSqlLiteBbConnection(string sqlLiteDbPath)
        {
            DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();

            DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
            conn.ConnectionString(sqlLiteDbPath);
            // DataTable dt = conn.ExecuteQuery<DataTable>("select CDICT_VALUE_NAME from DICT_VALUES where NDICT_VALUE_KEY = 8");
            DataTable dt = conn.ExecuteQuery<DataTable>("select a2 from aaa ");
            return dt.Rows[0][0].ToString();


        }
        #endregion
        /// <summary>
        /// возвращает все МГ
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllMG()
        {
            DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
            DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

            string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
            conn.ConnectionString(conStr);
            DataTable dt = new DataTable();
            dt = conn.ExecuteQuery<DataTable>("select * from MG ");
            return dt;
        }
        /// <summary>
        /// возвращает все нити
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllNit()
        {
            DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
            DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

            string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
            conn.ConnectionString(conStr);
            DataTable dt = new DataTable();
            dt = conn.ExecuteQuery<DataTable>("select * from Thread ");
            return dt;
        }

        /// <summary>
        /// возвращает все участки
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllRegion()
        {
            string query = "Select * From Regions_thread ";

            DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
            DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

            string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
            conn.ConnectionString(conStr);
            DataTable dt = new DataTable();
            dt = conn.ExecuteQuery<DataTable>(query);
            return dt;
        }
        /// <summary>
        /// возвращает по ключу газопровода все его нитки
        /// </summary>
        /// <param name="keyMg"></param>
        /// <returns></returns>
        /// 
        public static DataTable GetNitOnMg(string keyMg)
        {
            DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
            DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

            string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
            conn.ConnectionString(conStr);
            DataTable dt = new DataTable();
            dt = conn.ExecuteQuery<DataTable>("Select * From Thread t where t.Key_MG=" + keyMg);
            return dt;
        }

        /// <summary>
        /// по ключу нити возвращаются все участки
        /// </summary>
        /// <param name="keyNit"></param>
        /// <returns></returns>
        public static DataTable GetAllregionsOnNit(string keyNit)
        {
            string query = "Select t.nkey_region, t.Key_thread, t.cName, t.nkm_begin, ";
            query = query + "t.nkm_end, (t.nkm_end - t.nkm_begin) as lenghtRegion ";
            query = query + " From Regions_thread t where t.Key_thread=";
            DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
            DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

            string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
            conn.ConnectionString(conStr);
            DataTable dt = new DataTable();
            dt = conn.ExecuteQuery<DataTable>(query + keyNit);
            return dt;
        }

        /// <summary>
        /// по ключам актов - все трубы (трубы не привязанные к мг)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllPipeNoRegion()
        {
            //string query = "Select t.nkey_region, t.Key_thread, t.cName, t.nkm_begin, ";
            //query = query + "t.nkm_end, (t.nkm_end - t.nkm_begin) as lenghtRegion ";
            //query = query + " From Regions_thread t where t.Key_thread=";
            //DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
            //DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

            //string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
            //conn.ConnectionString(conStr);
            DataTable dt = new DataTable();
            //dt = conn.ExecuteQuery<DataTable>(query + keyNit);
            return dt;
        }

        /// <summary>
        /// возвращает все трубы участка
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static DataTable GetPipeOnRegion(string keyRegion)
        {
            DataTable dt = new DataTable();
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                string query = "Select (Select count(s.cNumberAkt)  From shurf_truba st1  , shurf s ";
                query = query + " where   st1.nKey_pipe = p.nKey_pipe  ";
                query = query + " and st1.nShurf_key = s.nShurf_key ) as cnt, ";
                query = query + "p.nKey_pipe, p.cNumber_pipe, round(p.cloc_km_beg, 3) as cloc_km_beg , p.nAngle,";
                query = query + "p.clength,  p.cdepth_pipe, p.cdiam,  ";
                query = query + "(Select count(dm.nDefect_key)  From defect_metalla dm ";
                query = query + "where dm.nKey_pipe = p.nKey_pipe  ) as nCountDefect, ";
                query = query + "(select max(dm.ndepth)  FROM defect_metalla dm ";
                query = query + "  WHERE dm.nKey_pipe = p.nKey_pipe ) as depthdef, ";
                query = query + "(select max((dm.nlengthdefect * dm.nwidth)/1000000)  FROM defect_metalla dm  ";
                query = query + " WHERE dm.nKey_pipe = p.nKey_pipe) as sqldef ";
                query = query + "  From Pipe p  where  p.nkey_region = ";
                //query = query + " Select p.nKey_pipe, p.cNumber_pipe, p.cloc_km_beg, p.nAngle,";
                //query = query + "p.сlength, p.cdepth_pipe, p.nCountDefect From Pipe p  where p.nkey_region = ";

                //dt = conn.ExecuteQuery<DataTable>(query + keyRegion + "  limit 100");
                dt = conn.ExecuteQuery<DataTable>(query + keyRegion);

                return dt;
            }
            catch (Exception)
            {
                return dt;
                throw;
            }




        }
        /// <summary>
        /// по ключу региона возвращает все акты на трубы
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static DataTable GetPipeOnRegionAllAct(string keyRegion)
        {
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = " select sf.cNumberAkt as cNumberAkt, sp.nKey_pipe  from shurf sf, shurf_truba sp, pipe p  ";
                query = query + " where sf.nShurf_key = sp.nShurf_key and sp.nKey_pipe = p.nKey_pipe  ";
                query = query + " and p.nkey_region = ";

                dt = conn.ExecuteQuery<DataTable>(query + keyRegion);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// по ключу участка возвращает все ключи актов на трубы c группировкой (для удаления - чистка участка)
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static DataTable GetPipeOnRegionAllActGroup(string keyRegion)
        {
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = " select sf.nShurf_key  from shurf sf, shurf_truba sp, pipe p  ";
                query = query + " where sf.nShurf_key = sp.nShurf_key and sp.nKey_pipe = p.nKey_pipe  ";
                query = query + " and p.nkey_region = ";

                dt = conn.ExecuteQuery<DataTable>(query + keyRegion + "    group by sf.nShurf_key ");
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// возвращает все ключи дефекты на трубах определенного участка (для удаления - чистка участка )
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static DataTable GetDefectPipeOnKeyRegion(string keyRegion)
        {
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = " Select dm.nDefect_key From Defect_metalla dm, pipe p  ";
                query = query + " where dm.nKey_pipe = p.nKey_pipe and p.nkey_region =  ";


                dt = conn.ExecuteQuery<DataTable>(query + keyRegion);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// выбрать все акты с непривязанными трубами (для дерева не привязанных труб)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllAktOnRegionIsNull()
        {
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = " SELECT   shr.cNumberAkt ,   shr.nShurf_key FROM   pipe p ,   shurf_truba sh ,  shurf shr  ";
                query = query + "WHERE   ifnull( p.nkey_region ,  -1 ) = -1 AND sh.nKey_pipe = p.nKey_pipe AND sh.nShurf_key = shr.nShurf_key  ";
                query = query + "GROUP BY   shr.cNumberAkt";

                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// по ключу акта получаем номера труб (для труб которые не привязаны - вне участков)
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <returns></returns>
        public static DataTable GetAllPipe_KeyAktOnRegionIsNull(string keyAkt)
        {
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = " Select p.cNumber_pipe, p.nKey_pipe From Pipe p, shurf_truba str   ";
                query = query + " where p.nKey_pipe = str.nKey_pipe and str.nShurf_key =  ";


                dt = conn.ExecuteQuery<DataTable>(query + keyAkt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// по ключам труб получаем ключи дефектов на трубах(если не привязан участок)
        /// </summary>
        /// <param name="keyPipeList"></param>
        /// <returns></returns>
        public static DataTable GetAllKeyDefectOnRegionIsNull(List<string> keyPipeList)
        {
            string _keyPipe = "";

            try
            {
                for (int i = 0; i < keyPipeList.Count; i++)
                {
                    _keyPipe = _keyPipe + keyPipeList[i] + ", ";
                }
                _keyPipe = _keyPipe.Substring(0, _keyPipe.Length - 2);

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = " Select dm.nDefect_key From Defect_metalla dm, pipe p  ";
                query = query + " where dm.nKey_pipe = p.nKey_pipe and p.nKey_pipe in (" + _keyPipe + ")";


                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }


        }

        /// <summary>
        ///1. чистим таблицы Calculation_defect
        ///2. Defect_metalla
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static string ClearBazaOnKeyDefect(List<string> keyDefect)
        {
            string result = "";

            string _keyDefect = "";
            for (int i = 0; i < keyDefect.Count; i++)
            {
                _keyDefect = _keyDefect + keyDefect[i] + ", ";
            }

            _keyDefect = _keyDefect.Substring(0, _keyDefect.Length - 2);
            //1. чистим таблицы Calculation_defect
            //2. Defect_metalla
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                string query = " delete From Calculation_defect where nDefect_key  in (" + _keyDefect + ")";
                int tt = conn.ExecuteNonQuery((query));

                query = "";
                query = " delete From  Defect_metalla where nDefect_key  in (" + _keyDefect + ")";
                int tt1 = conn.ExecuteNonQuery((query));

            }
            catch (Exception ee)
            {
                result = "Ошибка удаления дефекта и связей" + ee;

            }


            return result;
        }
        /// <summary>
        /// чистим таблицы
        /// 1.Shurf
        /// 2.Shurf_Truba
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <returns></returns>
        public static string ClearBazaOnkeyAkt(List<string> keyAkt)
        {
            string result = "";
            string _keyAkt = "";

            try
            {



                for (int i = 0; i < keyAkt.Count; i++)
                {
                    _keyAkt = _keyAkt + keyAkt[i] + ", ";
                }

                _keyAkt = _keyAkt.Substring(0, _keyAkt.Length - 2);
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                string query = " delete From Shurf_Truba where nShurf_key in (" + _keyAkt + ")";
                int tt = conn.ExecuteNonQuery((query));

                query = "";
                query = " delete From  Shurf where nShurf_key  in (" + _keyAkt + ")";
                int tt2 = conn.ExecuteNonQuery((query));


            }
            catch (Exception ee)
            {

                result = "Ошибка чистки базы акт  " + ee;
            }

            return result;
        }

        /// <summary>
        /// удаляем выбранные акты (+ удаление всех расчетов на дефекты)
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <returns></returns>
        public static string DeleteOnSelectkeyAkt(string keyAkt)
        {
            string result = "";

            try
            {
                string[] arr = keyAkt.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    string tt = arr[i].ToString();
                    //по ключу акта список всех дефектов
                    DataTable dt = new DataTable();
                    dt = GetDefectOnIas(arr[i]);
                    if (dt.Rows.Count > 0)
                    {
                        IEnumerable<DataRow> query =
                            from t in dt.AsEnumerable()
                            select t;

                        foreach (DataRow p in query)
                        {
                            string keyDefect = p.Field<object>("nDefect_key").ToString();
                            //по ключу дефекта удаляем его расчет
                            string deletecalc = DeleteCalcDefect(keyDefect);
                        }
                    }
                    DataTable dtPipe = new DataTable();
                    dtPipe = GetPipeOnkeyAkt(arr[i]);
                    if (dtPipe.Rows.Count > 0)
                    {
                        IEnumerable<DataRow> query1 =
                            from t1 in dtPipe.AsEnumerable()
                            select t1;

                        foreach (DataRow p1 in query1)
                        {
                            string keyPipe = p1.Field<object>("nKey_pipe").ToString();
                            //по ключу трубы удаляем группировки и колонии
                            string deleteGroup = DeleteColonyDefects(keyPipe);

                        }
                    }
                }
                //по ключу акта удаляем акты
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                string query2 = " delete From Shurf_Truba where nShurf_key in (" + keyAkt + ")";
                int tt2 = conn.ExecuteNonQuery((query2));

                query2 = "";
                query2 = " delete From  Shurf where nShurf_key  in (" + keyAkt + ")";
                int tt3 = conn.ExecuteNonQuery((query2));


            }
            catch (Exception ee)
            {

                result = "Ошибка удаления актов  " + ee;
            }

            return result;
        }
        /// <summary>
        /// по ключу акта получаем список труб привязанных к акту
        /// </summary>
        /// <param name="ckeyAkt"></param>
        /// <returns></returns>
        public static DataTable GetPipeOnkeyAkt(string ckeyAkt)
        {
            DataTable dt = new DataTable();

            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = " Select t.* From Shurf_Truba t where t.nShurf_key = " + ckeyAkt;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }



        /// <summary>
        /// чистим таблицы
        /// 1.Pipe
        /// 2.Regions_thread 
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static string ClearbazaPipeOnkeyRegion(string keyRegion)
        {
            string result = "";
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                string query = " delete From Pipe where nkey_region =" + keyRegion;
                int tt = conn.ExecuteNonQuery((query));

                query = "";
                query = " delete From Regions_thread where nkey_region =" + keyRegion;
                int tt2 = conn.ExecuteNonQuery((query));
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        /// <summary>
        /// Возвращает все дефекты на выбранных трубах
        /// </summary>
        /// <param name="selectPipeList"></param>
        /// <returns></returns>
        public static DataTable GetDefectPipe(IList<SelectPipeOnGrid> selectPipeList)
        {
            try
            {
                string selectpipe = ""; //если несколько труб, то выборку дефектов по всем трубам
                DataTable dt = new DataTable();
                //должна быть сводная таблица дефетов и их расчет, причем поле в расчетах crecom - пишет "допустимый" и т.д.
                // а уже на основании этого вставляем в соответсвующие ячейки картинку

                for (int i = 0; i < selectPipeList.Count; i++)
                {
                    selectpipe = selectpipe + selectPipeList[i].KeyPipe + ", ";
                }

                selectpipe = selectpipe.Substring(0, selectpipe.Length - 2);

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "";
                query = query + "Select  t.nKey_pipe, t.nDefect_key, t.nlengthDefect, t.nwidth, t.ndepth,  ";
                query = query + " t.cName,t.nclockwise_pos, round(t.nprevseam_dist,3) as nprevseam_dist , round(t.nloss_metall) as nloss_metall, t.isedited, ";
                query = query + "( SELECT dd.cname FROM Dict_Type_Defect dd WHERE dd.nTypeDefect_Key = t.nTypeDefect_Key ) AS defType ,   ";
                query = query +
                        "(select dd.cname from calculation_defect cd, dict_dip_defect dd where cd.nDict_Dip_Defect = dd.nDict_Dip_Defect_key and cd.nDict_type_calculation_key = 3 and cd.ndefect_key = t.nDefect_key) as rstreng, ";
                query = query +
                        "(select dd.cname from calculation_defect cd, dict_dip_defect dd where cd.nDict_Dip_Defect = dd.nDict_Dip_Defect_key and cd.nDict_type_calculation_key = 1 and cd.ndefect_key = t.nDefect_key) as dnv, ";
                query = query +
                        "(select dd.cname from calculation_defect cd, dict_dip_defect dd where cd.nDict_Dip_Defect = dd.nDict_Dip_Defect_key and cd.nDict_type_calculation_key = 2 and cd.ndefect_key = t.nDefect_key) as asme ";
                query = query + " From Defect_metalla t ";
                query = query + " where ifnull(t.isedited,-1) <>1 and t.nKey_pipe  in (" + selectpipe + ")";


                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// вывод количества дефектов и максимальная минимальная глубина дефектов
        /// </summary>
        /// <param name="selectPipeList"></param>
        /// <returns></returns>
        public static DataTable GetCountDefectDepth(IList<SelectPipeOnGrid> selectPipeList)
        {
            DataTable dt = new DataTable();
            try
            {
                string selectpipe = ""; //если несколько труб, то выборку дефектов по всем трубам



                for (int i = 0; i < selectPipeList.Count; i++)
                {
                    selectpipe = selectpipe + selectPipeList[i].KeyPipe + ", ";
                }

                selectpipe = selectpipe.Substring(0, selectpipe.Length - 2);

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "";
                query = query + " SELECT count (t.nDefect_key) as countdef , max(t.ndepth)as maxdepth,  min(t.ndepth)as mindepth  ";
                query = query + " From Defect_metalla t ";
                query = query + " where ifnull(t.isedited,-1) <>1 and t.nKey_pipe  in (" + selectpipe + ")";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// Возвращает все дефекты на конкретной трубе
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static DataTable GetDefectOnKeyPipe(string keyPipe)
        {
            try
            {

                DataTable dt = new DataTable();
                //должна быть сводная таблица дефетов и их расчет, причем поле в расчетах crecom - пишет "допустимый" и т.д.
                // а уже на основании этого вставляем в соответсвующие ячейки картинку

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "";

                //!!! с добавлением цвета!!!!!!!!!!

                query = query + "Select  t.nKey_pipe, t.nDefect_key, t.nlengthDefect, t.nwidth, t.ndepth, p.cdepth_pipe, ";
                query = query + " t.cName,t.nclockwise_pos, round(t.nprevseam_dist, 3) as nprevseam_dist , t.nloss_metall, t.isedited, ";
                query = query + "( SELECT dd.cname FROM Dict_Type_Defect dd WHERE dd.nTypeDefect_Key = t.nTypeDefect_Key ) AS defType ,   ";
                query = query +
                        "(select dd.cname from calculation_defect cd, dict_dip_defect dd where cd.nDict_Dip_Defect = dd.nDict_Dip_Defect_key and cd.nDict_type_calculation_key = 3 and cd.ndefect_key = t.nDefect_key) as rstreng, ";
                query = query +
                        "(select dd.cname from calculation_defect cd, dict_dip_defect dd where cd.nDict_Dip_Defect = dd.nDict_Dip_Defect_key and cd.nDict_type_calculation_key = 1 and cd.ndefect_key = t.nDefect_key) as dnv, ";
                query = query +
                        "(select dd.cname from calculation_defect cd, dict_dip_defect dd where cd.nDict_Dip_Defect = dd.nDict_Dip_Defect_key and cd.nDict_type_calculation_key = 2 and cd.ndefect_key = t.nDefect_key) as asme ";
                query = query + " From Defect_metalla t, pipe p   ";
                query = query + " where ifnull(t.isedited,-1) <>1 and t.nKey_pipe = p.nKey_pipe and t.nKey_pipe = " + keyPipe;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// удаление дефекта по ключу
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static string DeleteDefect(string keyDefect)
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "";
                query = query + " update  Defect_metalla set isEdited = 1  where  nDefect_key=  " + keyDefect;
                int result = conn.ExecuteNonQuery((query));
                string result2 = "Дефект удален";
                return result2;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// дефект подтвержден без редактирования
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static string ConfirmDefect(string keyDefect)
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "";
                query = query + " update  Defect_metalla set isEdited = 4  where  nDefect_key=  " + keyDefect;
                int result = conn.ExecuteNonQuery((query));
                string result2 = keyDefect;
                return result2;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// по ключу дефекта возвращает его расчет
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static DataTable GetCalculateOneDefect(string keyDefect, string keyCalcDefect)
        {
            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "";
                query = query + " SELECT cd.*, ddf.cName FROM Calculation_defect cd, dict_dip_defect ddf ";
                query = query + " WHERE cd.nDict_type_calculation_key = " + keyCalcDefect;
                query = query + " AND cd.nDefect_key =" + keyDefect;
                query = query + " and cd.nDict_Dip_Defect = ddf.nDict_Dip_Defect_key";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// удаляем расчеты по дефектам
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <param name="keyTypeCalc"></param>
        /// <returns></returns>
        public static string DeleteCalcDefect(string keyDefect)
        {
            string result = "";
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                //сначало удалим если есть расчет по ключу дефекта и ключу типа дефекта
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "delete From Calculation_defect where nDefect_key = " + keyDefect;
                int result1 = conn.ExecuteNonQuery((query));
            }
            catch (Exception ee)
            {

                result = ee.ToString();

            }
            return result;
        }
        /// <summary>
        /// сохранение расчета единичного дефекта по ASME
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <param name="keyTypeCalc"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string SaveCalcdefectOnASME(string keyDefect, string keyTypeCalc, Dictionary<string, string> d)
        {
            string keyCalc = "";

            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                //сначало удалим если есть расчет по ключу дефекта и ключу типа дефекта
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "delete From Calculation_defect where nDefect_key = " + keyDefect + " and ndict_type_calculation_key = " + keyTypeCalc;
                int result = conn.ExecuteNonQuery((query));

                string fields = string.Empty;
                string values = string.Empty;

                var ar = d.ToArray();
                for (int i = 0; i < ar.Length; i++)
                {
                    fields += ar[i].Key + ", ";
                    values += ar[i].Value + ", ";
                }

                fields += "nDefect_key" + ", ";
                values += keyDefect + ", ";

                fields += "nDict_type_calculation_key";
                values += keyTypeCalc;

                query = "";
                query += "insert into Calculation_defect (" + fields + ")" + " values (" + values + ")";
                int dt3 = conn.ExecuteNonQuery((query));
                return keyCalc;
            }
            catch (Exception ee)
            {

                keyCalc = ee.ToString();
                return keyCalc;
            }
        }

        /// <summary>
        /// удаляем все колонии и группы на заданной трубе
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static string DeleteColonyDefects(string keyPipe)
        {
            string result = "";
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                //сначало удалим если есть расчет по ключу дефекта и ключу типа дефекта
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                //вначале удаляем все созданные колонии по ключю трубы
                string query = "delete From DefColony where nKey_pipe = " + keyPipe;
                int result1 = conn.ExecuteNonQuery((query));

                query = "delete From DefGroup where nKey_pipe = " + keyPipe;
                int result2 = conn.ExecuteNonQuery((query));
                return result;

                return result;
            }
            catch (Exception ee)
            {
                return ee.ToString();
                throw;
            }

        }
        /// <summary>
        /// возвращает количество расчитываемых дефектов(если акт загружен из базы иаса)
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static string GetountCalcDrfrct(string keyDefect)
        {
            string countKeyDefectCalc = "";

            DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
            DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
            string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
            conn.ConnectionString(conStr);

            string query = "";

            query += " select count(nDefect_key) from Calculation_defect where nDefect_key in (" + keyDefect+")";

            countKeyDefectCalc = conn.ExecuteQuery<string>(query);

            return countKeyDefectCalc;
        }

        /// <summary>
        /// сохраняем координаты колонии дефектов(расчет)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static string SaveColonyDefects(Dictionary<string, string> d, string keyPipe)
        {
            string keyColony = "";
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                string fields = string.Empty;
                string values = string.Empty;

                var ar = d.ToArray();
                for (int i = 0; i < ar.Length; i++)
                {
                    fields += ar[i].Key + ", ";
                    values += ar[i].Value + ", ";
                }

                fields = fields.Substring(0, fields.Length - 2);
                values = values.Substring(0, values.Length - 2);

                string query = "";
                query += "insert into DefColony (" + fields + ")" + " values (" + values + ")";
                int dt3 = conn.ExecuteNonQuery((query));
                return keyColony;
            }
            catch (Exception ee)
            {
                return ee.ToString();
                throw;
            }

        }
        /// <summary>
        /// сохраняем координаты групп дефектов(расчет)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static string SaveGroupDefects(Dictionary<string, string> d, string keyPipe)
        {
            string keyColony = "";
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                string fields = string.Empty;
                string values = string.Empty;

                var ar = d.ToArray();
                for (int i = 0; i < ar.Length; i++)
                {
                    fields += ar[i].Key + ", ";
                    values += ar[i].Value + ", ";
                }

                fields = fields.Substring(0, fields.Length - 2);
                values = values.Substring(0, values.Length - 2);

                string query = "";
                query += "insert into DefGroup (" + fields + ")" + " values (" + values + ")";
                int dt3 = conn.ExecuteNonQuery((query));
                return keyColony;
            }
            catch (Exception ee)
            {
                return ee.ToString();
                throw;
            }
        }

        /// <summary>
        /// по ключу трубы возвращает все группы дефектов
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static DataTable GetGroupDefectsOnkeyPipe(string keyPipe)
        {
            DataTable dt = new DataTable();
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "Select dg.* From DefGroup dg  where dg.nKey_pipe =  " + keyPipe;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {
                return dt;
                throw;
            }

        }
        /// <summary>
        /// по ключу трубы получаем колонии дефектов
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static DataTable GetColonyDefectsOnkey(string keyPipe)
        {
            DataTable dt = new DataTable();
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "Select dg.* From DefColony dg  where dg.nKey_pipe =  " + keyPipe;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {
                return dt;
                throw;
            }
        }
        /// <summary>
        /// по ключу дефекта возвращает паспорт дефекта
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static DataTable GetPassportDefect(string keyDefect)
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select dm.*, p.cdepth_pipe From Defect_metalla dm, pipe p  where dm.nKey_pipe = p.nKey_pipe and dm.nDefect_key = " + keyDefect;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь типа дефекта
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictTypeDefect()
        {
            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_Type_Defect ";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// словарь расчетный тип
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictCalcType()
        {
            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_Calc_Type ";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь расположение в толщине металла
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictDepthPos()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_Depth_pos ";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь метод устранения дефекта
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictMetodUstr()
        {
            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_Metod_ustr_def ";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь дефект устранен
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictDefectUstr()
        {
            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_defect_ustr ";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static string SavePasportDefectModel(string keyDefect, Dictionary<string, string> d, string keyPipe, string isedited)
        {
            string query = "";
            string keyseq = "";

            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();


                if (keyDefect == "0")
                {
                    //если новый то создаем новый ключ
                    //увеличиваем на единицу и обновляем полученное значение в таблице
                    query = "update internalseq set key_seq = (key_seq+1)";
                    int dt2 = conn.ExecuteNonQuery((query));
                    //получаем ключ новый 
                    query = "Select key_seq from internalseq";
                    dt = conn.ExecuteQuery<DataTable>(query);
                    int keyseq1 = Convert.ToInt32(dt.Rows[0][0].ToString());
                    keyseq1 = keyseq1 * 100;
                    keyseq = keyseq1.ToString();
                    query = "";

                    string fields = string.Empty;
                    string values = string.Empty;

                    var ar = d.ToArray();
                    for (int i = 0; i < ar.Length; i++)
                    {
                        fields += ar[i].Key + ", ";
                        values += ar[i].Value + ", ";
                    }

                    fields += "nKey_pipe" + ", ";
                    values += keyPipe + ", ";

                    fields += "isEdited" + ", ";
                    values += isedited + ", ";

                    fields += "nDefect_key ";
                    values += keyseq;




                    query += "insert into Defect_metalla (" + fields + ")" + " values (" + values + ")";
                    int dt3 = conn.ExecuteNonQuery((query));

                }
                else
                {
                    keyseq = keyDefect;
                    string strResult = " ";
                    query = "";
                    var ar = d.ToArray();
                    for (int i = 0; i < ar.Length; i++)
                    {
                        strResult = strResult + " " + ar[i].Key + "=" + ar[i].Value + ", ";

                    }
                    strResult = strResult + " isEdited = " + isedited;
                    //strResult = strResult.Substring(0, strResult.Length - 2);
                    query += " update Defect_metalla set " + strResult + "  where nDefect_key = " + keyseq;
                    int dt3 = conn.ExecuteNonQuery((query));

                }
                return keyseq;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Возвращает ключ сохраннного акта
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <param name="selectPipeList"></param>
        /// <returns></returns>
        public static string SaveAct(Dictionary<string, string> d, IList<SelectPipeOnGrid> selectPipeList)
        {
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                int keyseq;
                int keyseq2;
                string query = "";
                string query2 = "";
                DataTable dt = new DataTable();
                //нужно создать новый ключ для инсерта акта
                ////получаем текущий ключ
                //query2 = "Select key_seq from internalseq";
                //dt = conn.ExecuteQuery<DataTable>(query);
                //keyseq2 = Convert.Todouble?(dt.Rows[0][0].ToString());
                //увеличиваем на единицу и обновляем полученное значение в таблице
                query = "update internalseq set key_seq = (key_seq+1)";
                int dt2 = conn.ExecuteNonQuery((query));
                //получаем ключ новый 
                query = "Select key_seq from internalseq";
                dt = conn.ExecuteQuery<DataTable>(query);
                keyseq = Convert.ToInt32(dt.Rows[0][0].ToString());
                keyseq = keyseq * 100;

                string fields = string.Empty;
                string values = string.Empty;

                var ar = d.ToArray();
                for (int i = 0; i < ar.Length; i++)
                {
                    fields += ar[i].Key + ", ";
                    values += ar[i].Value + ", ";
                }

                fields += "nShurf_key";
                values += keyseq.ToString();

                query = "";
                query += "insert into Shurf (" + fields + ")" + " values (" + values + ")";
                int dt3 = conn.ExecuteNonQuery((query));



                //делаем инсерт с уже созданным новым ключем
                //query =
                //    "insert into Shurf (nShurf_key, cNumberAkt, dDateShurf, cdefectoskopist, cagentLPU)";
                //query = query + "  values (" + keyseq.ToString() + "," + "'" + numberAkt + "'" + "," + "'" + cDataShurf + "'" + "," + defectoskop + "," + agentLpu + ")";


                //записываем ключ нового акта в таблицу связи с трубой 
                query = "";
                //если несколько труб и один шурф
                for (int i = 0; i < selectPipeList.Count; i++)
                {
                    query = "insert into Shurf_Truba (nKey_pipe, nShurf_key) values (" + selectPipeList[i].KeyPipe + "," + keyseq.ToString() + ")";
                    int dt4 = conn.ExecuteNonQuery((query));
                }
                //query = "insert into Shurf_Truba (nKey_pipe, nShurf_key) values ("+selectPipeList[0].KeyPipe +","+keyseq.ToString()+ ")";
                //int dt4 = conn.ExecuteNonQuery((query));
                return keyseq.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        
        /// <summary>
        /// подпись акта
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <returns></returns>
        public static string SetSignatureAkt(string keyAkt, string isedit)
        {
            string query = "";
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                query += " update Shurf set isEdited = " + isedit + "  where nShurf_key = " + keyAkt;
                int dt3 = conn.ExecuteNonQuery((query));
                return keyAkt;

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// сохранение всего акта (документальное представление)
        /// </summary>
        /// <param name="keyAct"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string SaveAllAct(string keyAct, Dictionary<string, string> d)
        {
            string query = "";
            string keyseq = "";

            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);


                keyseq = keyAct;
                string strResult = " ";
                query = "";
                var ar = d.ToArray();
                for (int i = 0; i < ar.Length; i++)
                {
                    strResult = strResult + " " + ar[i].Key + "=" + ar[i].Value + ", ";

                }
                strResult = strResult.Substring(0, strResult.Length - 2);
                query += " update Shurf set " + strResult + "  where nShurf_key = " + keyseq;
                int dt3 = conn.ExecuteNonQuery((query));
                return keyAct;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string SaveNewPipeUpdate(string keyPipe, Dictionary<string, string> d)
        {
            string query = "";
            string keyseq = "";

            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();

                keyseq = keyPipe;
                string strResult = " ";
                query = "";
                var ar = d.ToArray();
                for (int i = 0; i < ar.Length; i++)
                {


                    strResult = strResult + " " + ar[i].Key + "=" + (ar[i].Value == null ? "null" : ar[i].Value) + ", ";

                }
                strResult = strResult.Substring(0, strResult.Length - 2);
                query += " update Pipe set " + strResult + "  where nKey_pipe = " + keyseq;
                int dt3 = conn.ExecuteNonQuery((query));
                return keyPipe;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// сохранение данных по трубе
        /// </summary>
        /// <param name="cpm"></param>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static string SaveCharacteristicPipe(CharacteristicPipeModel cpm, string keyPipe)
        {
            string diam = cpm.Diam.ToString();
            return diam;
        }
        /// <summary>
        /// словарь категория участка
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictKetegor_uch()
        {
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_kategor_uch";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// словарь температурный коэффициент
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictTemperaturn_koef()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_temperaturn_koef";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь завод изготовитель
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictFactory_builder()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Factory_builder ";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь марка стали
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictFeelGrade()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_feel_grade ";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// таблица соответствия марки стали и завода изготовителя
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEcvivalent_marka_builder()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "select t.*, (SELECT tt.cName FROM dict_feel_grade tt WHERE tt.nDict_feel_grade_key = t.nDict_feel_grade_key ) AS cname FROM Ecvivalent_marka_builder t ";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// характеристики трубы
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static DataTable GetCharacteristicPipe(string keyPipe, string keyShurf)
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * ";
                //query += " (select t.ndict_temperaturn_koef_key from shurf t where t.nShurf_key =" + keyShurf + ") as temperatkoef ";
                query += "  From Pipe where nKey_pipe = " + keyPipe;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// словарь характеристика грунта
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictCharacter_grunt()
        {
            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_Character_grunt";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь тип изоляции
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictType_izol()
        {
            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_type_izol";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь состояние защитного покрытия
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictIzol_state()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_Izol_state";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь наличие нахлеста
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictAvailabl_overt()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_availabi_overl";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь прилепаемость к трубе
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictAdherenc_pipe()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_adherenc_pipe";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// словарь наличие влаги под изоляцией
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictAvailabl_dump()
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Dict_availabil_damp";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable GetDocumentPresecationShurf(string keyShurf)
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select * From Shurf where nShurf_key = " + keyShurf;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// возвращает список актов на выбранных трубах
        /// </summary>
        /// <param name="selectPipeList"></param>
        /// <returns></returns>
        public static DataTable GetListAct()
        {
            //GetListAct(string SelectKeyRegion)
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "";
                query = query + "Select ss.IsEdited, st.nShurf_key, ss.cNumberAkt, ss.dDateShurf, p.cNumber_pipe, round(p.cloc_km_beg, 5) as cloc_km_beg , st.nkey_pipe , ";
                query = query + " p.nAngle, p.nkey_region, (Select count(st1.nKey_pipe)  From Shurf_Truba st1 where st1.nShurf_key = ss.nShurf_key ) as nCountPipe ";
                query = query + "  From Pipe p, Shurf_Truba st, shurf ss, regions_thread rth";
                query = query + "  LEFT OUTER join regions_thread on   p.nkey_region  =  regions_thread.nkey_region ";
                query = query + "  where  ifnull(p.nkey_region,-1) and p.nKey_pipe = st.nKey_pipe and ss.nShurf_key = st.nShurf_key ";
                //query = query + "  and p.nkey_region = rth.nkey_region and  rth.nkey_region = " + SelectKeyRegion;
                //query = query + "  and p.nkey_region = rth.nkey_region " ;
                query = query + "  group by st.nShurf_key";
                dt = conn.ExecuteQuery<DataTable>(query);

                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// возвращает список всех номеров труб, углов, километражей на которых есть определенный акт
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <returns></returns>
        public static DataTable GetNumberPipeOnAkt(string keyAkt)
        {
            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "Select st.*, round(p.cloc_km_beg, 3) as cloc_km_beg, p.nangle, p.cNumber_pipe ";
                query = query + "from shurf_truba st, pipe p ";
                query = query + " where st.nShurf_key = " + keyAkt + " and p.nKey_pipe=st.nKey_pipe ";

                dt = conn.ExecuteQuery<DataTable>(query);

                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// список всех участков
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllRegions()
        {
            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();
                string query = "";
                query = query + "Select rt.nkey_region, rt.cname from Regions_thread rt";
                dt = conn.ExecuteQuery<DataTable>(query);

                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// по ключу акта возвращаем список труб
        /// </summary>
        /// <param name="keyAct"></param>
        /// <returns></returns>
        public static DataTable GetListPipeOnkeyAct(string keyAct)
        {
            try
            {

                DataTable dt = new DataTable();
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "";
                query = query + "Select p.nKey_pipe, round(p.cloc_km_beg, 5) as cloc_km_beg , p.cNumber_pipe,";
                query = query + " p.nAngle, p.clength, p.cdepth_pipe, p.cdiam, ";
                query = query + "(Select count(dm.nDefect_key)  From defect_metalla dm ";
                query = query + "where dm.nKey_pipe = p.nKey_pipe  ) as nCountDefect ";
                query = query + "  From Pipe p, Shurf_Truba st ";
                query = query + " where p.nKey_pipe = st.nKey_pipe and st.nShurf_key =" + keyAct;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable GetPipeOnkey(string keyPipe)
        {
            try
            {


                DataTable dt = new DataTable();
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "";
                query = query + "Select p.nKey_pipe, round(p.cloc_km_beg, 5) as cloc_km_beg, p.cNumber_pipe,";
                query = query + " p.nAngle, p.clength, p.cdepth_pipe, p.cdiam ";
                query = query + "  From Pipe p ";
                query = query + " where p.nKey_pipe = " + keyPipe;
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// создание новой трубы
        /// </summary>
        /// <returns></returns>
        public static string CreateNewPipe()
        {
            string newKeyPipe = "";
            string query = "";
            string keyseq = "";

            try
            {

                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                DataTable dt = new DataTable();


                //если новый то создаем новый ключ
                //увеличиваем на единицу и обновляем полученное значение в таблице
                query = "update internalseq set key_seq = (key_seq+1)";
                int dt2 = conn.ExecuteNonQuery((query));
                //получаем ключ новый 
                query = "Select key_seq from internalseq";
                dt = conn.ExecuteQuery<DataTable>(query);
                int keyseq1 = Convert.ToInt32(dt.Rows[0][0].ToString());
                keyseq1 = keyseq1 * 100;
                keyseq = keyseq1.ToString();
                query = "";

                query += "insert into Pipe (nKey_pipe, clength, cdiam)" + " values (" + keyseq + ", " + " 11, " + " 1420 )";
                int dt3 = conn.ExecuteNonQuery((query));
                return keyseq;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// возврат всех новых актов для выгрузки в иас
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAktOnIas(string selectAkt)
        {
            try
            {

                DataTable dt = new DataTable();
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "Select * FROM Shurf WHERE ( nShurf_key % 100 ) = 0 and nShurf_key  in (" + selectAkt + ")";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// выгружаем все шурф-труба для выгрузки в иас
        /// </summary>
        /// <param name="onKeyShurf"></param>
        /// <returns></returns>
        public static DataTable GetShurf_Truba(string selectAkt)
        {
            try
            {

                DataTable dt = new DataTable();
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "Select * FROM Shurf_Truba WHERE ( nShurf_key % 100 ) = 0 and nShurf_key  in (" + selectAkt + ")";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// возврат всех дефектов попавших в шурф для выгрузки в иас
        /// </summary>
        /// <param name="onKeyShurf"></param>
        /// <returns></returns>
        public static DataTable GetDefectOnIas(string onKeyShurf)
        {
            //IsEdited
            //1 - дефект удален
            //2 - редактирование
            //5 - новый

            try
            {

                DataTable dt = new DataTable();
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "Select * FROM Defect_metalla  WHERE nKey_pipe IN (SELECT nKey_pipe FROM Shurf_Truba WHERE nShurf_key = " + onKeyShurf + ") and isEdited IN (1,2,5)";
                dt = conn.ExecuteQuery<DataTable>(query);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// очистка базы данных
        /// </summary>
        /// <returns></returns>
        public static string ClearBasa()
        {

            string result = "";
            string query = "DELETE FROM ";
            try
            {
                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                conn.ExecuteNonQuery(query + " MG");
                conn.ExecuteNonQuery(query + " THREAD");
                conn.ExecuteNonQuery(query + " REGIONS_THREAD");
                conn.ExecuteNonQuery(query + " PIPE");
                conn.ExecuteNonQuery(query + " Defect_metalla");
                conn.ExecuteNonQuery(query + " Shurf");
                conn.ExecuteNonQuery(query + " Shurf_Truba");
                conn.ExecuteNonQuery(query + " Calculation_defect");
                conn.ExecuteNonQuery(query + " DefGroup");
                conn.ExecuteNonQuery(query + " DefColony");

                //чистим словари
                conn.ExecuteNonQuery(query + " Dict_Type_Defect");
                conn.ExecuteNonQuery(query + " Dict_Calc_Type");
                conn.ExecuteNonQuery(query + " Dict_Depth_pos");
                conn.ExecuteNonQuery(query + " Dict_Metod_ustr_def");
                conn.ExecuteNonQuery(query + " Dict_Character_grunt");
                conn.ExecuteNonQuery(query + " Dict_type_izol");
                conn.ExecuteNonQuery(query + " Dict_Izol_state");
                conn.ExecuteNonQuery(query + " Dict_availabil_damp");
                conn.ExecuteNonQuery(query + " Factory_builder");
                conn.ExecuteNonQuery(query + " Dict_feel_grade");
                conn.ExecuteNonQuery(query + " Ecvivalent_marka_builder");

                StreamWriter writer = new System.IO.StreamWriter(@"Query.txt", false);
                writer.Write("");
                writer.Close();

            }
            catch (Exception exception)
            {

                result = "Ошибка очистки базы";
            }

            return result;
        }

        public static string AddTable(string nameTable, Dictionary<string, string> d)
        {
            string result = "";
            string query = "";

            try
            {
                string fields = string.Empty;
                string values = string.Empty;

                var ar = d.ToArray();
                for (int i = 0; i < ar.Length; i++)
                {
                    fields += ar[i].Key + ", ";
                    values += ar[i].Value + ", ";
                }

                fields = fields.Substring(0, fields.Length - 2);
                values = values.Substring(0, values.Length - 2);


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);

                query += "insert into  " + nameTable + "  (" + fields + ")" + " values (" + values + ")";
                StreamWriter writer = new System.IO.StreamWriter(@"Query.txt", true);
                writer.WriteLine(nameTable + ": ");
                writer.WriteLine(query);
                writer.Close();
                conn.ExecuteNonQuery((query));

            }
            catch (Exception ee)
            {

                result = "Ошибка insert в таблицу " + nameTable + ", " + ee.Message;
                StreamWriter writer = new System.IO.StreamWriter(@"Query.txt", true);
                writer.WriteLine(result);
                writer.Close();
            }

            return result;
        }

        public static int GetVisibleReport()
        {
            int visibleReport = 0;

            try
            {


                DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
                DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");
                string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
                conn.ConnectionString(conStr);
                string query = "Select visiblereport From VisibLeReport";
                visibleReport = conn.ExecuteQuery<int>(query);

                return visibleReport;
            }
            catch (Exception ee)
            {

                throw;
            }
        }


    }


}

//#
// #region вызов без dbcon
// вызов без dbcon   //DataTable dt = ExecuteSQLQuery(query);
//public static DataTable ExecuteSQLQuery(string strSQL)
//{
//    DataTable dt = new DataTable();
//    try
//    {
//        SQLiteConnection connect = Connect(ConfigurationManager.AppSettings.Get("SqlLiteDataBaseName"));
//        SQLiteCommand check = connect.CreateCommand();
//        check.CommandText = strSQL;
//        SQLiteDataReader reader = check.ExecuteReader();
//        dt.Load(reader);

//        Disconnect(connect);
//    }
//    catch (Exception exception)
//    {
//        MessageBox.Show("Ошибка: " + exception.Message, "Ошибка на странице");
//    }
//    return dt;
//}

//private static SQLiteConnection Connect(string strNameDB)
//{
//    SQLiteConnection connection = new SQLiteConnection();
//    try
//    {
//        SQLiteConnectionStringBuilder cs = new SQLiteConnectionStringBuilder();
//        cs.DataSource = strNameDB; // Field in class, initializes in constructor
//        connection.ConnectionString = cs.ToString();
//        connection.Open();
//    }
//    catch (Exception exception)
//    {
//        MessageBox.Show("Ошибка: " + exception.Message, "Ошибка на странице");
//    }
//    return connection;
//}
// #endregion вызов без dbcon


//query = query + "select nKey_pipe, Defect_metalla.nDefect_key ,  nLengthDefect, nwidth, ndepth, Defect_metalla.cname,";
//query = query + " nclockwise_pos, nprevseam_dist, nloss_metall, Dict_Type_Defect.cName as defType, ";
//query = query +
//        " Calculation_defect.nDict_Dip_Defect_keyRstreng, Calculation_defect.nDict_Dip_Defect_keyASME, ";
//query = query + " Calculation_defect.nDict_Dip_Defect_keyDNV from Defect_metalla";
//query = query +
//        " LEFT OUTER join Dict_Type_Defect on Defect_metalla.nTypeDefect_Key= Dict_Type_Defect.nTypeDefect_Key";
//query = query +
//        " LEFT OUTER join Calculation_defect on Defect_metalla.nDefect_key=Calculation_defect.nDefect_key";
//query = query + "  where Defect_metalla.nKey_pipe in (" + selectpipe+ ")";//" + selectPipeList[0].KeyPipe;


//query = query + " Select  t.nKey_pipe, t.nDefect_key, t.nlengthDefect, t.nwidth, t.ndepth, ";
//query = query + " t.cName,t.nclockwise_pos, t.nprevseam_dist, t.nloss_metall, ";
//query = query + " dd.cName as defType ";
//query = query + " From Defect_metalla t, Dict_Type_Defect dd  ";
//query = query + " where t.nTypeDefect_Key = dd.nTypeDefect_Key and t.nKey_pipe = " + keyPipe;

//query = query + " Select  t.nKey_pipe, t.nDefect_key, t.nlengthDefect, t.nwidth, t.ndepth, ";
//query = query + " t.cName,t.nclockwise_pos, t.nprevseam_dist, t.nloss_metall, ";
//query = query + " dd.cName as defType ";
//query = query + " From Defect_metalla t, Dict_Type_Defect dd  ";
//query = query + " where t.nTypeDefect_Key = dd.nTypeDefect_Key and t.nKey_pipe in (" + selectpipe+ ")";


///// <summary>
///// сохранение паспорта дефекта
///// </summary>
///// <param name="pdm"></param>
///// <returns></returns>
//public static string SavePasportDefectModel(PasportDefectModel pdm, string keyPipe)
//{
//    string keyDefect = pdm.KeyDefect;
//    string query = "";
//    string keyseq="";

//    DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
//    DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

//    string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
//    conn.ConnectionString(conStr);
//    DataTable dt = new DataTable();


//    if (keyDefect =="new")
//    {
//       //если новый то создаем новый ключ
//        //увеличиваем на единицу и обновляем полученное значение в таблице
//        query = "update internalseq set key_seq = (key_seq+1)";
//        int dt2 = conn.ExecuteNonQuery((query));
//        //получаем ключ новый 
//        query = "Select key_seq from internalseq";
//        dt = conn.ExecuteQuery<DataTable>(query);
//        int keyseq1 = Convert.ToInt32(dt.Rows[0][0].ToString());
//        keyseq1 = keyseq1 * 100;
//        keyseq = keyseq1.ToString();
//        query = "";
//        query = query + "insert into Defect_metalla t (t.nDefect_key, t.cName, t.ntypedefect_key,";
//        query = query + " t.nDict_Calc_Type_Key, t.nlengthDefect, t.nwidth, t.ndepth, t.nloss_metall,";
//        query = query + " t.nprevseam_dist, t.nnext_dist, t.nprevmark_dist, t.nnextmark_dist, ";
//        query = query + " t.nclockwise_pos, t.ndepth_pos_Key, t.cstress_descr, t.nMetod_Ustr_Def_Key, nKey_pipe)";
//        query = query + "  values (" + keyseq + ", " +"'"+ pdm.DefectName +"'"+ ", " + pdm.SelectedTypeDefect.Key + ", ";
//        query = query + pdm.SelectedCalcType.Key + ", " + pdm.W + ", " + pdm.H + ", " + pdm.Depth + ", " + pdm.LossMetal + ", ";
//        query = query + pdm.PrevSeam_Dist + ", " + pdm.Next_Dist + ", " + pdm.PrevMark_Dist + ", " + pdm.NextReper_Dist + ", ";
//        query = query + pdm.Angle + ", " + pdm.SelectedDepthPos.Key + ", " + pdm.Stress_Koor + ", " + pdm.SelectedMetodUstr.Key + keyPipe+ ")";
//        //int dt3 = conn.ExecuteNonQuery((query));

//    }
//    else
//    {
//        keyseq = keyDefect;
//    }
//    return keyDefect;
//}



///// <summary>
///// возвращает словарь дефектоскопист
///// </summary>
///// <returns></returns>
//public static DataTable GetDictDefectoskopist()
//{
//    DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
//    DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

//    string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
//    conn.ConnectionString(conStr);
//    DataTable dt = new DataTable();
//    string query = " Select * From Dict_defectoskopist ";
//    dt = conn.ExecuteQuery<DataTable>(query);

//    return dt;
//}
///// <summary>
///// возвращает сдлварь - представитель ЛПУ - не используется
///// </summary>
///// <returns></returns>
//public static DataTable GetDictAgentLPU()
//{
//    DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
//    DBConn.Conn conn = databaseFactory.CreateObject("SQLiteConn");

//    string conStr = ConfigurationManager.AppSettings["SqlLiteDataBaseName"].ToString();
//    conn.ConnectionString(conStr);
//    DataTable dt = new DataTable();
//    string query = " Select * From Dict_agent_LPU ";
//    dt = conn.ExecuteQuery<DataTable>(query);
//    return dt;
//}
