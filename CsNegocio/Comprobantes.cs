using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsDatos;


namespace CsNegocio
{
    public class Comprobantes
    {
        public void Actualizar(string CfeTipo,string codmov ,string nromov, string status,string status_desc,string anulado, string error, string error_desc)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[8];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@status", status);
            sPar[3] = new DBHelper.StoredProcedureParameter("@status_desc", status_desc);
            sPar[4] = new DBHelper.StoredProcedureParameter("@anulado", anulado);
            sPar[5] = new DBHelper.StoredProcedureParameter("@CfeTipo", CfeTipo);
            sPar[6] = new DBHelper.StoredProcedureParameter("@error", error);
            sPar[7] = new DBHelper.StoredProcedureParameter("@error_desc", error_desc);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("elect_comprobantes_actualizar_estado", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Comprobantes", "Actualizar", ex,"0");
            }
            finally
            {
                oDb.Disconnect();
            }
        }

    }
}
