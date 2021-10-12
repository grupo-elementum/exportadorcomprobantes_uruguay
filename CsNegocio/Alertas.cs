using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsDatos;

namespace CsNegocio
{
    public class Alertas
    {
        public  void ProcesoImproductivosNuevo(int cant_dias_consumo)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Cantdiasconsumo", cant_dias_consumo );

            try
            {

                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("ALERTAS_ClientesAlertasImproductivos_NUEVO", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Proceso Alertas", "Error ejecutando proceso alertas", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void ProcesoImproductivos_actualizaImproductivos(int diasconsumo)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@DIAS_CONSUMO", diasconsumo);

            try
            {

                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("ALERTAS_ClientesAlertasImproductivos_Actualizar_NUEVO", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Proceso Alertas", "Error ejecutando proceso alertas", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }

        public void ProcesoImproductivos_actualizaComodatos()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@usuario", "ALERTAS PROCESO");

            try
            {

                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("ALERTAS_IMPRODUCTIVOS_ACTUALIZAR_COMODATO", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Proceso Alertas", "Error ejecutando proceso alertas", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }

        public void ProcesoImproductivos_actualizaPrestamos(int dias_vencimiento)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@usuario", "ALERTAS PROCESO");
            sPar[1] = new DBHelper.StoredProcedureParameter("@dias_vencimiento", dias_vencimiento);

            try
            {

                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("ALERTAS_IMPRODUCTIVOS_ACTUALIZAR_PRESTAMOS", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Proceso Alertas", "Error ejecutando proceso alertas", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }

        public void ProcesoImproductivos_PrestamosVencidos(int dias_vencimiento)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@DIAS", dias_vencimiento);

            try
            {

                oDb.ExecuteProcedureNonQuery("ALERTAS_Prestamos_Vencidos", sPar);
                oDb.ExecuteProcedureNonQuery("ALERTAS_Prestamos_Vencidos_Proceso", sPar);
                oDb.ExecuteProcedureNonQuery("ALERTAS_ClientesAlertasPrestamosVencidos_SP");

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Proceso Alertas", "Error ejecutando proceso alertas", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }

        public void ImproductivosAgregarRepartos (string repartos)
        {
            DBHelper oDb = new DBHelper();
            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            string script = "";

            oDb.ExecuteSqlNonQuery("delete from alertas_imp_repartos");
            foreach (string reparto in repartos.Split(';'))
            {
                try
                {
                    if (reparto.Trim().Length > 0)
                    {
                        script = "insert into alertas_imp_repartos values (" + reparto + ")";
                        oDb.ExecuteSqlNonQuery(script);
                    }
                }
                catch (Exception ex)
                {
                    log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Proceso Alertas", "Error ejecutando proceso alertas", ex);
                }
                finally
                {
                    oDb.Disconnect();
                }

            }

        }
        public string  ImproductivosGetRepartos()
        {
            DBHelper oDb = new DBHelper();
            DataSet oDs = new DataSet();
            string resultado = "";
            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("select idreparto from alertas_imp_repartos");
                foreach (DataRow reparto in oDs.Tables[0].Rows)
                {
                    resultado += reparto["idreparto"].ToString() + ";";

                }
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Proceso Alertas", "Error ejecutando proceso alertas", ex);
            }
            finally
            {
                oDb.Disconnect();
            }

            return resultado;

        }

    }
}
