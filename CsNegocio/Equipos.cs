using CsDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CsNegocio
{
    public class Equipos
    {
        #region Metodos

        public bool ModuloHabilitado()
        {
            bool bValida = false;
            DataSet oDs;
            DBHelper oDb = new DBHelper(1);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT HABILITA_FRIO_CALOR = MAX(ISNULL(HABILITA_FRIO_CALOR,0)) FROM PARAMETROS");

                if (oDs.Tables[0].Rows[0]["Habilita_Frio_Calor"].ToString()!="0")
                {
                    bValida = true;
                    //_IdCliente = double.Parse(oDr["IdCliente"].ToString());
                }

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Equipos", "ModuloHabilitado", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return bValida;
        }
        public DataSet Listar()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT * FROM REPARTOSRECARGASEQUIPOS " +
                                                    "WHERE FCHREP >= CONVERT(VARCHAR(10),GETDATE(),103) "+
                                                    "AND ANULADO = 0 ORDER BY IDREPARTO,IDPRODUCTO_EQUIPO ");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Equipos", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ListarMotivosNoCompra()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT * FROM MOTIVOS");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Equipos", "ListarMotivosNoCompra", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public string InsertarMotivoNoCompra(double idmotivo, string descripcion, int orden, int cierra_llamado)
        {
            string result = "OK";
            DBHelper oDb = new DBHelper();
            try
            {

                oDb.Connect();

                oDb.ExecuteSqlNonQuery("INSERT INTO MOTIVOS(IDMOTIVO,DESCRIPCION,ORDEN,CIERRA_LLAMADO) " +
                          "VALUES ("+ idmotivo.ToString() +",'"+descripcion+"',"+orden.ToString()+","+cierra_llamado.ToString()+")");


            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "EQUIPOS", "InsertarMotivoNoCompra", ex);
            }
            finally
            {
                oDb.Disconnect();
            }

            return result;
        }
        public void ModificaMotivoNoCompra(double idmotivo, string descripcion, int orden, int cierra_llamado)
        {
            DBHelper oDb = new DBHelper();
            try
            {

                oDb.Connect();

                oDb.ExecuteSqlNonQuery("UPDATE MOTIVOS SET CIERRA_LLAMADO = "+cierra_llamado.ToString()+ 
                            " WHERE IDMOTIVO = "+ idmotivo.ToString());


            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "EQUIPOS", "InsertarMotivoNoCompra", ex);
            }
            finally
            {
                oDb.Disconnect();
            }

        }        
        public string InsertarMotivoRecambio(double idmotivo, string descripcion, int orden)
        {
            string result = "OK";
            DBHelper oDb = new DBHelper();
            try
            {

                oDb.Connect();

                oDb.ExecuteSqlNonQuery("INSERT INTO MOTIVOS_RECAMBIO(IDMOTIVO,DESCRIPCION,ORDEN) " +
                          "VALUES (" + idmotivo.ToString() + ",'" + descripcion + "'," + orden.ToString() + ")");


            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "EQUIPOS", "InsertarMotivoNoCompra", ex);
            }
            finally
            {
                oDb.Disconnect();
            }

            return result;
        }
        public string EliminarMotivoNoCompra(double idmotivo)
        {
            string result = "OK";
            DBHelper oDb = new DBHelper();
            try
            {

                oDb.Connect();

                oDb.ExecuteSqlNonQuery("DELETE FROM MOTIVOS WHERE IDMOTIVO = " + idmotivo.ToString() + 
                            " and (select count(*) from NoCompradores where idMotivo = motivos.idmotivo) = 0");


            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "EQUIPOS", "EliminarMotivoNoCompra", ex);
            }
            finally
            {
                oDb.Disconnect();
            }

            return result;
        }
        public string EliminarMotivoRecambio(double idmotivo)
        {
            string result = "OK";
            DBHelper oDb = new DBHelper();
            try
            {

                oDb.Connect();

                oDb.ExecuteSqlNonQuery("DELETE FROM MOTIVOS_RECAMBIO WHERE IDMOTIVO = " + idmotivo.ToString() + 
                        " and (select count(*) from recambios where motivo_recambio = motivos_recambio.idmotivo) = 0");


            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "EQUIPOS", "EliminarMotivoRecambios", ex);
            }
            finally
            {
                oDb.Disconnect();
            }

            return result;
        }
        public DataSet ListarMotivosRecambios()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT * FROM MOTIVOS_RECAMBIO WHERE NOT DESCRIPCION LIKE '%RECAMBIO FABRICA%'");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Equipos", "ListarMotivosNoCompra", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ListarEquiposFCAcciones()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Equipos_FC_Listar_Acciones");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Equipos", "ListarEquiposFCAcciones", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ListarEquiposFCPrecios()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Equipos_FC_Acciones_Precios");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Equipos", "ListarEquiposFCPrecios", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }

        #endregion
    }
}
