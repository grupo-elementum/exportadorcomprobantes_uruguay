using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsDatos;

namespace CsNegocio
{
    public class Configuracion
    {
        #region Declaraciones
        private double _Codigo;
        private string _Tarea;
        private Int32 _Tomado;
        private DateTime _Programacion;
        private string _Tarea2;
        private string _Tarea3;
        private string _Tarea4;
        #endregion

        #region Constructores
        public Configuracion(double Codigo, string Tarea, Int32 Tomado, DateTime Programacion, string Tarea2, string Tarea3, string Tarea4)
        {
            _Codigo = Codigo;
            _Tarea = Tarea;
            _Tomado = Tomado;
            _Programacion = Programacion;
            _Tarea2 = Tarea2;
            _Tarea3 = Tarea3;
            _Tarea4 = Tarea4;
        }
        public Configuracion(string Tarea)
        {
            _Tarea = Tarea;
        }
        public Configuracion()
        {
        }
        #endregion

        #region Propiedades

        public double Codigo
        {
            get { return _Codigo; }
            set { this._Codigo = value; }
        }


        public string Tarea
        {
            get { return _Tarea; }
            set { this._Tarea = value; }
        }


        public Int32 Tomado
        {
            get { return _Tomado; }
            set { this._Tomado = value; }
        }


        public DateTime Programacion
        {
            get { return _Programacion; }
            set { this._Programacion = value; }
        }


        public string Tarea2
        {
            get { return _Tarea2; }
            set { this._Tarea2 = value; }
        }


        public string Tarea3
        {
            get { return _Tarea3; }
            set { this._Tarea3 = value; }
        }


        public string Tarea4
        {
            get { return _Tarea4; }
            set { this._Tarea4 = value; }
        }
        #endregion

        #region Metodos
        public void InicializarObjeto()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Tarea", _Tarea);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("EXPORTADOR_AFIP_Inicializar_Tareas", sPar);
                _Codigo = double.Parse(oDs.Tables[0].Rows[0]["codigo"].ToString());
                _Tarea = oDs.Tables[0].Rows[0]["tarea"].ToString();
                _Tomado = Int32.Parse(oDs.Tables[0].Rows[0]["tomado"].ToString());
                _Programacion = DateTime.Parse(oDs.Tables[0].Rows[0]["programacion"].ToString());
                _Tarea2 = oDs.Tables[0].Rows[0]["tarea2"].ToString();
                _Tarea3 = oDs.Tables[0].Rows[0]["tarea3"].ToString();
                _Tarea4 = oDs.Tables[0].Rows[0]["tarea4"].ToString();
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "InicializarObjeto", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void Guardar()
        {
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[7];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Codigo", _Codigo);
            sPar[1] = new DBHelper.StoredProcedureParameter("@Tarea", _Tarea);
            sPar[2] = new DBHelper.StoredProcedureParameter("@Tomado", _Tomado);
            sPar[3] = new DBHelper.StoredProcedureParameter("@Prog", _Programacion);
            sPar[4] = new DBHelper.StoredProcedureParameter("@Tarea2", _Tarea2);
            sPar[5] = new DBHelper.StoredProcedureParameter("@Tarea3", _Tarea3);
            sPar[6] = new DBHelper.StoredProcedureParameter("@Tarea4", _Tarea4);

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("EXPORTADOR_AFIP_Guardar_Tareas", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Guardar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public bool Activo()
        {
            if (_Tomado == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Desactivar()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Codigo", _Codigo);
            sPar[1] = new DBHelper.StoredProcedureParameter("@Tomado", _Tomado);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("EXPORTADOR_AFIP_Desactivar_Tarea", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Desactivar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public bool Abierto()
        {
            bool Activo = false;
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT TOMADO FROM ConfigExportadorAfip WHERE TAREA = 'FacturaElectronica'");
                foreach (DataRow row in oDs.Tables[0].Rows)
                {
                    if (row["Tomado"].ToString() == "1")
                    {
                        Activo = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Abierto", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return Activo;
        }
        public DateTime UltimaEjecucion()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT ULTIMAEJECUCION = ISNULL(ULTIMAEJECUCION,0) FROM ConfigExportadorAfip WHERE CODIGO =" + _Codigo);

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "UltimaEjecucion", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return DateTime.Parse(oDs.Tables[0].Rows[0]["ultimaEjecucion"].ToString());
        }
        public bool Corrio()
        {
            bool Corrio = false;
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT CORRIO = ISNULL(CORRIO,0) FROM ConfigExportadorAfip WHERE CODIGO = " + _Codigo);
                foreach (DataRow row in oDs.Tables[0].Rows)
                {
                    if (row["Corrio"].ToString() == "1")
                    {
                        Corrio = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Corrio", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return Corrio;
        }
        public void SiCorrio()
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteSqlNonQuery("UPDATE ConfigExportadorAfip SET CORRIO = 1, ULTIMAEJECUCION = GETDATE() WHERE CODIGO = " + _Codigo);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "SiCorrio", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void NoCorrio()
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                if (DateTime.Parse(UltimaEjecucion().ToShortDateString()) <= DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    oDb.ExecuteSqlNonQuery("UPDATE ConfigExportadorAfip SET CORRIO = 0 WHERE CODIGO = " + _Codigo);
                }
                else
                {
                    oDb.ExecuteSqlNonQuery("UPDATE ConfigExportadorAfip SET CORRIO = 0, ULTIMAEJECUCION = GETDATE()-1 WHERE CODIGO = " + _Codigo);
                }

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "NoCorrio", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public DataSet ObtenerEmpresasParametrosFE()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT * FROM PARAMETROSFE ORDER BY ID");
            }
            catch(Exception ex)
            {

            }
            finally
            {
                oDb.Disconnect();
            }

            return oDs;
        }
        public void InsertarFacturaCae(string codmov, long nromov, long cae, string fechaCae, string vencimientoCae)
        {
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[5];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@cae", cae);
            try
            {
                //Se usa para insertar las facturas con detalle, la fecha que devuelve es diferente al otro servicio, entonces intenta convertir la fecha si no puede las manda directamente.
                sPar[3] = new DBHelper.StoredProcedureParameter("@fechacae", Convert.ToDateTime(fechaCae));
                sPar[4] = new DBHelper.StoredProcedureParameter("@vencimientoCae", Convert.ToDateTime(vencimientoCae));
            }
            catch (Exception)
            {

                sPar[3] = new DBHelper.StoredProcedureParameter("@fechacae", fechaCae);
                sPar[4] = new DBHelper.StoredProcedureParameter("@vencimientoCae", vencimientoCae);
            }            

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("Insertar_Factura_Cae", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Insertando Cae Factura", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void InsertarFacturaRequest(string codmov, long nromov, long idcomprobante, string request)
        {
            DBHelper oDb = new DBHelper();            

            try
            {
                DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
                sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
                sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);
                sPar[2] = new DBHelper.StoredProcedureParameter("@idcomprobante", idcomprobante);
                sPar[3] = new DBHelper.StoredProcedureParameter("@request", request);

                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("insertar_request_facturante", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Insertando Request Factura", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void InsertarFacturaComprobante(string codmov, long nromov, long cae, string fechaCae, string vencimientoCae, long idcomprobante, long nromov_nuevo)
        {
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[7];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@cae", cae);
            try
            {
                //Se usa para insertar las facturas con detalle, la fecha que devuelve es diferente al otro servicio, entonces intenta convertir la fecha si no puede las manda directamente.
                sPar[3] = new DBHelper.StoredProcedureParameter("@fechacae", Convert.ToDateTime(fechaCae));
                sPar[4] = new DBHelper.StoredProcedureParameter("@vencimientoCae", Convert.ToDateTime(vencimientoCae));
            }
            catch (Exception)
            {

                sPar[3] = new DBHelper.StoredProcedureParameter("@fechacae", fechaCae);
                sPar[4] = new DBHelper.StoredProcedureParameter("@vencimientoCae", vencimientoCae);
            }

            sPar[5] = new DBHelper.StoredProcedureParameter("@idcomprobante", idcomprobante);
            sPar[6] = new DBHelper.StoredProcedureParameter("@nromov_nuevo", nromov_nuevo);

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("Insertar_Factura_Cae", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Insertando Cae Factura", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void InsertarFacturaError(string codmov, long nromov, string obs, string desc)
        {
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@obs", obs);
            sPar[3] = new DBHelper.StoredProcedureParameter("@desc", desc);            

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("Insertar_Factura_CaeError", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Insertando Cae Factura", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public string  InsertarExportadorError(string codmov, string nromov, string obs, string tipo_error,int notificado)
        {
            DBHelper oDb = new DBHelper();
            DataSet oDs = new DataSet();
            string resultado = "NOTIFICAR";

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[5];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@obs", obs);
            sPar[3] = new DBHelper.StoredProcedureParameter("@tipo_error", tipo_error);
            sPar[4] = new DBHelper.StoredProcedureParameter("@notifico", notificado);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet  ("EXPORTADOR_AFIP_ACTUALIZAR_ERROR", sPar);
                if (oDs.IsInitialized)
                {
                    resultado = oDs.Tables[0].Rows[0]["resultado"].ToString();
                }
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "EXPORTADOR_AFIP_ACTUALIZAR_ERROR", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return resultado;
        }
        public void NotificoExportadorError(string codmov, string nromov,string tipo_error)
        {
            DBHelper oDb = new DBHelper();
            string sSql = "";
            
            try
            {
                sSql = "UPDATE EXPORTADOR_AFIP_ERRORES SET NOTIFICADO = 1 WHERE CODMOV = '"+codmov+"'";
                sSql = sSql + " AND NROMOV =" + nromov + " AND TIPO_ERROR = '" + tipo_error + "'";

                oDb.Connect();
                oDb.ExecuteSqlNonQuery(sSql);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "NotificoExportadorError", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }

        public DataSet ObtenerFacturaTxt()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
                sPar[0] = new DBHelper.StoredProcedureParameter("@TIPO", "RE");
                sPar[1] = new DBHelper.StoredProcedureParameter("@INTERNOID", 0);
                sPar[2] = new DBHelper.StoredProcedureParameter("@CODMOV", "");
                sPar[3] = new DBHelper.StoredProcedureParameter("@NROMOV", 0);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("OBTENER_LINEA_TXT_FE", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Obtener Factura Txt (logico)", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ObtenerFacturasExportar_facturante(long id)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();

                DBHelper.StoredProcedureParameter[] sPar2 = new DBHelper.StoredProcedureParameter[1];
                sPar2[0] = new DBHelper.StoredProcedureParameter("@InternoID", id);

                oDs = oDb.ExecuteProcedureAsDataSet("ProcesoExportacionFacturas_facturante", sPar2);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Procesar Facturas FE", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ObtenerFacturasParaRevisarResultados_facturante(long id)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();

                DBHelper.StoredProcedureParameter[] sPar2 = new DBHelper.StoredProcedureParameter[1];
                sPar2[0] = new DBHelper.StoredProcedureParameter("@InternoID", id);

                oDs = oDb.ExecuteProcedureAsDataSet("ObtenerFacturasParaRevisarResultados_facturante", sPar2);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Procesar Facturas FE", ex);

            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ObtenerFacturasExportar(long id)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Tipo", "0");
            sPar[1] = new DBHelper.StoredProcedureParameter("@Tarea2", _Tarea2);
            sPar[2] = new DBHelper.StoredProcedureParameter("@Tarea3", _Tarea3);
            sPar[3] = new DBHelper.StoredProcedureParameter("@InternoID", id);

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("ProcesoExportacionFacturas", sPar);

                DBHelper.StoredProcedureParameter[] sPar2 = new DBHelper.StoredProcedureParameter[4];
                sPar2[0] = new DBHelper.StoredProcedureParameter("@Tipo", "1");
                sPar2[1] = new DBHelper.StoredProcedureParameter("@Tarea2", _Tarea2);
                sPar2[2] = new DBHelper.StoredProcedureParameter("@Tarea3", _Tarea3);
                sPar2[3] = new DBHelper.StoredProcedureParameter("@InternoID", id);

                oDs = oDb.ExecuteProcedureAsDataSet("ProcesoExportacionFacturas", sPar2);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Procesar Facturas FE", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ObtenerRemitosExportar(long id)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Tipo", "0");
            sPar[1] = new DBHelper.StoredProcedureParameter("@Tarea2", _Tarea2);
            sPar[2] = new DBHelper.StoredProcedureParameter("@Tarea3", _Tarea3);
            sPar[3] = new DBHelper.StoredProcedureParameter("@InternoID", id);

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("ProcesoExportacionRemitos", sPar);

                DBHelper.StoredProcedureParameter[] sPar2 = new DBHelper.StoredProcedureParameter[4];
                sPar2[0] = new DBHelper.StoredProcedureParameter("@Tipo", "1");
                sPar2[1] = new DBHelper.StoredProcedureParameter("@Tarea2", _Tarea2);
                sPar2[2] = new DBHelper.StoredProcedureParameter("@Tarea3", _Tarea3);
                sPar2[3] = new DBHelper.StoredProcedureParameter("@InternoID", id);

                oDs = oDb.ExecuteProcedureAsDataSet("ProcesoExportacionRemitos", sPar2);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Procesar Remitos FE", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }

        public DataSet ObtenerComprobantes_Actualizar(long id)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@InternoID", id);

            try
            {
                oDb.Connect();
                oDs =oDb.ExecuteProcedureAsDataSet  ("Elect_comprobantes_actualizar_listar", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "ObtenerComprobantes_Actualizar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }

        public DataSet ObtenerComprobantes_Eliminados(long id)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@InternoID", id);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Elect_comprobantes_eliminados_listar", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "ObtenerComprobantes_Eliminados", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }

        public void EscribirResultadoFE(string tipo, long id, string codmov, long nromov) 
        {
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Tipo", tipo);
            sPar[1] = new DBHelper.StoredProcedureParameter("@InternoID", id);
            sPar[2] = new DBHelper.StoredProcedureParameter("@Codmov", codmov);
            sPar[3] = new DBHelper.StoredProcedureParameter("@Nromov", nromov);

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("OBTENER_LINEA_TXT_FE", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Escribir Resultado", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public DataSet getResultadoFE(string tipo, long id, string codmov, long nromov)
        {
            DBHelper oDb = new DBHelper();
            DataSet oDs = new DataSet();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Tipo", tipo);
            sPar[1] = new DBHelper.StoredProcedureParameter("@InternoID", id);
            sPar[2] = new DBHelper.StoredProcedureParameter("@Codmov", codmov);
            sPar[3] = new DBHelper.StoredProcedureParameter("@Nromov", nromov);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("FE_OBTENER_DATOS_FACTURA", sPar);
            oDb.Disconnect();

            return oDs;
        }
        public DataSet getResultadoFE_Remito(string tipo, long id, string codmov, long nromov)
        {
            DBHelper oDb = new DBHelper();
            DataSet oDs = new DataSet();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Tipo", tipo);
            sPar[1] = new DBHelper.StoredProcedureParameter("@InternoID", id);
            sPar[2] = new DBHelper.StoredProcedureParameter("@Codmov", codmov);
            sPar[3] = new DBHelper.StoredProcedureParameter("@Nromov", nromov);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("FE_OBTENER_DATOS_REMITO", sPar);
            oDb.Disconnect();

            return oDs;
        }

        public void FinalizarFacturaFE(string codmov, long nromov)
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteSqlNonQuery("UPDATE FACTURASEXPORTADASFE SET EXPORTADA = 1 WHERE CODMOV = '" 
                                    + codmov + "'AND NROMOV=" + nromov.ToString());
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Escribir Resultado", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void EscribirTxt(string BD)
        {
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@BD", BD);

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("EXPORTARFACTURASTXT", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Escribir Txt", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void ProcesarArchivosCae()
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("OBTENERRESULTADOSCAE");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Procesando Archivos Cae", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void ProcesarArchivosError()
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("OBTENERRESULTADOSERROR");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Procesando Archivos Error", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void ProcesarCaeFacturas()
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("INSERTAR_FACTURA_RESULTADO");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Procesando Cae Facturas", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void ProcesarErrorFacturas()
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("INSERTAR_FACTURA_ERROR");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Procesando Error Facturas", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void ejecutarScript(string script)
        {
            DBHelper oDb = new DBHelper();
            oDb.Connect();
            oDb.ExecuteSqlNonQuery(script);
            oDb.Disconnect();
        }
        public DataSet GetParametrosEmpresa()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper(1);
            string[] linStr;
            string bdOpe = "";
            string script = "";

            foreach (string linea in ConfigurationSettings.AppSettings["ConnectionString"].Split(';'))
            {
                linStr = linea.Split('=');
                if (linStr[0].ToString() == "Initial Catalog")
                {
                    bdOpe = linStr[1].ToString().Trim();
                    break;
                }
            }


            try
            {
                script = "select * from parametros where baseda = '" + bdOpe + "'";

                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet(script);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Parametros Entrada", "Error ejecutando Script", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public bool factura_puedo_enviar(string codmov, string nromov)
        {
            DBHelper oDb = new DBHelper();
            DataSet oDs = new DataSet();
            bool puedoenviar = true;

            try
            {
                string script = "";

                script = "select idcomprobante = isnull(idcomprobante,0) from facturascaefe where codmov ='" + codmov + "' and nromov =" + nromov;

                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet(script);
                if (oDs.IsInitialized)
                {
                    if (oDs.Tables[0].Rows.Count > 0)
                    {
                        if (long.Parse(oDs.Tables[0].Rows[0]["idcomprobante"].ToString()) == 0)
                        {
                            puedoenviar = false;
                        }
                    }
                }
                oDb.Disconnect();
            }
            catch (Exception ex)
            {
                puedoenviar = false;

            }

            return puedoenviar;
        
        }
        public bool factura_con_error(string codmov, string nromov)
        {
            DBHelper oDb = new DBHelper();
            DataSet oDs = new DataSet();
            bool factura_con_error = false;

            try
            {
                string script = "";

                script = "select error = count(*) from facturasanulafe where codmov ='" + codmov + "' and nromov =" + nromov;

                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet(script);
                if (oDs.IsInitialized)
                {
                    if (long.Parse(oDs.Tables[0].Rows[0]["error"].ToString()) > 0)
                    {
                        factura_con_error = true;
                    }
                }
                oDb.Disconnect();
            }
            catch (Exception ex)
            {
                factura_con_error = false;

            }

            return factura_con_error;

        }
        public void borrarHistorialErroresyRequestFE()
        {
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteProcedureNonQuery("FE_LIMPIAR_HISTORIAL_ERRORES_REQUEST");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Configuracion", "Limpiando Historial", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        #endregion
    }
}
