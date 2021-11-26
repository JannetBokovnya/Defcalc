using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using DEFCALC.DataModel;
using DrawPipe.DataModel;

namespace DEFCALC
{
    public class CBLL
    {
        //#region Тест
        ///// <summary>
        ///// Тестовая фцнкция подключения к Oracle БД.
        ///// </summary>
        ///// <returns>Возвращаемое значение</returns>
        //public static string TestOracleBdConnection()
        //{
        //    return ASBLL.TestOracleBbConnection();
        //}
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
        public static double GetLossMetal(object valueDephDef, object valueDephPipe)
        {
            try
            {
                double dephDef = double.Parse(valueDephDef.ToString());
                double dephPipe = double.Parse(valueDephPipe.ToString());

                return Math.Round((dephDef / dephPipe), 3);
            }
            catch (Exception)
            {
                return 0d;
            }

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

        //public static string GetValueRowDataTime(object valueRow)
        //{
        //    if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
        //    {
        //        string result;

        //        if (DateTime.TryParse(valueRow.ToString(), out result))
        //            return result;
        //    }
        //    return ;
        //}


        public static string TestSqlLiteBbConnection(string sqlLiteDbPath)
        {
            return ASBLL.TestSqlLiteBbConnection(sqlLiteDbPath);
        }

        /// <summary>
        /// возвращает все МГ
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllMG()
        {

            return ASBLL.GetAllMG();
        }

        /// <summary>
        /// возвращает все нити
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllNit()
        {
            return ASBLL.GetAllNit();

        }

        /// <summary>
        /// возвращает все участки
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllRegion()
        {
            return ASBLL.GetAllRegion();
        }

        /// <summary>
        /// возвращает по ключу газопровода все его нитки
        /// </summary>
        /// <param name="keyMg"></param>
        /// <returns></returns>
        public static DataTable GetNitOnMg(string keyMg)
        {
            return ASBLL.GetNitOnMg(keyMg);
        }

        /// <summary>
        /// по ключу нити возвращаются все участки
        /// </summary>
        /// <param name="keyNit"></param>
        /// <returns></returns>
        public static DataTable GetAllregionsOnNit(string keyNit)
        {
            return ASBLL.GetAllregionsOnNit(keyNit);
        }

        /// <summary>
        /// по ключам актов - все трубы (трубы не привязанные к мг)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllPipeNoRegion()
        {
            return ASBLL.GetAllPipeNoRegion();
        }


        /// <summary>
        /// возвращает все трубы участка
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static DataTable GetPipeOnRegion(string keyRegion)
        {
            return ASBLL.GetPipeOnRegion(keyRegion);
        }
        /// <summary>
        /// по ключу участка возвращает все акты на трубы
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static DataTable GetPipeOnRegionAllAct(string keyRegion)
        {
            return ASBLL.GetPipeOnRegionAllAct(keyRegion);
        }

        /// <summary>
        /// очищается база по конкретному участку
        /// </summary>
        /// <param name="keyregion"></param>
        /// <returns></returns>
        public static string ClearBazaOnRegion(string keyregion)
        {
            string report = "Участок успешно удален ";

            //список всех ключей труб на участке
            List<string> keyPipeOnKeyRegionList = new List<string>();
            //список всех ключей актов на участке
            List<string> keyAktOnkeyRegion = new List<string>();
            //список ключей всех дефектов труб на участке
            List<string> keyDefectOnkeyRegion = new List<string>();

            //

            try
            {

                DataTable dt = ASBLL.GetPipeOnRegion(keyregion);
                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in dt.AsEnumerable()
                        select t;
                    foreach (DataRow p in query)
                    {
                        keyPipeOnKeyRegionList.Add(
                            (p.Field<object>("nKey_pipe") != null ? p.Field<object>("nKey_pipe").ToString() : ""));
                    }
                }



                DataTable dtListAct = ASBLL.GetPipeOnRegionAllActGroup(keyregion);
                if (dtListAct.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in dtListAct.AsEnumerable()
                        select t;
                    foreach (DataRow p1 in query)
                    {
                        keyAktOnkeyRegion.Add(
                            (p1.Field<object>("nShurf_key") != null ? p1.Field<object>("nShurf_key").ToString() : ""));
                    }
                }

                DataTable dtListDefect = ASBLL.GetDefectPipeOnKeyRegion(keyregion);
                if (dtListDefect.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in dtListDefect.AsEnumerable()
                        select t;
                    foreach (DataRow p2 in query)
                    {
                        keyDefectOnkeyRegion.Add(
                            (p2.Field<object>("nDefect_key") != null ? p2.Field<object>("nDefect_key").ToString() : ""));
                    }
                }

                if (keyDefectOnkeyRegion.Count != 0)
                {
                    report = report + ClearBazaOnKeyDefect(keyDefectOnkeyRegion);
                }

                if (keyAktOnkeyRegion.Count != 0)
                {
                    report = report + ClearBazaOnkeyAkt(keyAktOnkeyRegion);
                }

                if (!string.IsNullOrEmpty(keyregion))
                {
                    report = report + ClearbazaPipeOnkeyRegion(keyregion);
                }


            }
            catch (Exception ee)
            {

                report = "Ошибка очистки базы " + ee;
            }

            return report;
        }

        /// <summary>
        /// очищаем базу - все не привязанные к участку трубы
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <returns></returns>
        public static string ClearBazaOnNullRegion(string keyAkt)
        {
            string report = "Труба успешно удалена ";
            //список ключей труб для удаления
            List<string> keyPipeList = new List<string>();
            //список ключей дефектов для удаления
            List<string> keyDefectList = new List<string>();

            List<string> keyAktList = new List<string>();
            keyAktList.Add(keyAkt);

            try
            {
                DataTable dt = ASBLL.GetAllPipe_KeyAktOnRegionIsNull(keyAkt);
                if (dt.Rows.Count != 0)
                {
                    IEnumerable<DataRow> query =
                      from t in dt.AsEnumerable()
                      select t;
                    foreach (DataRow p in query)
                    {
                        keyPipeList.Add(!String.IsNullOrEmpty(p.Field<object>("nKey_pipe").ToString()) ? p.Field<object>("nKey_pipe").ToString()
                          : "0");

                    }

                }

                DataTable dt1 = ASBLL.GetAllKeyDefectOnRegionIsNull(keyPipeList);
                if (dt1.Rows.Count != 0)
                {
                    IEnumerable<DataRow> query1 =
                      from t in dt1.AsEnumerable()
                      select t;
                    foreach (DataRow p1 in query1)
                    {
                        keyDefectList.Add(!String.IsNullOrEmpty(p1.Field<object>("nDefect_key").ToString()) ? p1.Field<object>("nDefect_key").ToString() : "0");

                    }

                }

                if (keyDefectList.Count != 0)
                {
                    report = report + ClearBazaOnKeyDefect(keyDefectList);
                }
                if (keyAktList.Count != 0)
                {
                    report = report + ClearBazaOnkeyAkt(keyAktList);
                }

            }
            catch (Exception ee)
            {
                report = "Ошибка очистки базы " + ee;

            }
            return report;

        }
        /// <summary>
        /// выбрать все акты с непривязанными трубами
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllAktOnRegionIsNull()
        {
            try
            {
                return ASBLL.GetAllAktOnRegionIsNull();
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
                return ASBLL.GetAllPipe_KeyAktOnRegionIsNull(keyAkt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// по ключу участка возвращает все акты на трубы c группировкой
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static DataTable GetPipeOnRegionAllActGroup(string keyRegion)
        {
            return ASBLL.GetPipeOnRegionAllActGroup(keyRegion);
        }

        /// <summary>
        /// возвращает все ключи дефекты на трубах определенного участка (для удаления - чистка участка )
        /// </summary>
        /// <param name="keyRegion"></param>
        /// <returns></returns>
        public static DataTable GetDefectPipeOnKeyRegion(string keyRegion)
        {
            return ASBLL.GetDefectPipeOnKeyRegion(keyRegion);
        }
        /// <summary>
        ///1. чистим таблицы Calculation_defect
        ///2. Defect_metalla
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static string ClearBazaOnKeyDefect(List<string> keyDefect)
        {
            return ASBLL.ClearBazaOnKeyDefect(keyDefect);
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
            return ASBLL.ClearBazaOnkeyAkt(keyAkt);
        }

        /// <summary>
        /// удаляем выбранные акты
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <returns></returns>
        public static string DeleteOnSelectkeyAkt(string keyAkt)
        {
            return ASBLL.DeleteOnSelectkeyAkt(keyAkt);
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
            return ASBLL.ClearbazaPipeOnkeyRegion(keyRegion);
        }

        /// <summary>
        /// возвращает словарь дефектоскопист не используется
        /// </summary>
        /// <returns></returns>
        //public static DataTable GetDictDefectoskopist()
        //{
        //    return ASBLL.GetDictDefectoskopist();
        //}
        /// <summary>
        /// возвращает сдлварь - представитель ЛПУ не используется
        /// </summary>
        /// <returns></returns>
        //public static DataTable GetDictAgentLPU()
        //{
        //    return ASBLL.GetDictAgentLPU();
        //}
        /// <summary>
        /// сохраняет акт - возвращает ключ акта
        /// </summary>
        /// <param name="numberAkt"></param>
        /// <param name="dataShurf"></param>
        /// <param name="keydefectoskop"></param>
        /// <param name="keyAgentLpu"></param>
        /// <param name="selectPipeList"></param>
        /// <returns></returns>
        public static string SaveAct(Dictionary<string, string> d, IList<SelectPipeOnGrid> selectPipeList)
        {
            string keyNewAct = "";
            keyNewAct = ASBLL.SaveAct(d, selectPipeList);
            return keyNewAct;
        }
        /// <summary>
        /// Возвращает все дефекты на выбранных трубах
        /// </summary>
        /// <param name="selectPipeList"></param>
        /// <returns></returns>
        public static DataTable GetDefectPipe(IList<SelectPipeOnGrid> selectPipeList)
        {
            //должна быть сводная таблица дефетов и их расчет, причем поле в расчетах crecom - пишет "допустимый" и т.д.
            // а уже на основании этого вставляем в соответсвующие ячейки картинку
            return ASBLL.GetDefectPipe(selectPipeList);
        }
        /// <summary>
        /// вывод количества дефектов и максимальная минимальная глубина дефектов
        /// </summary>
        /// <param name="selectPipeList"></param>
        /// <returns></returns>
        public static DataTable GetCountDefectDepth(IList<SelectPipeOnGrid> selectPipeList)
        {
            return ASBLL.GetCountDefectDepth(selectPipeList);
        }
        /// <summary>
        /// Возвращает все дефекты на конкретной трубе
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static DataTable GetDefectOnkeypipe(string keyPipe)
        {
            //должна быть сводная таблица дефетов и их расчет, причем поле в расчетах crecom - пишет "допустимый" и т.д.
            // а уже на основании этого вставляем в соответсвующие ячейки картинку
            return ASBLL.GetDefectOnKeyPipe(keyPipe);
        }
        /// <summary>
        /// по ключу дефекта возвращает его расчет
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static DataTable GetCalculateOneDefect(string keyDefect, string keyCAlcDefect)
        {
            return ASBLL.GetCalculateOneDefect(keyDefect, keyCAlcDefect);
        }
        /// <summary>
        /// удаляем расчеты по дефектам
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <param name="keyTypeCalc"></param>
        /// <returns></returns>
        public static string DeleteCalcDefect(string keyDefect)
        {
            return ASBLL.DeleteCalcDefect(keyDefect);
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
            return ASBLL.SaveCalcdefectOnASME(keyDefect, keyTypeCalc, d);
        }

        /// <summary>
        /// удаляем все колонии на заданной трубе
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static string DeleteColonyDefects(string keyPipe)
        {
            return ASBLL.DeleteColonyDefects(keyPipe);
        }

        /// <summary>
        /// удаляем все колонии и группы на заданной трубе
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static string GetountCalcDrfrct(string keyDefect)
        {
            return ASBLL.GetountCalcDrfrct(keyDefect);
        }
        /// <summary>
        /// сохраняем координаты колонии дефектов(расчет)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static string SaveColonyDefects(Dictionary<string, string> d, string keyPipe)
        {
            return ASBLL.SaveColonyDefects(d, keyPipe);
        }
        /// <summary>
        /// сохраняем координаты групп дефектов(расчет)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static string SaveGroupDefects(Dictionary<string, string> d, string keyPipe)
        {
            return ASBLL.SaveGroupDefects(d, keyPipe);
        }
        /// <summary>
        /// по ключу трубы возвращает все группы дефектов
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static DataTable GetGroupDefectsOnkeyPipe(string keyPipe)
        {
            return ASBLL.GetGroupDefectsOnkeyPipe(keyPipe);
        }
        /// <summary>
        /// по ключу трубы получаем колонии дефектов
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static DataTable GetColonyDefectsOnkey(string keyPipe)
        {
            return ASBLL.GetColonyDefectsOnkey(keyPipe);
        }
        /// <summary>
        /// словарь тип дефекта
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDictTypeDefect()
        {
            return ASBLL.GetDictTypeDefect();
        }
        /// <summary>
        /// паспорт дефекта
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static PasportDefectModel GetPasportDefectModel(string key, string keyPipe)
        {
            //загружаем словари 
            DataTable td = ASBLL.GetDictTypeDefect();
            DataTable ct = ASBLL.GetDictCalcType();
            DataTable dp = ASBLL.GetDictDepthPos();
            DataTable mu = ASBLL.GetDictMetodUstr();
            DataTable du = ASBLL.GetDictDefectUstr();

            string valuetd = "";
            string valuect = "";
            string valuedp = "";
            string valuemu = "";
            string valuemu2 = "";
            string valuedu = "";

            int valuetdFind = 0;
            int valuectFind = 0;
            int valuedpFind = 0;
            int valuemuFind = 0;
            int valuemu2Find = 0;
            int valueduFind = 0;


            //наполняем модель данными
            PasportDefectModel pdm = new PasportDefectModel();
            if (key == "0")
            {
                pdm.KeyPipe = keyPipe;
            }
            else
            {
                //загружаем данные
                DataTable dt = new DataTable();
                dt = ASBLL.GetPassportDefect(key);

                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                from t in dt.AsEnumerable()
                select t;
                    foreach (DataRow p in query)
                    {
                        pdm.DefectName = (p.Field<object>("cName") != null
                                              ? p.Field<object>("cName").ToString()
                                              : "");
                       // pdm.W = GetValueRow(p.Field<object>("nlengthDefect"));
                        pdm.W = (p.Field<object>("nlengthDefect") != null
                                              ? p.Field<object>("nlengthDefect").ToString()
                                              : "");

                        //pdm.H = GetValueRow(p.Field<object>("nwidth"));
                        pdm.H = (p.Field<object>("nwidth") != null
                                              ? p.Field<object>("nwidth").ToString()
                                              : "");
                        //pdm.Depth = GetValueRow(p.Field<object>("ndepth"));
                        pdm.Depth = (p.Field<object>("ndepth") != null
                                              ? p.Field<object>("ndepth").ToString()
                                              : "");

                        pdm.LossMetal = GetLossMetal(p.Field<object>("ndepth"), p.Field<object>("cdepth_pipe"));//GetValueRow(p.Field<object>("nloss_metall"));
                        //pdm.PrevSeam_Dist = GetValueRow(p.Field<object>("nprevseam_dist"));
                        pdm.PrevSeam_Dist = (p.Field<object>("nprevseam_dist") != null
                                              ? p.Field<object>("nprevseam_dist").ToString()
                                              : "");

                        pdm.Next_Dist = GetValueRow(p.Field<object>("nnext_dist"));
                        pdm.PrevMark_Dist = GetValueRow(p.Field<object>("nprevmark_dist"));
                        pdm.NextReper_Dist = GetValueRow(p.Field<object>("nnextmark_dist"));
                        //pdm.Angle = GetValueRow(p.Field<object>("nclockwise_pos"));
                        pdm.Angle = (p.Field<object>("nclockwise_pos") != null
                                              ? p.Field<object>("nclockwise_pos").ToString()
                                              : "");
                        pdm.Stress_Koor = (p.Field<object>("cstress_descr") != null
                                              ? p.Field<object>("cstress_descr").ToString()
                                              : "");
                        pdm.DataUstrDefect = (p.Field<object>("cDataUstrDefect") != null
                                          ? p.Field<object>("cDataUstrDefect").ToString()
                                          : "");
                        pdm.KeyPipe = (p.Field<object>("nKey_pipe") != null
                                                                  ? p.Field<object>("nKey_pipe").ToString()
                                                                  : "");
                        valuetd = (p.Field<object>("nTypeDefect_Key") != null
                                              ? p.Field<object>("nTypeDefect_Key").ToString()
                                              : "-1");
                        valuect = (p.Field<object>("nDict_Calc_Type_Key") != null
                                              ? p.Field<object>("nDict_Calc_Type_Key").ToString()
                                              : "-1");
                        valuedp = (p.Field<object>("ndepth_pos_Key") != null
                                              ? p.Field<object>("ndepth_pos_Key").ToString()
                                              : "-1");
                        valuemu = (p.Field<object>("nMetod_Ustr_Def_Key") != null
                                              ? p.Field<object>("nMetod_Ustr_Def_Key").ToString()
                                              : "-1");
                        valuemu2 = (p.Field<object>("nMetod_Ustr_Def_Otrem_Key") != null
                                              ? p.Field<object>("nMetod_Ustr_Def_Otrem_Key").ToString()
                                              : "-1");
                        valuedu = (p.Field<object>("nDict_defect_ustr_key") != null
                                              ? p.Field<object>("nDict_defect_ustr_key").ToString()
                                              : "-1");

                    }
                }
            }

            pdm.KeyDefect = key;

            //словарь - тип дефекта
            pdm.DictListTypeDefect.Clear();
            if (td.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in td.AsEnumerable()
                    select t;

                pdm.DictListTypeDefect.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    pdm.DictListTypeDefect.Add(
                           new Dict
                               (
                                 (p.Field<object>("nTypeDefect_Key") != null ? p.Field<object>("nTypeDefect_Key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }

            //словарь расчетный тип

            pdm.DictListCalcType.Clear();

            if (ct.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in ct.AsEnumerable()
                    select t;

                pdm.DictListCalcType.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    pdm.DictListCalcType.Add(
                           new Dict
                               (
                                 (p.Field<object>("nDict_Calc_Type_Key") != null ? p.Field<object>("nDict_Calc_Type_Key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }

            //словарь расположение в толщине металла
            pdm.DictListDepthPos.Clear();

            if (dp.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in dp.AsEnumerable()
                    select t;

                pdm.DictListDepthPos.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    pdm.DictListDepthPos.Add(
                           new Dict
                               (
                                 (p.Field<object>("ndepth_pos_Key") != null ? p.Field<object>("ndepth_pos_Key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }

            //словарь метод устранения дефекта

            pdm.DictListMetodUstr.Clear();

            if (mu.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in mu.AsEnumerable()
                    select t;

                pdm.DictListMetodUstr.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    pdm.DictListMetodUstr.Add(
                           new Dict
                               (
                                 (p.Field<object>("nMetod_Ustr_Def_Key") != null ? p.Field<object>("nMetod_Ustr_Def_Key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }


            //словарь метод устранения ОТРЕМОНТИРОВАННОГО дефекта
            pdm.DictListMetodUstr2.Clear();

            if (mu.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in mu.AsEnumerable()
                    select t;

                pdm.DictListMetodUstr2.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    pdm.DictListMetodUstr2.Add(
                           new Dict
                               (
                                 (p.Field<object>("nMetod_Ustr_Def_Key") != null ? p.Field<object>("nMetod_Ustr_Def_Key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }

            pdm.DictListDefectUstr.Clear();

            if (du.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in du.AsEnumerable()
                    select t;

                pdm.DictListDefectUstr.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    pdm.DictListDefectUstr.Add(
                           new Dict
                               (
                                 (p.Field<object>("Dict_defect_ustr_key") != null ? p.Field<object>("Dict_defect_ustr_key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }


            if (key == "0")
            {
                pdm.SelectedTypeDefect = pdm.DictListTypeDefect[0];
                pdm.SelectedCalcType = pdm.DictListCalcType[0];
                pdm.SelectedDepthPos = pdm.DictListDepthPos[0];
                pdm.SelectedMetodUstr = pdm.DictListMetodUstr[0];
                pdm.SelectedMetodUstr2 = pdm.DictListMetodUstr2[0];
                pdm.SelectedDefectUstr = pdm.DictListDefectUstr[0];

            }
            else
            {


                for (int ii = 0; ii < pdm.DictListTypeDefect.Count; ii++)
                {

                    if (pdm.DictListTypeDefect[ii].Key == valuetd)
                    {
                        pdm.SelectedTypeDefect = pdm.DictListTypeDefect[ii];
                        valuetdFind = 1;
                        break;
                    }
                }
                for (int ii = 0; ii < pdm.DictListCalcType.Count; ii++)
                {

                    if (pdm.DictListCalcType[ii].Key == valuect)
                    {
                        pdm.SelectedCalcType = pdm.DictListCalcType[ii];
                        valuectFind = 1;
                        break;
                    }
                }

                for (int ii = 0; ii < pdm.DictListDepthPos.Count; ii++)
                {

                    if (pdm.DictListDepthPos[ii].Key == valuedp)
                    {
                        pdm.SelectedDepthPos = pdm.DictListDepthPos[ii];
                        valuedpFind = 1;
                        break;
                    }
                }

                for (int ii = 0; ii < pdm.DictListMetodUstr.Count; ii++)
                {

                    if (pdm.DictListMetodUstr[ii].Key == valuemu)
                    {
                        pdm.SelectedMetodUstr = pdm.DictListMetodUstr[ii];
                        valuemuFind = 1;
                        break;
                    }
                }

                for (int ii = 0; ii < pdm.DictListMetodUstr2.Count; ii++)
                {

                    if (pdm.DictListMetodUstr2[ii].Key == valuemu2)
                    {
                        pdm.SelectedMetodUstr2 = pdm.DictListMetodUstr2[ii];
                        valuemu2Find = 1;
                        break;
                    }
                }

                for (int ii = 0; ii < pdm.DictListDefectUstr.Count; ii++)
                {

                    if (pdm.DictListDefectUstr[ii].Key == valuedu)
                    {
                        pdm.SelectedDefectUstr = pdm.DictListDefectUstr[ii];
                        valueduFind = 1;
                        break;
                    }
                }
                #region проверка на значение. которого нет в словаре
                if (valuetdFind == 0)
                {
                    pdm.DictListTypeDefect.Add(new Dict(valuetd, "<нет соответствия со словарем>"));
                    for (int ii = 0; ii < pdm.DictListTypeDefect.Count; ii++)
                    {
                        if (pdm.DictListTypeDefect[ii].Key == valuetd)
                        {
                            pdm.SelectedTypeDefect = pdm.DictListTypeDefect[ii];
                            break;
                        }
                    }

                }

                if (valuectFind == 0)
                {
                    pdm.DictListCalcType.Add(new Dict(valuect, "<нет соответствия со словарем>"));
                    for (int ii = 0; ii < pdm.DictListCalcType.Count; ii++)
                    {

                        if (pdm.DictListCalcType[ii].Key == valuect)
                        {
                            pdm.SelectedCalcType = pdm.DictListCalcType[ii];
                            break;
                        }
                    }
                }
                if (valuedpFind == 0)
                {
                    pdm.DictListDepthPos.Add(new Dict(valuedp, "<нет соответствия со словарем>"));
                    for (int ii = 0; ii < pdm.DictListDepthPos.Count; ii++)
                    {

                        if (pdm.DictListDepthPos[ii].Key == valuedp)
                        {
                            pdm.SelectedDepthPos = pdm.DictListDepthPos[ii];
                            break;
                        }
                    }
                }
                if (valuemuFind == 0)
                {
                    pdm.DictListMetodUstr.Add(new Dict(valuemu, "<нет соответствия со словарем>"));

                    for (int ii = 0; ii < pdm.DictListMetodUstr.Count; ii++)
                    {

                        if (pdm.DictListMetodUstr[ii].Key == valuemu)
                        {
                            pdm.SelectedMetodUstr = pdm.DictListMetodUstr[ii];
                            break;
                        }
                    }
                }

                if (valuemu2Find == 0)
                {
                    pdm.DictListMetodUstr2.Add(new Dict(valuemu2, "<нет соответствия со словарем>"));
                    for (int ii = 0; ii < pdm.DictListMetodUstr2.Count; ii++)
                    {

                        if (pdm.DictListMetodUstr2[ii].Key == valuemu2)
                        {
                            pdm.SelectedMetodUstr2 = pdm.DictListMetodUstr2[ii];
                            break;
                        }
                    }
                }
                if (valueduFind == 0)
                {
                    pdm.DictListDefectUstr.Add(new Dict(valuedu, "<нет соответствия со словарем>"));
                    for (int ii = 0; ii < pdm.DictListDefectUstr.Count; ii++)
                    {

                        if (pdm.DictListDefectUstr[ii].Key == valuedu)
                        {
                            pdm.SelectedDefectUstr = pdm.DictListDefectUstr[ii];
                            break;
                        }
                    }
                }
                #endregion

            }

            return pdm;
        }
        /// <summary>
        /// сохранение паспорта дефекта
        /// </summary>
        /// <param name="pdm"></param>
        /// <returns></returns>
        public static string SavePasportDefectModel(string keyDefect, Dictionary<string, string> d, string keyPipe, string isedited)
        {
            return ASBLL.SavePasportDefectModel(keyDefect, d, keyPipe, isedited);
        }

        public static string DeleteDefect(string keyDefect)
        {
            return ASBLL.DeleteDefect(keyDefect);
        }

        /// <summary>Dic
        /// дефект подтвержден без редактирования
        /// </summary>
        /// <param name="keyDefect"></param>
        /// <returns></returns>
        public static string ConfirmDefect(string keyDefect)
        {
            return ASBLL.ConfirmDefect(keyDefect);
        }
        /// <summary>
        /// характеристики трубы
        /// </summary>
        /// <param name="keyPipe"></param>
        /// <returns></returns>
        public static CharacteristicPipeModel GetCharacteristicPipe(string keyPipe, string keyShurf)
        {


            #region загружаем словари
            //загружаем словари 
            DataTable fg = ASBLL.GetDictFeelGrade();
            DataTable ku = ASBLL.GetDictKetegor_uch();
            DataTable tk = ASBLL.GetDictTemperaturn_koef();
            DataTable fb = ASBLL.GetDictFactory_builder();
            //DataTable tkv =ASBLL.GetDictTemperaturn_koef();
            //DataTable kuv =new DataTable();

            string valueku = "";//категоря участка
            string valuetk = "";//температурный коэффициент
            string valuefb = "";//завод изготовитель
            string valuefg = "";//марка стали


            string keyFactory = "";
            string keyFellGrade = "";




            #endregion

            //создаем модель  и наполняем модель данными
            CharacteristicPipeModel chp = new CharacteristicPipeModel();

            //загружаем данные
            DataTable dt = new DataTable();
            dt = ASBLL.GetCharacteristicPipe(keyPipe, keyShurf);

            if (dt.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
            from t in dt.AsEnumerable()
            select t;
                foreach (DataRow p in query)
                {
                    chp.LenghtPipe = (p.Field<object>("clength") != null
                                          ? p.Field<object>("clength").ToString()
                                          : "");
                    chp.Diam = (p.Field<object>("cdiam") != null
                                          ? p.Field<object>("cdiam").ToString()
                                          : "");
                    chp.Depth_pipe = (p.Field<object>("cdepth_pipe") != null
                                          ? p.Field<object>("cdepth_pipe").ToString()
                                          : "");
                    chp.MaxPressure = (p.Field<object>("nmaxpressure") != null
                                          ? p.Field<object>("nmaxpressure").ToString()
                                          : "");
                    chp.Pressure = (p.Field<object>("npressure") != null
                                          ? p.Field<object>("npressure").ToString()
                                          : "");
                    chp.NumberPipe = (p.Field<object>("cNumber_pipe") != null
                                          ? p.Field<object>("cNumber_pipe").ToString()
                                          : "");
                    chp.KmPipe = (p.Field<object>("cloc_km_beg") != null
                                          ? p.Field<object>("cloc_km_beg").ToString()
                                          : "");
                    chp.AngleShovPipe = (p.Field<object>("nAngle") != null
                                          ? p.Field<object>("nAngle").ToString()
                                          : "");
                    chp.DataExpl = (p.Field<object>("nDataExpl") != null
                                          ? p.Field<object>("nDataExpl").ToString()
                                          : "");
                    valueku = (p.Field<object>("ndict_kategor_uch_key") != null
                                          ? p.Field<object>("ndict_kategor_uch_key").ToString()
                                          : "-1");

                    valuefb = (p.Field<object>("nFactory_builder_key") != null
                                          ? p.Field<object>("nFactory_builder_key").ToString()
                                          : "-1");
                    valuefg = (p.Field<object>("nDict_feel_grade_key") != null
                                          ? p.Field<object>("nDict_feel_grade_key").ToString()
                                          : "-1");
                    valuetk = (p.Field<object>("nDict_temperaturn_koef_key") != null
                                          ? p.Field<object>("nDict_temperaturn_koef_key").ToString()
                                          : "-1");

                }
            }


            //словарь категория участка


            if (ku.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in ku.AsEnumerable()
                    select t;

                chp.DictListKetegor_uch.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    chp.DictListKetegor_uch.Add(
                           new Dict
                               (
                                 (p.Field<object>("ndict_kategor_uch_key") != null ? p.Field<object>("ndict_kategor_uch_key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                    chp.DictGetKategorUch_value.Add(
                      new Dict
                          (
                            (p.Field<object>("ndict_kategor_uch_key") != null ? p.Field<object>("ndict_kategor_uch_key").ToString() : "-1"),
                            (p.Field<object>("nCoefficient") != null ? p.Field<object>("nCoefficient").ToString() : "-1")

                          )
                      );
                }
            }



            //словарь температурный коэффициент


            if (tk.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in tk.AsEnumerable()
                    select t;

                chp.DictListTemperaturn_koef.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    chp.DictListTemperaturn_koef.Add(
                           new Dict
                               (
                                 (p.Field<object>("nDict_temperaturn_koef_key") != null ? p.Field<object>("nDict_temperaturn_koef_key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                    //температурный коэффициент  - зависимость ключа из таблицы и занчение коэффициент(ДЛЯ РАСЧЕТОВ)
                    chp.DictGetTemperKoef_value.Add(
                         new Dict
                             (
                               (p.Field<object>("nDict_temperaturn_koef_key") != null ? p.Field<object>("nDict_temperaturn_koef_key").ToString() : "-1"),
                               (p.Field<object>("nCoefficient") != null ? p.Field<object>("nCoefficient").ToString() : "-1")
                             )
                         );

                }
            }




            //словарь завод изготовитель 


            if (fb.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in fb.AsEnumerable()
                    select t;

                chp.DictListFactory_builder.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    chp.DictListFactory_builder.Add(
                           new Dict
                               (
                                 (p.Field<object>("nFactory_builder_key") != null ? p.Field<object>("nFactory_builder_key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }

            //словарь марка стали


            if (fg.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                    from t in fg.AsEnumerable()
                    select t;

                chp.DictListFeelGrade.Add(new Dict("-1", "Выбрать"));
                foreach (DataRow p in query)
                {
                    chp.DictListFeelGrade.Add(
                           new Dict
                               (
                                 (p.Field<object>("nDict_feel_grade_key") != null ? p.Field<object>("nDict_feel_grade_key").ToString() : "<не указано>"),
                                 (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                               )
                           );
                }
            }


            // таблица соответствия марки стали и завода изготовителя
            DataTable emb = ASBLL.GetEcvivalent_marka_builder();
            chp.EcvivalentMarka_BuilderList.Clear();

            if (emb.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
            from t in emb.AsEnumerable()
            select t;
                foreach (DataRow p in query)
                {
                    chp.EcvivalentMarka_BuilderList.Add(
                        new EcvivalentMarka_Builder
                            (
                            (p.Field<object>("nFactory_builder_key") != null ? p.Field<object>("nFactory_builder_key").ToString() : "<не указано>"),
                            (p.Field<object>("nDict_feel_grade_key") != null ? p.Field<object>("nDict_feel_grade_key").ToString() : "<не указано>"),
                            (p.Field<object>("nRange_flud") != null ? p.Field<object>("nRange_flud").ToString() : "<не указано>"),
                            (p.Field<object>("nRange_stranght") != null ? p.Field<object>("nRange_stranght").ToString() : "<не указано>"),
                            (p.Field<object>("nModulUng") != null ? p.Field<object>("nModulUng").ToString() : "<не указано>"),
                            (p.Field<object>("nKoefPuanson") != null ? p.Field<object>("nKoefPuanson").ToString() : "<не указано>"),
                            (p.Field<object>("nKoefLinExpansion") != null ? p.Field<object>("nKoefLinExpansion").ToString() : "<не указано>"),
                            (p.Field<object>("cname") != null ? p.Field<object>("cname").ToString() : "<не указано>")
                            )
                        );


                }
            }


            //chp.EcvivalentMarka_BuilderList.Add(new EcvivalentMarka_Builder("1", "1", "2", "3", "5", "4", "6"));
            //chp.EcvivalentMarka_BuilderList.Add(new EcvivalentMarka_Builder("1", "2", "5", "7", "9", "6", "2"));
            //chp.EcvivalentMarka_BuilderList.Add(new EcvivalentMarka_Builder("1", "3", "3", "4", "2", "7", "5"));
            //chp.EcvivalentMarka_BuilderList.Add(new EcvivalentMarka_Builder("1", "4", "7", "2", "8", "1", "9"));

            #region выбираем ключ словаря

            for (int ii = 0; ii < chp.DictListKetegor_uch.Count; ii++)
            {

                if (chp.DictListKetegor_uch[ii].Key == valueku)
                {
                    chp.SelectedKetegor_uch = chp.DictListKetegor_uch[ii];
                    break;
                }
            }

            for (int ii = 0; ii < chp.DictListFactory_builder.Count; ii++)
            {

                if (chp.DictListFactory_builder[ii].Key == valuefb)
                {
                    chp.SelectedFactory_builder = chp.DictListFactory_builder[ii];
                    //когда известен ключ завода ищем все марки стали этого завода и наполняем комбобокс для отображения
                    chp.DictListFactoryOnFeelGrade.Clear();
                    chp.DictListFactoryOnFeelGrade.Add(new Dict("-1", "Выбрать"));

                    foreach (var embL in chp.EcvivalentMarka_BuilderList)
                    {
                        if (embL.KeFactory_builder == valuefb)
                        {

                            chp.DictListFactoryOnFeelGrade.Add(
                          new Dict
                              (
                                (embL.KeyFeel_grade),
                                (embL.NameFeelGrade)
                              )
                          );

                        }
                    }
                    break;
                }
            }

            //если не нашли соответствия - то наполняем комбобокс всеми марками стали
            if (chp.DictListFactoryOnFeelGrade.Count == 1)
            {
                chp.DictListFactoryOnFeelGrade.Clear();

                foreach (var embL in chp.DictListFeelGrade)
                {
                    chp.DictListFactoryOnFeelGrade.Add(
                  new Dict
                      (
                        (embL.Key),
                        (embL.Name)
                      )
                  );
                }
            }


            for (int ii = 0; ii < chp.DictListFactoryOnFeelGrade.Count; ii++)
            {

                if (chp.DictListFactoryOnFeelGrade[ii].Key == valuefg)
                {
                    chp.SelectedFeelGrade = chp.DictListFactoryOnFeelGrade[ii];
                    break;
                }
            }

            //for (int ii = 0; ii < chp.DictListFeelGrade.Count; ii++)
            //{

            //    if (chp.DictListFeelGrade[ii].Key == valuefg)
            //    {
            //        chp.SelectedFeelGrade = chp.DictListFeelGrade[ii];
            //        break;
            //    }
            //}

            for (int ii = 0; ii < chp.DictListTemperaturn_koef.Count; ii++)
            {

                if (chp.DictListTemperaturn_koef[ii].Key == valuetk)
                {
                    chp.SelectedTemperaturn_koef = chp.DictListTemperaturn_koef[ii];
                    break;
                }
            }

            #endregion

            return chp;
        }

        public static string SaveCharacteristicPipe(CharacteristicPipeModel cpm, string keyPipe)
        {
            return ASBLL.SaveCharacteristicPipe(cpm, keyPipe);
        }

        public static DocumentPresentationModel GetDocumentPresentation(string keyShurf)
        {

            string valueds = "";//ключ дефектоскопист
            string valueal = "";//ключ представитель ЛПУ
            string valuecgr = "";//ключ  характеристика грунта
            string valueti = "";//ключ тип изоляции
            string valueistate = "";//ключ состояние защитного покрытия
            string valueavo = "";//ключ наличие нахлеста
            string valueap = "";//ключ прилепаемость к трубе
            string valueadump = "";//ключ наличие влаги под изоляцией



            //наполняем модель данными
            DocumentPresentationModel dpm = new DocumentPresentationModel();


            try
            {


                #region данные по акту
                //загружаем данные
                DataTable dt = new DataTable();
                dt = ASBLL.GetDocumentPresecationShurf(keyShurf);

                if (dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in dt.AsEnumerable()
                        select t;
                    foreach (DataRow p in query)
                    {
                        dpm.KeyAkt = (p.Field<object>("nShurf_key") != null
                                        ? p.Field<object>("nShurf_key").ToString()
                                        : "");
                        dpm.NumberAkt = (p.Field<object>("cNumberAkt") != null
                                        ? p.Field<object>("cNumberAkt").ToString()
                                        : "");


                        dpm.DataShurf = (p.Field<object>("dDateShurf") != null
                                       ? p.Field<object>("dDateShurf").ToString()
                                       : "");

                        dpm.DataVisible = (p.Field<object>("dDate_visit") != null
                                        ? p.Field<object>("dDate_visit").ToString()
                                        : "");
                        dpm.DataExpl = (p.Field<object>("nDataExpl") != null
                                        ? p.Field<object>("nDataExpl").ToString()
                                        : "");

                        //valueds = (p.Field<object>("ndict_defectoskopist_key") != null
                        //               ? p.Field<object>("ndict_defectoskopist_key").ToString()
                        //               : "-1");
                        //valueal = (p.Field<object>("ndict_agent_LPU_Key") != null
                        //               ? p.Field<object>("ndict_agent_LPU_Key").ToString()
                        //               : "-1");

                        dpm.Charakter_grount = (p.Field<object>("crelief_ground") != null
                                        ? p.Field<object>("crelief_ground").ToString()
                                        : "");
                        valuecgr = (p.Field<object>("nDict_Character_grunt_key") != null
                                        ? p.Field<object>("nDict_Character_grunt_key").ToString()
                                        : "-1");

                        dpm.Depth = GetValueRowNull(p.Field<object>("ndepth_location"));
                        dpm.Height = GetValueRowNull(p.Field<object>("ndepth"));

                        dpm.Lenght = GetValueRowNull(p.Field<object>("nlength"));

                        dpm.WaterLevel = GetValueRowNull(p.Field<object>("cwater_level"));
                        dpm.Rezistor_grunt = GetValueRowNull(p.Field<object>("nrezistor_grunt"));
                        dpm.Voltage_pipe = GetValueRowNull(p.Field<object>("nvoltage_pipe"));
                        dpm.AgentLPU = (p.Field<object>("cagentLPU") != null
                                        ? p.Field<object>("cagentLPU").ToString()
                                        : "");
                        dpm.Defectoskopist = (p.Field<object>("cdefectoskopist") != null
                                        ? p.Field<object>("cdefectoskopist").ToString()
                                        : "");
                        valueti = (p.Field<object>("ntype_izol_key") != null
                                        ? p.Field<object>("ntype_izol_key").ToString()
                                        : "-1");
                        valueistate = (p.Field<object>("nDict_Izol_state_key") != null
                                        ? p.Field<object>("nDict_Izol_state_key").ToString()
                                        : "-1");

                        dpm.Number_skin = (p.Field<object>("nNumber_skin") != null
                                          ? p.Field<object>("nNumber_skin").ToString()
                                          : "");
                        dpm.Number_obertka = (p.Field<object>("nNumberObertka") != null
                                           ? p.Field<object>("nNumberObertka").ToString()
                                           : "");
                        valueavo = (p.Field<object>("ndict_availabi_overl_key") != null
                                        ? p.Field<object>("ndict_availabi_overl_key").ToString()
                                        : "-1");
                        valueap = (p.Field<object>("ndict_adherenc_pipe_key") != null
                                         ? p.Field<object>("ndict_adherenc_pipe_key").ToString()
                                         : "-1");
                        valueadump = (p.Field<object>("ndict_availabil_damp_key") != null
                                          ? p.Field<object>("ndict_availabil_damp_key").ToString()
                                          : "-1");
                        dpm.Inspect_squre = GetValueRowNull(p.Field<object>("nInspect_square"));
                        dpm.Inspect_koroz = GetValueRowNull(p.Field<object>("nInspect_square_koroz"));
                        dpm.Vid_koroz = (p.Field<object>("cVid_koroz_damage") != null
                                            ? p.Field<object>("cVid_koroz_damage").ToString()
                                            : "");
                        dpm.Consulusion = (p.Field<object>("cConculusion") != null
                                             ? p.Field<object>("cConculusion").ToString()
                                             : "");


                    }
                }


                #endregion

                #region загружаем словари



                //словарь  характеристика грунта
                DataTable cgr = ASBLL.GetDictCharacter_grunt();

                dpm.DictListCharacter_grunt.Clear();

                if (cgr.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in cgr.AsEnumerable()
                        select t;

                    dpm.DictListCharacter_grunt.Add(new Dict("-1", "Выбрать"));
                    foreach (DataRow p in query)
                    {
                        dpm.DictListCharacter_grunt.Add(
                               new Dict
                                   (
                                     (p.Field<object>("nDict_Character_grunt_key") != null ? p.Field<object>("nDict_Character_grunt_key").ToString() : "<не указано>"),
                                     (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                                   )
                               );
                    }
                }
                // словарь тип изоляции
                DataTable ti = ASBLL.GetDictType_izol();
                dpm.DictListType_izol.Clear();

                if (ti.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in ti.AsEnumerable()
                        select t;

                    dpm.DictListType_izol.Add(new Dict("-1", "Выбрать"));
                    foreach (DataRow p in query)
                    {
                        dpm.DictListType_izol.Add(
                               new Dict
                                   (
                                     (p.Field<object>("ntype_izol_key") != null ? p.Field<object>("ntype_izol_key").ToString() : "<не указано>"),
                                     (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                                   )
                               );
                    }
                }

                //словарь состояние защитного покрытия
                DataTable istate = ASBLL.GetDictIzol_state();

                dpm.DictListIzol_state.Clear();

                if (istate.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in istate.AsEnumerable()
                        select t;

                    dpm.DictListIzol_state.Add(new Dict("-1", "Выбрать"));
                    foreach (DataRow p in query)
                    {
                        dpm.DictListIzol_state.Add(
                               new Dict
                                   (
                                     (p.Field<object>("nDict_Izol_state_key") != null ? p.Field<object>("nDict_Izol_state_key").ToString() : "<не указано>"),
                                     (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                                   )
                               );
                    }
                }

                //словарь наличие нахлеста
                DataTable avo = ASBLL.GetDictAvailabl_overt();

                dpm.DictListAvailabl_overt.Clear();

                if (avo.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in avo.AsEnumerable()
                        select t;

                    dpm.DictListAvailabl_overt.Add(new Dict("-1", "Выбрать"));
                    foreach (DataRow p in query)
                    {
                        dpm.DictListAvailabl_overt.Add(
                               new Dict
                                   (
                                     (p.Field<object>("ndict_availabi_overl_key") != null ? p.Field<object>("ndict_availabi_overl_key").ToString() : "<не указано>"),
                                     (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                                   )
                               );
                    }
                }

                //словарь прилепаемость к трубе
                DataTable ap = ASBLL.GetDictAdherenc_pipe();

                dpm.DictListAdherenc_pipe.Clear();

                if (ap.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in ap.AsEnumerable()
                        select t;

                    dpm.DictListAdherenc_pipe.Add(new Dict("-1", "Выбрать"));
                    foreach (DataRow p in query)
                    {
                        dpm.DictListAdherenc_pipe.Add(
                               new Dict
                                   (
                                     (p.Field<object>("ndict_adherenc_pipe_key") != null ? p.Field<object>("ndict_adherenc_pipe_key").ToString() : "<не указано>"),
                                     (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                                   )
                               );
                    }
                }
                // словарь наличие влаги под изоляцией
                DataTable adump = ASBLL.GetDictAvailabl_dump();

                dpm.DictListAvailabl_dump.Clear();

                if (adump.Rows.Count > 0)
                {
                    IEnumerable<DataRow> query =
                        from t in adump.AsEnumerable()
                        select t;

                    dpm.DictListAvailabl_dump.Add(new Dict("-1", "Выбрать"));
                    foreach (DataRow p in query)
                    {
                        dpm.DictListAvailabl_dump.Add(
                               new Dict
                                   (
                                     (p.Field<object>("ndict_availabil_damp_key") != null ? p.Field<object>("ndict_availabil_damp_key").ToString() : "<не указано>"),
                                     (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
                                   )
                               );
                    }
                }



                #endregion загружаем словари


                #region выбираем ключ-значение словаря из данных акта

                //for (int ii = 0; ii < dpm.DictListDefectockopist.Count; ii++)
                //{

                //    if (dpm.DictListDefectockopist[ii].Key == valueds)
                //    {
                //        dpm.SelectedDefectockopist = dpm.DictListDefectockopist[ii];
                //        break;
                //    }
                //}

                //for (int ii = 0; ii < dpm.DictListAgentLPU.Count; ii++)
                //{

                //    if (dpm.DictListAgentLPU[ii].Key == valueal)
                //    {
                //        dpm.SelectedDictListAgentLPU = dpm.DictListAgentLPU[ii];
                //        break;
                //    }
                //}

                for (int ii = 0; ii < dpm.DictListCharacter_grunt.Count; ii++)
                {

                    if (dpm.DictListCharacter_grunt[ii].Key == valuecgr)
                    {
                        dpm.SelectedDictListCharacter_grunt = dpm.DictListCharacter_grunt[ii];
                        break;
                    }
                }

                for (int ii = 0; ii < dpm.DictListType_izol.Count; ii++)
                {

                    if (dpm.DictListType_izol[ii].Key == valueti)
                    {
                        dpm.SelectedDictListType_izol = dpm.DictListType_izol[ii];
                        break;
                    }
                }

                for (int ii = 0; ii < dpm.DictListIzol_state.Count; ii++)
                {

                    if (dpm.DictListIzol_state[ii].Key == valueistate)
                    {
                        dpm.SelectedDictListIzol_state = dpm.DictListIzol_state[ii];
                        break;
                    }
                }

                for (int ii = 0; ii < dpm.DictListAvailabl_overt.Count; ii++)
                {

                    if (dpm.DictListAvailabl_overt[ii].Key == valueavo)
                    {
                        dpm.SelectedDictListAvailabl_overt = dpm.DictListAvailabl_overt[ii];
                        break;
                    }
                }

                for (int ii = 0; ii < dpm.DictListAdherenc_pipe.Count; ii++)
                {

                    if (dpm.DictListAdherenc_pipe[ii].Key == valueap)
                    {
                        dpm.SelectedDictListAdherenc_pipe = dpm.DictListAdherenc_pipe[ii];
                        break;
                    }
                }

                for (int ii = 0; ii < dpm.DictListAvailabl_dump.Count; ii++)
                {

                    if (dpm.DictListAvailabl_dump[ii].Key == valueadump)
                    {
                        dpm.SelectedDictListAvailabl_dump = dpm.DictListAvailabl_dump[ii];
                        break;
                    }
                }

                #endregion

                return dpm;
            }
            catch (Exception e)
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
            return ASBLL.GetListAct();
        }
        /// <summary>
        /// показывать строку репортса или нет
        /// </summary>
        /// <returns></returns>
        public static int GetVisibleReport()
        {
            return ASBLL.GetVisibleReport();
        }
        /// <summary>
        /// возвращает список всех номеров труб на которых есть определенный акт
        /// </summary>
        /// <param name="keyAkt"></param>
        /// <returns></returns>
        public static DataTable GetNumberPipeOnAkt(string keyAkt)
        {
            return ASBLL.GetNumberPipeOnAkt(keyAkt);

        }
        /// <summary>
        /// список всех участков
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllRegions()
        {
            return ASBLL.GetAllRegions();
        }
        /// <summary>
        /// сохранение всего акта (документальное представление)
        /// </summary>
        /// <param name="keyAct"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string SaveAllAct(string keyAct, Dictionary<string, string> d)
        {
            return ASBLL.SaveAllAct(keyAct, d);
        }

        public static string SetSignatureAkt(string keyAkt, string isedit)
        {
            return ASBLL.SetSignatureAkt(keyAkt, isedit);
        }

        public static string SaveNewPipeUpdate(string keyPipe, Dictionary<string, string> d)
        {
            return ASBLL.SaveNewPipeUpdate(keyPipe, d);
        }

        /// <summary>
        /// по ключу акта возвращаем список труб
        /// </summary>
        /// <param name="keyAct"></param>
        /// <returns></returns>
        public static DataTable GetListPipeOnkeyAct(string keyAct)
        {
            return ASBLL.GetListPipeOnkeyAct(keyAct);
        }

        public static DataTable GetPipeOnkey(string keyPipe)
        {
            return ASBLL.GetPipeOnkey(keyPipe);
        }

        public static string CreateNewPipe()
        {
            return ASBLL.CreateNewPipe();
        }
        /// <summary>
        /// возврат всех новых актов для выгрузки в иас
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAktOnIas(string selectAkt)
        {
            return ASBLL.GetAktOnIas(selectAkt);
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
            return ASBLL.GetDefectOnIas(onKeyShurf);
        }
        /// <summary>
        /// выгружаем все шурф-труба
        /// </summary>
        /// <param name="onKeyShurf"></param>
        /// <returns></returns>
        public static DataTable GetShurf_Truba(string selectAkt)
        {
            return ASBLL.GetShurf_Truba(selectAkt);
        }
        /// <summary>
        /// очистка базы данных
        /// </summary>
        /// <returns></returns>
        public static string ClearBasa()
        {
            return ASBLL.ClearBasa();
        }
        public static string AddTable(string nameTable, Dictionary<string, string> d)
        {
            string addtable = ASBLL.AddTable(nameTable, d);
            return addtable;

        }

        //#endregion
    }
}

//public static DocumentPresentationModel GetDocumentPresentation(string keyShurf)
//{
//    //загружаем словари 
//    //DataTable ds = ASBLL.GetDictDefectoskopist();
//    //DataTable al = ASBLL.GetDictAgentLPU();
//    //DataTable cg = ASBLL.GetDictCharacter_grunt();
//    //DataTable ti = ASBLL.GetDictType_izol();
//    //DataTable istate = ASBLL.GetDictIzol_state();
//    //DataTable avo = ASBLL.GetDictAvailabl_overt();
//    //DataTable ap = ASBLL.GetDictAdherenc_pipe();
//    //DataTable adump = ASBLL.GetDictAvailabl_dump();


//    //загружаем данные
//    DataTable dt = new DataTable();
//    dt = ASBLL.GetDocumentPresecationShurf(keyShurf);
//    //EcvivalentMarka_Builder



//    //наполняем модель данными
//    DocumentPresentationModel dpm = new DocumentPresentationModel();

//    dpm.DictListDefectockopist.Clear();
//    dpm.DictListDefectockopist.Add(new Dict("1", "Иванов"));
//    dpm.DictListDefectockopist.Add(new Dict("2", "Петров"));
//    dpm.DictListDefectockopist.Add(new Dict("3", "Сидоров"));

//    dpm.DictListAgentLPU.Clear();
//    dpm.DictListAgentLPU.Add(new Dict("1", "Иван"));
//    dpm.DictListAgentLPU.Add(new Dict("2", "Петр"));
//    dpm.DictListAgentLPU.Add(new Dict("3", "Светлана"));

//    dpm.DictListCharacter_grunt.Clear();
//    dpm.DictListCharacter_grunt.Add(new Dict("1", "грунт1"));
//    dpm.DictListCharacter_grunt.Add(new Dict("2", "грунт2"));
//    dpm.DictListCharacter_grunt.Add(new Dict("3", "грунт3"));


//    dpm.DictListType_izol.Clear();
//    dpm.DictListType_izol.Add(new Dict("1", "изоляция1"));
//    dpm.DictListType_izol.Add(new Dict("2", "изоляция2"));
//    dpm.DictListType_izol.Add(new Dict("3", "изоляция3"));
//    dpm.DictListType_izol.Add(new Dict("4", "изоляция4"));


//    dpm.DictListIzol_state.Clear();
//    dpm.DictListIzol_state.Add(new Dict("1", "покрытие1"));
//    dpm.DictListIzol_state.Add(new Dict("2", "покрытие2"));
//    dpm.DictListIzol_state.Add(new Dict("3", "покрытие3"));


//    dpm.DictListAvailabl_overt.Clear();
//    dpm.DictListAvailabl_overt.Add(new Dict("1", "нахлест1"));
//    dpm.DictListAvailabl_overt.Add(new Dict("2", "нахлест2"));
//    dpm.DictListAvailabl_overt.Add(new Dict("3", "нахлест3"));

//    dpm.DictListAdherenc_pipe.Clear();
//    dpm.DictListAdherenc_pipe.Add(new Dict("1", "прилепаемость1"));
//    dpm.DictListAdherenc_pipe.Add(new Dict("2", "прилепаемость2"));
//    dpm.DictListAdherenc_pipe.Add(new Dict("3", "прилепаемость3"));

//    dpm.DictListAvailabl_dump.Clear();
//    dpm.DictListAvailabl_dump.Add(new Dict("1", "наличие влаги1"));
//    dpm.DictListAvailabl_dump.Add(new Dict("2", "наличие влаги2"));
//    dpm.DictListAvailabl_dump.Add(new Dict("3", "наличие влаги3"));

//    dpm.NumberAkt = "18";
//    dpm.DataShurf = DateTime.Now;
//    dpm.DataVisible = DateTime.Today;
//    dpm.SelectedDefectockopist = dpm.DictListDefectockopist[2];
//    dpm.SelectedDictListAgentLPU = dpm.DictListAgentLPU[1];
//    dpm.DataExpl = "2012";
//    dpm.Charakter_grount = "земля";
//    dpm.SelectedDictListCharacter_grunt = dpm.DictListCharacter_grunt[2];
//    dpm.Depth = "3";
//    dpm.Height = "1";
//    dpm.Lenght = "0.4";
//    dpm.WaterLevel = "0.1";
//    dpm.Rezistor_grunt = "34";
//    dpm.Voltage_pipe = "56";
//    dpm.SelectedDictListType_izol = dpm.DictListType_izol[2];
//    dpm.SelectedDictListIzol_state = dpm.DictListIzol_state[1];
//    dpm.Number_skin = "1";
//    dpm.Number_obertka = "2";
//    dpm.SelectedDictListAvailabl_overt = dpm.DictListAvailabl_overt[0];
//    dpm.SelectedDictListAdherenc_pipe = dpm.DictListAdherenc_pipe[1];
//    dpm.SelectedDictListAvailabl_dump = dpm.DictListAvailabl_dump[2];
//    dpm.Inspect_squre = "2";
//    dpm.Inspect_koroz = "1";
//    dpm.Vid_koroz = "трещина";
//    dpm.CKZ = "1";
//    dpm.CKZ1 = "2";
//    dpm.I = "3";
//    dpm.I1 = "4";
//    dpm.U = "5";
//    dpm.U1 = "6";
//    dpm.Consulusion = "slkdjglwkjblkjwblkjb";

//    return dpm;
//}
//загружаем словари 

//DataTable ds = ASBLL.GetDictDefectoskopist();
////словарь дефектоскопист
//dpm.DictListDefectockopist.Clear();

//if (ds.Rows.Count > 0)
//{
//    IEnumerable<DataRow> query =
//        from t in ds.AsEnumerable()
//        select t;

//    dpm.DictListDefectockopist.Add(new Dict("-1", "Выбрать"));
//    foreach (DataRow p in query)
//    {
//        dpm.DictListDefectockopist.Add(
//               new Dict
//                   (
//                     (p.Field<object>("ndict_defectoskopist_key") != null ? p.Field<object>("ndict_defectoskopist_key").ToString() : "<не указано>"),
//                     (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
//                   )
//               );
//    }
//}

//словарь представитель ЛПУ
//DataTable al = ASBLL.GetDictAgentLPU();

//dpm.DictListAgentLPU.Clear();
//if (al.Rows.Count > 0)
//{
//    IEnumerable<DataRow> query =
//        from t in al.AsEnumerable()
//        select t;

//    dpm.DictListAgentLPU.Add(new Dict("-1", "Выбрать"));
//    foreach (DataRow p in query)
//    {
//        dpm.DictListAgentLPU.Add(
//               new Dict
//                   (
//                     (p.Field<object>("ndict_agent_LPU_Key") != null ? p.Field<object>("ndict_agent_LPU_Key").ToString() : "<не указано>"),
//                     (p.Field<object>("cName") != null ? p.Field<object>("cName").ToString() : "<не указано>")
//                   )
//               );
//    }
//}
