package exchange;

import android.content.Context;
import android.database.Cursor;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import database.ConnectionFactory;
import database.DataBaseHelper;

/**
 * Created with IntelliJ IDEA.
 * User: ihor
 * Date: 4/1/13
 * Time: 1:42 PM
 */
public class Exporter {

    private DataBaseHelper dbHelper;

    public Exporter(Context context){
        dbHelper= ConnectionFactory.getDataBaseHelper(context);
    }

    public String getOutput() {
        JsonArray result=new JsonArray();

        String sqlShurf = "select nShurf_key, cNumberAkt, dDateShurf, crelief_ground, nDict_Character_grunt_key," +
                "ndepth_location, nrezistor_grunt, nvoltage_pipe, ndepth, nlength, cwater_level, ntype_izol_key, nDict_Izol_state_key, nNumber_skin," +
                "nNumberObertka, ndict_availabi_overl_key, ndict_adherenc_pipe_key, ndict_availabil_damp_key, nInspect_square," +
                "nInspect_square_koroz, cVid_koroz_damage, cConculusion, dDate_visit, ndict_defectoskopist_key, ndict_agent_LPU_Key," +
                "dChange_date, dEditing_data, nDict_temperaturn_koef_key, nCkz1, I1, U1, I2, U2, nCkz2 from Shurf where (nShurf_key % 100)=0" ;
        Cursor shurfs=dbHelper.runQuery(sqlShurf);

        while(shurfs.moveToNext()){
            JsonObject doc=new JsonObject();
            JsonObject shurf = new JsonObject();
            shurf.addProperty("nShurf_key",shurfs.getString(shurfs.getColumnIndex("nShurf_key")));
            shurf.addProperty("cNumberAkt",shurfs.getString(shurfs.getColumnIndex("cNumberAkt")));
            shurf.addProperty("dDateShurf",shurfs.getString(shurfs.getColumnIndex("dDateShurf")));
            shurf.addProperty("crelief_ground",shurfs.getString(shurfs.getColumnIndex("crelief_ground")));
            shurf.addProperty("nDict_Character_grunt_key",shurfs.getString(shurfs.getColumnIndex("nDict_Character_grunt_key")));
            shurf.addProperty("ndepth_location",shurfs.getString(shurfs.getColumnIndex("ndepth_location")));
            shurf.addProperty("nrezistor_grunt",shurfs.getString(shurfs.getColumnIndex("nrezistor_grunt")));
            shurf.addProperty("nvoltage_pipe",shurfs.getString(shurfs.getColumnIndex("nvoltage_pipe")));
            shurf.addProperty("ndepth",shurfs.getString(shurfs.getColumnIndex("ndepth")));
            shurf.addProperty("nlength",shurfs.getString(shurfs.getColumnIndex("nlength")));
            shurf.addProperty("cwater_level",shurfs.getString(shurfs.getColumnIndex("cwater_level")));
            shurf.addProperty("ntype_izol_key",shurfs.getString(shurfs.getColumnIndex("ntype_izol_key")));
            shurf.addProperty("nDict_Izol_state_key",shurfs.getString(shurfs.getColumnIndex("nDict_Izol_state_key")));
            shurf.addProperty("nNumber_skin",shurfs.getString(shurfs.getColumnIndex("nNumber_skin")));
            shurf.addProperty("nNumberObertka",shurfs.getString(shurfs.getColumnIndex("nNumberObertka")));
            shurf.addProperty("ndict_availabi_overl_key",shurfs.getString(shurfs.getColumnIndex("ndict_availabi_overl_key")));
            shurf.addProperty("ndict_adherenc_pipe_key",shurfs.getString(shurfs.getColumnIndex("ndict_adherenc_pipe_key")));
            shurf.addProperty("ndict_availabil_damp_key",shurfs.getString(shurfs.getColumnIndex("ndict_availabil_damp_key")));
            shurf.addProperty("nInspect_square",shurfs.getString(shurfs.getColumnIndex("nInspect_square")));
            shurf.addProperty("nInspect_square_koroz",shurfs.getString(shurfs.getColumnIndex("nInspect_square_koroz")));
            shurf.addProperty("cVid_koroz_damage",shurfs.getString(shurfs.getColumnIndex("cVid_koroz_damage")));
            shurf.addProperty("cConculusion",shurfs.getString(shurfs.getColumnIndex("cConculusion")));
            shurf.addProperty("dDate_visit",shurfs.getString(shurfs.getColumnIndex("dDate_visit")));
            shurf.addProperty("ndict_defectoskopist_key",shurfs.getString(shurfs.getColumnIndex("ndict_defectoskopist_key")));
            shurf.addProperty("ndict_agent_LPU_Key",shurfs.getString(shurfs.getColumnIndex("ndict_agent_LPU_Key")));
            shurf.addProperty("dChange_date",shurfs.getString(shurfs.getColumnIndex("dChange_date")));
            shurf.addProperty("dEditing_data",shurfs.getString(shurfs.getColumnIndex("dEditing_data")));
            shurf.addProperty("nDict_temperaturn_koef_key",shurfs.getString(shurfs.getColumnIndex("nDict_temperaturn_koef_key")));
            shurf.addProperty("nCkz1",shurfs.getString(shurfs.getColumnIndex("nCkz1")));
            shurf.addProperty("I1",shurfs.getString(shurfs.getColumnIndex("I1")));
            shurf.addProperty("U1",shurfs.getString(shurfs.getColumnIndex("U1")));
            shurf.addProperty("I2",shurfs.getString(shurfs.getColumnIndex("I2")));
            shurf.addProperty("U2",shurfs.getString(shurfs.getColumnIndex("U2")));
            shurf.addProperty("nCkz2",shurfs.getString(shurfs.getColumnIndex("nCkz2")));

            JsonArray defects=new JsonArray();

            String sqlDefects="select nDefect_key, nlengthDefect, nTypeDefect_Key, nclockwise_pos, nwidth, ndepth,  cName," +
                    "nloss_metall, nprevseam_dist, nnext_dist, nprevmark_dist, nnextmark_dist, cstress_descr, nKey_pipe, " +
                    "nDict_Calc_Type_Key, ndepth_pos_Key, nMetod_Ustr_Def_Key, dChange_date, cDataUstrDefect, nDict_defect_ustr_key, " +
                    "isEdited, nMetod_Ustr_Def_Otrem_Key, nGroup_key" +
                    " from Defect_metalla where nKey_pipe in (select nKey_pipe from Shurf_Truba where nShurf_key=?)";
            Cursor cs=dbHelper.runQuery(sqlDefects, shurfs.getString(shurfs.getColumnIndex("nShurf_key")));
            while (cs.moveToNext()){
              JsonObject defectObj=new JsonObject();
                defectObj.addProperty("nDefect_key",cs.getString(cs.getColumnIndex("nDefect_key")));
                defectObj.addProperty("nlengthDefect",cs.getString(cs.getColumnIndex("nlengthDefect")));
                defectObj.addProperty("nTypeDefect_Key",cs.getString(cs.getColumnIndex("nTypeDefect_Key")));
                defectObj.addProperty("nclockwise_pos",cs.getString(cs.getColumnIndex("nclockwise_pos")));
                defectObj.addProperty("nwidth",cs.getString(cs.getColumnIndex("nwidth")));
                defectObj.addProperty("ndepth",cs.getString(cs.getColumnIndex("ndepth")));
                defectObj.addProperty("cName",cs.getString(cs.getColumnIndex("cName")));
                defectObj.addProperty("nloss_metall",cs.getString(cs.getColumnIndex("nloss_metall")));
                defectObj.addProperty("nprevseam_dist",cs.getString(cs.getColumnIndex("nprevseam_dist")));
                defectObj.addProperty("nnext_dist",cs.getString(cs.getColumnIndex("nnext_dist")));
                defectObj.addProperty("nprevmark_dist",cs.getString(cs.getColumnIndex("nprevmark_dist")));
                defectObj.addProperty("cstress_descr",cs.getString(cs.getColumnIndex("cstress_descr")));
                defectObj.addProperty("nKey_pipe",cs.getString(cs.getColumnIndex("nKey_pipe")));
                defectObj.addProperty("nDict_Calc_Type_Key",cs.getString(cs.getColumnIndex("nDict_Calc_Type_Key")));
                defectObj.addProperty("ndepth_pos_Key",cs.getString(cs.getColumnIndex("ndepth_pos_Key")));
                defectObj.addProperty("nMetod_Ustr_Def_Key",cs.getString(cs.getColumnIndex("nMetod_Ustr_Def_Key")));
                defectObj.addProperty("dChange_date",cs.getString(cs.getColumnIndex("dChange_date")));
                defectObj.addProperty("cDataUstrDefect",cs.getString(cs.getColumnIndex("cDataUstrDefect")));
                defectObj.addProperty("nDict_defect_ustr_key",cs.getString(cs.getColumnIndex("nDict_defect_ustr_key")));
                defectObj.addProperty("isEdited",cs.getString(cs.getColumnIndex("isEdited")));
                defectObj.addProperty("nMetod_Ustr_Def_Otrem_Key",cs.getString(cs.getColumnIndex("nMetod_Ustr_Def_Otrem_Key")));
                defectObj.addProperty("nGroup_key",cs.getString(cs.getColumnIndex("nGroup_key")));

              defects.add(defectObj);
            }

           shurf.add("Defects",defects);

            result.add(shurf);

        }

        return result.toString();
    }


}
