using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsDatos;

namespace CsNegocio
{
    public class Mensajes
    {
        #region Declaraciones

        private double _IdLlamada;
        private double _Nrocta;
        private string _RazonSocial;
        private string _Direccion;
        private string _Observacion;
        private int _IdReparto;

        #endregion

        public Mensajes()
        {}

        #region Metodos

        public DataSet Listar()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Mensajes_H2O_Listar");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ListarAndroide()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Mensajes_H2O_Listar_Androide");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "ListarAndroide", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet listarEliminadasAndroide()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT ID_API FROM ANDROIDE_MENSAJES WHERE ISNULL(ELIMINADO,0) = 1");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ListarClientesMensajesAndroide(int company_id)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
                sPar[0] = new DBHelper.StoredProcedureParameter("@company_id", company_id);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("ANDROIDE_MENSAJES_CLIENTES",sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "ListarClientesMensajesAndroide", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public void actualiza_clientes_idapi(string nrocta, string id_api)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
                sPar[0] = new DBHelper.StoredProcedureParameter("@nrocta", nrocta);
                sPar[1] = new DBHelper.StoredProcedureParameter("@id_api", id_api);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("ANDROIDE_ACTUALIZA_ID_CLIENTES", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "actualiza_clientes_idapi", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void eliminarMensajesAndroide()
        {            
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteSqlNonQuery("DELETE FROM ANDROIDE_MENSAJES WHERE ISNULL(ELIMINADO,0) = 1");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }            
        }
        public DataSet ListarLlamada(double idllamada)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Idllamada", idllamada);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Mensajes_H2O_Listar", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "ListarLlamada", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public void ModificarFechaWeb (double idllamada, int nritem)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Idllamada", idllamada);
            sPar[1] = new DBHelper.StoredProcedureParameter("@Nritem", nritem);

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("Mensajes_H2O_Modificar_Fecha_Web", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "ModificarFechaWeb", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public string InsertarMensajeResultado(double idllamada, int nritem, string accion, bool modif)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Idllamada", idllamada);
            sPar[1] = new DBHelper.StoredProcedureParameter("@Nritem", nritem);
            sPar[2] = new DBHelper.StoredProcedureParameter("@accion", accion);
            sPar[3] = new DBHelper.StoredProcedureParameter("@modifica", modif);
            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Mensajes_H2O_Insertar", sPar);

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "InsertarMensajeResultado", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs.Tables[0].Rows[0][0].ToString();
        }
        public void ActualizarStatus(double idllamada, int idLlamadaEstado)
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteSqlNonQuery("UPDATE LLAMADAS SET STATUS = '" + idLlamadaEstado.ToString() + 
                                            "' WHERE IDLLAMADA = " + idllamada);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "ActualizarStatus", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public bool ExisteLlamadaWeb(double idmensaje, int idreparto)
        {
            bool bValida = false;
            DataSet oDs;
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT IDMENSAJE = ISNULL(IDMENSAJE,0) FROM LLAMADAS_WEB WHERE IDMENSAJE = " + 
                                idmensaje.ToString() + " AND IDREPARTO = " + idreparto.ToString());
                
                foreach (DataRow row in oDs.Tables[0].Rows)
                {
                    if (row["idmensaje"].ToString() == idmensaje.ToString())
                    {
                        bValida = true;
                        //_IdCliente = double.Parse(oDr["IdCliente"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "ExisteLlamadaWeb", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return bValida;
        }
        public void InsertarLlamadaWeb(double idmensaje, int idreparto, string mensaje, string usuario)
        {
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@idmensaje", idmensaje);
            sPar[1] = new DBHelper.StoredProcedureParameter("@idreparto", idreparto);
            sPar[2] = new DBHelper.StoredProcedureParameter("@mensaje", mensaje);
            sPar[3] = new DBHelper.StoredProcedureParameter("@usuario", usuario);

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("MENSAJES_H2O_INSERTAR_LLAMADA_WEB",sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Mensajes", "InsertarLlamadaWeb", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }

        #endregion

    }
}
